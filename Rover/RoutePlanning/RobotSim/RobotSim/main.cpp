// 4/10/19 - Tadeusz Pforte
// This is a hot pile of garbage that somehow works sometimes
#include <SFML/Audio.hpp>
#include <SFML/Graphics.hpp>

#include <cmath>
#include <queue>
#include <stdio.h>
#include <stdlib.h>
#include <string>
#include <time.h>
#include <chrono>
#include <random>

#include "ResourcePath.hpp" // Imports resourcePath() for macos
#include "Simulator.hpp"
#include "grid.hpp"
#include "simController.hpp"
#include "ui.hpp"
#include "interface.hpp"
#include "RobotEKF.hpp"

#include "../../../ZedDepth/zed-depth.h"

std::default_random_engine generator;
std::normal_distribution<double> distribution(0.0,3.0);

#ifndef LOCAL
#include "WorldCommunicator.hpp"
#endif

#if defined(_WIN32) || defined(__linux__) || defined(__unix__)
const std::string RESOURCE_DIR = "./Resources/";
#elif __APPLE__
const std::string RESOURCE_DIR = resourcePath();
#endif

// ---------------------------------------- //
// ---------- Internal Variables ---------- //
// ---------------------------------------- //
const sf::Color bgColor = sf::Color(255, 255, 255);

constexpr float RECOMPUTE_COOLDOWN = 1.5f; // time between recomputation of the path/graph

static inline sf::CircleShape getNode(RP::node, float scale, float height);

void draw_qtree(sf::RenderWindow &win, const RP::QTreeNode &node, float scale,
                float height);

#ifndef LOCAL
WorldCommunicator worldCommunicator;
#endif
static float goalDirection;
static float toMove;

RobotEKF filter;

static void zed_depth_init()
{
    zdInit();
}

// ---------------------------------------- //

// ---------------------------------------- //
// ---------- BeagleBone Methods ---------- //
// ---------------------------------------- //

// Call methods using the Interface beaglebone

// ---------------------------------------- //

