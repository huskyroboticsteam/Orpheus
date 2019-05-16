""" modified from code in wikipedia article https://en.wikipedia.org/wiki/Maze_generation_algorithm#Python_code_example """
import numpy
from numpy.random import randint as rand
import matplotlib.pyplot as pyplot

def maze(width=81, height=51, complexity=.75, density=.75):
    # Only odd shapes
    shape = ((height // 2) * 2 + 1, (width // 2) * 2 + 1)
    # Adjust complexity and density relative to maze size
    complexity = int(complexity * (5 * (shape[0] + shape[1]))) # number of components
    density    = int(density * ((shape[0] // 2) * (shape[1] // 2))) # size of components
    # Build actual maze
    Z = numpy.zeros(shape, dtype=bool)
    # Fill borders
    Z[0, :] = Z[-1, :] = 1
    Z[:, 0] = Z[:, -1] = 1
    # Make aisles
    for i in range(density):
        x, y = rand(0, shape[1] // 2) * 2, rand(0, shape[0] // 2) * 2 # pick a random position
        Z[y, x] = 1
        for j in range(complexity):
            neighbours = []
            if x > 1:             neighbours.append((y, x - 2))
            if x < shape[1] - 2:  neighbours.append((y, x + 2))
            if y > 1:             neighbours.append((y - 2, x))
            if y < shape[0] - 2:  neighbours.append((y + 2, x))
            if len(neighbours):
                y_,x_ = neighbours[rand(0, len(neighbours) - 1)]
                if Z[y_, x_] == 0:
                    Z[y_, x_] = 1
                    Z[y_ + (y - y_) // 2, x_ + (x - x_) // 2] = 1
                    x, y = x_, y_
    return Z

mzrows, mzcols = mazesz = (20, 20)
mzrows += 1
mzcols += 1
gheight, gwidth = gridsz = (40, 36)
cheight = gheight / (mzrows-1)
cwidth = gwidth / (mzcols-1)

obstacles = list() # 2-tuples of 2-tuples ((row, col), (row, col))

mz = maze(*mazesz)
for r in range(0, mzrows):
    for c in range(0, mzcols):
        if not mz[r][c]:
            continue
        
        topleft = (cheight * r, cwidth * c)
        center = (topleft[0] + cheight/2, topleft[1] + cwidth/2)
        botright = (topleft[0] + cheight, topleft[1] + cwidth)
        if r > 0 and mz[r-1][c]:
            obstacles.append((center, (topleft[0], center[1])))
        if c > 0 and mz[r][c-1]:
            obstacles.append((center, (center[0], topleft[1])))
        if r < mzrows - 1 and mz[r+1][c]:
            obstacles.append((center, (botright[0], center[1])))
        if c < mzcols - 1 and mz[r][c+1]:
            obstacles.append((center, (center[0], botright[1])))

for ob in obstacles:
    for pt in ob:
        print(round(pt[1], 2), round(pt[0], 2), end='\t')
    print()

# pyplot.figure(figsize=(10, 5))
# pyplot.imshow(maze(80, 40), cmap=pyplot.cm.binary, interpolation='nearest')
# pyplot.xticks([]), pyplot.yticks([])
# pyplot.show()
