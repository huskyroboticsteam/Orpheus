using Scarlet.Utilities;
using SharpDX.XInput;
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
        public static void Start(Controller controller)
        {
            int maxSpeed = 100;
            int xSpeed = 0;
            int ySpeed = 0;

            HttpClient client = new HttpClient();
            PTZCamera cam = new PTZCamera("192.168.0.42", "admin", "1234", client);
            //Joystick joystick = GetJoystick();

            if (controller != null)
            {

                while (true)
                {
                    if (!controller.IsConnected)
                    {
                        Thread.Sleep(100);
                        continue;
                    }

                    State state = controller.GetState();

                    bool leftPressed = (state.Gamepad.Buttons & GamepadButtonFlags.DPadLeft) != 0;
                    bool rightPressed = (state.Gamepad.Buttons & GamepadButtonFlags.DPadRight) != 0;
                    bool upPressed = (state.Gamepad.Buttons & GamepadButtonFlags.DPadUp) != 0;
                    bool downPressed = (state.Gamepad.Buttons & GamepadButtonFlags.DPadDown) != 0;
                    int horizontalSpeed = 0;
                    int verticalSpeed = 0;

                    if (leftPressed) { horizontalSpeed = -1; }
                    else if (rightPressed) { horizontalSpeed = 1; }
                    else if (upPressed) { verticalSpeed = 1; }
                    else if (downPressed) { verticalSpeed = -1; }

                    Console.WriteLine(verticalSpeed + " " + horizontalSpeed);

                    xSpeed = Convert.ToInt16(horizontalSpeed * maxSpeed);
                    ySpeed = Convert.ToInt16(verticalSpeed * maxSpeed);

                    cam.SetSpeeds(xSpeed, ySpeed);
                    Thread.Sleep(10);

                }
            }
        }

        private static short PreventOverflow(short shortVal)
        {
            if (shortVal == -32768)
            {
                shortVal++;
            }
            return shortVal;
        }
    }
}
