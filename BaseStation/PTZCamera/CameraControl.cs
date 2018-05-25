using OpenTK;
using OpenTK.Input;
using Scarlet.Utilities;
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
            int maxSpeed = 100;
            int xSpeed=0;
            int ySpeed=0;

            HttpClient client = new HttpClient();
            PTZCamera cam = new PTZCamera("192.168.0.42", "admin","1234",client);
            
            while (true)
            {
                JoystickState jsState = Joystick.GetState(0);

                if (jsState.IsConnected == true)
                {
                    xSpeed = Convert.ToInt16(jsState.GetAxis(0) * maxSpeed);
                    ySpeed = Convert.ToInt16(jsState.GetAxis(1) * maxSpeed);

                    cam.SetSpeeds(xSpeed, ySpeed);
                }
                Thread.Sleep(10);

            }
        }
    }
}