int main(int, char const **)
{
    zed_depth_init();
    // ---------------------------------------- //
    // ----------- SFML Window Setup ---------- //
    // ---------------------------------------- //
    unsigned int screenWidth = sf::VideoMode::getDesktopMode().width;
    unsigned int screenHeight = sf::VideoMode::getDesktopMode().height;

    float WINDOW_SCALE = .67f;
    if (screenHeight < 1000)
        WINDOW_SCALE = .34f;
    else if (screenHeight > 1500)
        WINDOW_SCALE = 1.f;

    std::cout << "screen detected: " << screenWidth << "x" << screenHeight << std::endl;
    std::cout << "window scale set to " << WINDOW_SCALE << std::endl;

    sf::RenderWindow window(sf::VideoMode(1476 * WINDOW_SCALE, 1576 * WINDOW_SCALE), "Robot Simulator");
    window.setFramerateLimit(60);

    // application icon
    sf::Image icon;
    if (icon.loadFromFile(RESOURCE_DIR + "HuskyRoboticsLogo.png"))
        window.setIcon(icon.getSize().x, icon.getSize().y, icon.getPixelsPtr());

    // used to render text
    sf::Font font;
    if (!font.loadFromFile(RESOURCE_DIR + "DejaVuSans.ttf"))
        std::cout << "Failed to load font" << std::endl;

    // used to render sprites
    sf::RenderTexture g_rendertexture;
    // ---------------------------------------- //

    // --------- Initial Configuration -------- //
    const std::string OBSTACLES_FILE = RESOURCE_DIR + "obstacles.txt";

    float gridWidth = 40.f;
    float gridHeight = 40.f;
    float gridScale = 36 * WINDOW_SCALE;

    RP::point agentInitPos{2.5f, 2.5f};
    float agentInitRot = 45.f;
    // ---------------------------------------- //

    Grid grid(gridWidth, gridHeight, gridScale);
    grid.target = RP::point{35.f, 35.f}; // sets the autonomous target

    Agent agent(gridScale, gridWidth, gridHeight, agentInitPos, agentInitRot);
    agent.bot_width = 1.8f;
    // agent.scaleSpeed(2.f);

    // Use to send commands to the agent and get world information
    // move(speed), turn(speed), turnTo(targetDir)
    // currentPosition(), currentRotation(), currentObstaclesInView()
    Interface beaglebone(grid, agent);

    RP::Simulator sim(grid.obstacleList, agent, RP::simulator_config{70.f, 10.f}, gridScale, gridHeight);
    RP::Pather pather(sim.getpos(), grid.target, RP::point{39.f, 39.f});
    RP::SimController control(grid, agent, pather);

    // used to control how often the simulator recomputes the graph
    RP::Timer recompute_timer;

    // ---------- Application Toggles --------- //

    // if true, display calculated path to target
    bool lazer = false;

    // if true, display the trimmed pathing graph
    bool showGraph = false;

    // if true, autonomously navigate to target
    bool auton = false;
    // ---------------------------------------- //

    // -------------- FPS Display ------------- //
    //fps tracker (time code stolen from SO)
    sf::Text fpsCounter(" 0 fps", font, 24);
    fpsCounter.setFillColor(sf::Color::Black);
    fpsCounter.move(2.f, 2.f);

    unsigned int now = std::chrono::duration_cast<std::chrono::milliseconds>(std::chrono::system_clock::now().time_since_epoch()).count();
    int frameCount = 0;
    bool gibFPS = true;
    // ---------------------------------------- //

    // ---------------------------------------- //
    // ---------- 60 FPS Update Loop ---------- //
    // ---------------------------------------- //
    Kalman::KMatrix<float, 1, true> P0(STATE_VEC_SIZE, STATE_VEC_SIZE);
    Kalman::KVector<float, 1, true> x(STATE_VEC_SIZE);

    filter.setP0(P0);

    x(1) = agent.getPosition().x;
    x(2) = agent.getPosition().y;
    x(3) = 0;
    x(4) = 0;

    filter.init(x, P0);
    while (window.isOpen()) {
        
        sf::Event event;

        while (window.pollEvent(event))
        {

            // Close window on X or Cmd+W
            if (event.type == sf::Event::Closed)
                window.close();

            else if (event.type == sf::Event::KeyPressed)
            {
                switch (event.key.code)
                {
                case sf::Keyboard::H:
                {
                    std::cout << "Help Menu: " << std::endl;
                    std::cout
                        << "P   -- Returns the internal position of the robot"
                        << std::endl;
                    std::cout << "G      -- Toggle grid" << std::endl;
                    std::cout << "O      -- Import obstacles from obstacles.txt" << std::endl;
                    std::cout << "U      -- Complete autonomous mode" << std::endl;
                    std::cout << "N      -- Toggle clipping" << std::endl;
                    std::cout << "E      -- Show pathing graph" << std::endl;
                    std::cout << "0      -- Toggle robot path" << std::endl;
                    std::cout << "1      -- Toggle fps counter" << std::endl;
                    std::cout << "9      -- Draw algorithm path" << std::endl;
                    std::cout << "Ctrl = -- Resets the board" << std::endl;
                    break;
                }
                case sf::Keyboard::P:
                {
                    std::cout << "Internal Position: (" << agent.getX() << ","
                              << agent.getY() << ") at "
                              << agent.getInternalRotation() << " degrees"
                              << std::endl;
                    break;
                }
                case sf::Keyboard::G:
                {
                    grid.toggleGrid();
                    break;
                }
                case sf::Keyboard::O:
                {
                    grid.obstacleList.clear();
                    grid.readObstaclesFromFile(OBSTACLES_FILE);
                    // grid.addBorderObstacles();
                    std::cout << "Added obstacles" << std::endl;
                    break;
                }
                case sf::Keyboard::U:
                {
                    auton = !auton;
                    if (auton)
                        control.start_auto();
                    else
                        control.stop_auto();
                    break;
                }
                case sf::Keyboard::N:
                {
                    grid.toggleClipping();
                    break;
                }
                case sf::Keyboard::E:
                {
                    showGraph = !showGraph;
                    break;
                }
                case sf::Keyboard::Num9:
                {
                    if (lazer)
                        grid.drawPath();
                    lazer = !lazer;
                    break;
                }
                case sf::Keyboard::Num0:
                {
                    agent.togglePath();
                    break;
                }
                case sf::Keyboard::Num1:
                {
                    gibFPS = !gibFPS;
                    break;
                }
                case sf::Keyboard::Equal:
                {
                    if (sf::Keyboard::isKeyPressed(sf::Keyboard::LControl) || sf::Keyboard::isKeyPressed(sf::Keyboard::RControl))
                    {
                        // reset everything
                        std::cout << "Resetting everything to initial configuration" << std::endl;

                        agent.resetTo(agentInitPos, agentInitRot);
                        grid.obstacleList.clear();
                        grid.drawPath();

                        pather.reset();

                        lazer = false;
                        showGraph = false;

                        if (auton)
                            control.stop_auto();
                        auton = false;
                    }
                }
                default:
                {
                    //std::cout << "Command not recognized" << std::endl;
                    break;
                }
                }
            }
        }

        if (sf::Keyboard::isKeyPressed(sf::Keyboard::W)) // move forwards
            beaglebone.move(1);
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::S)) // move backwards
            beaglebone.move(-1);
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::A)) // turn left
            beaglebone.turn(-1);
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::D)) // turn right
            beaglebone.turn(1);

        window.clear(bgColor);

        if (auton)
            control.tic();

        sim.update_agent();

        float change = 0.0;
