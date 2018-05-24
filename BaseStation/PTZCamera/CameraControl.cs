using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HuskyRobotics.BaseStation
{
    /// <summary>
    /// This class is just used to test the camera controls.
    /// </summary>
    public static class CameraControl
    {
        public static void Start()
        {
            int max_speed = 100;
            int x_speed=0;
            int y_speed=0;

            HttpClient client = new HttpClient();
            PTZCamera cam = new PTZCamera("192.168.0.42", "admin","1234",client);

            while (true)
            {
                if (Keyboard.GetState().IsKeyDown(Key.Up) )
                {
                        y_speed = max_speed;
                }
                else if (Keyboard.GetState().IsKeyDown(Key.Down))
                {
                        y_speed = -max_speed;
                }
                else if (y_speed != 0)
                {
                    y_speed = 0;
                }

                if (Keyboard.GetState().IsKeyDown(Key.Left))
                {
                        x_speed = -max_speed;
                }
                else if (Keyboard.GetState().IsKeyDown(Key.Right))
                {
                        x_speed = max_speed;
                }
                else if (x_speed != 0)
                {
                    x_speed = 0;
                }

                cam.SetSpeeds(x_speed, y_speed);
                Thread.Sleep(10);
            }
        }
    }
}
