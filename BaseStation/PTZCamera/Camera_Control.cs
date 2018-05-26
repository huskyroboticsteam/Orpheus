using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HuskyRobotics
{
    /// <summary>
    /// This class is just used to test the camera controls.
    /// </summary>
    class Camera_Control
    {
        static void Main(string[] args)
        {
            int max_speed = 100;
            int x_speed=0;
            int y_speed=0;
            bool speed_Changed;
            HttpClient client = new HttpClient();
            PTZCamera cam = new PTZCamera("192.168.0.42", "admin","1234",client);
            while (true)
            {
                speed_Changed = false;
                if (Keyboard.GetState().IsKeyDown(Key.Up) )
                {
                    if(y_speed != max_speed){ 
                        y_speed = max_speed;
                        speed_Changed = true;
                    }
                }
                else if (Keyboard.GetState().IsKeyDown(Key.Down))
                {
                    if (y_speed != -max_speed)
                    {
                        y_speed = -max_speed;
                        speed_Changed = true;
                    }
                }
                else if (y_speed != 0)
                {
                    y_speed = 0;
                    speed_Changed = true;
                }

                if (Keyboard.GetState().IsKeyDown(Key.Left))
                {
                    if (x_speed != -max_speed)
                    {
                        x_speed = -max_speed;
                        speed_Changed = true;
                    }
                }
                else if (Keyboard.GetState().IsKeyDown(Key.Right))
                {
                    if (x_speed != max_speed)
                    {
                        x_speed = max_speed;
                        speed_Changed = true;
                    }
                }
                else if (x_speed != 0)
                {
                    x_speed = 0;
                    speed_Changed = true;
                }

                if (speed_Changed)
                {
                    Console.WriteLine("x,y: " + x_speed + ", " + y_speed);
                    cam.set_speeds(x_speed, y_speed);
                }

            }
        }
    }
}