#ifndef LOCAL
        worldCommunicator.update(beaglebone.currentPosition(), beaglebone.currentRotation(), change, toMove, sim.visible_obstacles());

        beaglebone.turnTo(goalDirection);
        beaglebone.move(toMove);
#endif
        goalDirection += change;
        goalDirection = (int)goalDirection % 360;

        RP::point errored = sim.getpos();
        errored.x = errored.x + distribution(generator);//((float)rand()/RAND_MAX-0.5)*1.8*4;
        errored.y = errored.y + distribution(generator);//((float)rand()/RAND_MAX-0.5)*1.8*4;

        std::cout << "errored x: " << errored.x << " y: " << errored.y << std::endl;
       
        sf::CircleShape errored_circle(10);
        errored_circle.setOrigin(10, 10);
        errored_circle.setFillColor(GRAPH_NODE_COLOR);
        errored_circle.setPosition((errored.x + 1) * gridScale, (gridHeight - errored.y) * gridScale);
        window.draw(errored_circle);   

        

        RP::point filterOutput;
        Kalman::KVector<float, 1, true> z(4);
        Kalman::KVector<float, 1, true> u(2);
        z(1) = errored.x;
        z(2) = errored.y;
        // std::cout << "Success: Initialized the things" << std::endl;
        filter.step(u, z);
        filterOutput.x = filter.getX()(1);
        filterOutput.y = filter.getX()(2);
        std::cout << "Kalman says: lat: " << filterOutput.x << " lng: " << filterOutput.y << std::endl;
        sf::CircleShape filter_circle(10);
        filter_circle.setOrigin(10, 10);
        filter_circle.setFillColor(OBST_COLOR);
        filter_circle.setPosition((filterOutput.x + 1) * gridScale, (gridHeight - filterOutput.y) * gridScale);
        window.draw(filter_circle); 
        
        pather.set_pos(filterOutput);

        pather.add_obstacles(sim.visible_obstacles());
        bool graph_updated = false;
        if (!auton && recompute_timer.elapsed() > RECOMPUTE_COOLDOWN)
        {
            recompute_timer.reset();
            pather.compute_path();
            graph_updated = true;
        }

        if (auton && control.just_updated)
        {
            graph_updated = true;
        }

        if (graph_updated)
        {
            g_rendertexture.clear(sf::Color(0, 0, 0, 0)); // transparent background
            g_rendertexture.create(window.getSize().x, window.getSize().y);
            // use https://www.sfml-dev.org/tutorials/2.5/graphics-draw.php#off-screen-drawing
            const RP::graph &dg = pather.d_graph();
            // draw nodes
            if (showGraph && !dg.nodes.empty())
            {
                std::vector<bool> visited(dg.nodes.size(), false);
                std::vector<bool> visiting(dg.nodes.size(), false);
                std::queue<int> q;
                q.push(0);
                int iters = 0;
                visited[0] = true;
                while (!q.empty())
                {
                    iters++;
                    int ind = q.front();
                    const auto &nd = dg.nodes[ind];
                    q.pop();
                    for (const auto &pair : nd.connection)
                    {
                        const RP::edge &edge = pair.second;
                        if (!visited[edge.child])
                        {
                            if (!visiting[edge.child])
                            {
                                visiting[edge.child] = true;
                                q.push(edge.child);
                            }
                            g_rendertexture.draw(get_vertex_line(
                                nd.coord, dg.nodes[edge.child].coord,
                                GRAPH_EDGE_COLOR, gridScale, gridHeight));
                        }
                    }
                    visited[ind] = true;
                    if (ind != 0)
                        g_rendertexture.draw(getNode(nd, gridScale, gridHeight));
                }
                printf("traversal iters: %d\n", iters);
                g_rendertexture.display();
            }
        }
        const RP::QTreeNode &root = *pather.debug_qtree_root();
        draw_qtree(window, root, gridScale, gridHeight);
        if (lazer)
            grid.drawPath(pather.get_cur_path(), agent);

        window.draw(grid);
        window.draw(agent);
        const sf::Texture &texture = g_rendertexture.getTexture();
        sf::Sprite graph_sprite(texture);
        window.draw(graph_sprite);
        for (auto obst : pather.mem_obstacles())
            window.draw(get_vertex_line(obst.p, obst.q, SEEN_OBST_COLOR, gridScale, gridHeight));
        window.draw(sim);
        // printf("%f, %f\n", next.x, next.y);

        if (frameCount == 12)
        {
            int lastFrame = now;
            now = std::chrono::duration_cast<std::chrono::milliseconds>(std::chrono::system_clock::now().time_since_epoch()).count();

            double avgFrameTime = (now - lastFrame) / 12.0;

            fpsCounter.setString(std::to_string((int)(1000.0 / avgFrameTime)) + " fps");
            frameCount = 0;
        }
        frameCount++;

        if (gibFPS)
            window.draw(fpsCounter);

        sf::Text disp("Fun fact: Harry Potter is called Gary Potter in Russian", font, 16);
        disp.setFillColor(sf::Color(211, 211, 211));
        disp.move(2.f, WINDOW_SCALE * 1526.f);

        if (WINDOW_SCALE == 1.f)
            window.draw(disp);

        window.display();
    }
    // ---------- End of 60 FPS Update Loop ---------- //

    return EXIT_SUCCESS;
}

