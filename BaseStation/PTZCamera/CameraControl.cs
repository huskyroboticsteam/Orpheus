using SharpDX.XInput;
using System;
using System.Net.Http;


namespace HuskyRobotics.BaseStation
{
    /// <summary>
    /// This class is just used to test the camera controls.
    /// </summary>
    public static class CameraControl
    {
        private const int MAX_SPEED = 100;
        private static PTZCamera cam;

        public static void Setup() => cam = new PTZCamera("192.168.0.42", "admin", "1234", new HttpClient());

        public static void Update()
        {
			Controller controller = GamepadFactory.DriveGamepad;
            if(controller.IsConnected)
            {
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
                //Console.WriteLine(verticalSpeed + " " + horizontalSpeed);
                int xSpeed = Convert.ToInt16(horizontalSpeed * MAX_SPEED);
                int ySpeed = Convert.ToInt16(verticalSpeed * MAX_SPEED);
                cam.SetSpeeds(xSpeed, ySpeed);
            }
        }
    }
}