void draw_qtree(sf::RenderWindow &win, const RP::QTreeNode &node, float scale, float height)
{
    sf::RectangleShape rect;
    rect.setSize(sf::Vector2f((node.max_x - node.min_x) * scale,
                              (node.max_y - node.min_y) * scale));
    rect.setOutlineColor(sf::Color(0, 255, 255));
    rect.setOutlineThickness(1.f);
    rect.setPosition((node.min_x + 1) * scale, (height - node.max_y) * scale);
    sf::Color fillColor =
        node.is_blocked ? sf::Color(0, 255, 255, 64) : sf::Color(0, 0, 0, 0);
    rect.setFillColor(fillColor);
    win.draw(rect);
    if (!node.is_leaf)
    {
        draw_qtree(win, *node.topleft, scale, height);
        draw_qtree(win, *node.topright, scale, height);
        draw_qtree(win, *node.botleft, scale, height);
        draw_qtree(win, *node.botright, scale, height);
    }
}

static inline sf::CircleShape getNode(RP::node nd, float scale, float height)
{
    sf::CircleShape circle(5);
    circle.setOrigin(5, 5);
    circle.setFillColor(GRAPH_NODE_COLOR);
    circle.setPosition((nd.coord.x + 1) * scale, (height - nd.coord.y) * scale);
    return circle;
}
