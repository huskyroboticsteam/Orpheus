using Scarlet.Utilities;
using SlimDX.DirectInput;
using SlimDX.XInput;
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
            Joystick joystick = GetJoystick();

            if (joystick != null)
            {
                // Acquire the joystick
                joystick.Acquire();

                while (true)
                {
                    joystick.Poll();
                    var state = joystick.GetCurrentState();

                    xSpeed = Convert.ToInt16(UtilMain.LinearMap(state.X, 0, 65535, -1, 1) * maxSpeed);
                    ySpeed = Convert.ToInt16(UtilMain.LinearMap(state.Y, 0, 65535, -1, 1) * maxSpeed);
                    Console.WriteLine(xSpeed + ", " + ySpeed);

                    cam.SetSpeeds(xSpeed, ySpeed);
                    Thread.Sleep(10);

                }
            }
        }

        private static Joystick GetJoystick() {
            // Initialize DirectInput
            var directInput = new DirectInput();

            // Find a Joystick Guid
            var joystickGuid = Guid.Empty;

            // Look for Gamepad
            foreach (var deviceInstance in directInput.GetDevices(SlimDX.DirectInput.DeviceType.Gamepad,
                        DeviceEnumerationFlags.AllDevices))
                joystickGuid = deviceInstance.InstanceGuid;

            // If Gamepad not found, look for a Joystick
            if (joystickGuid == Guid.Empty)
                foreach (var deviceInstance in directInput.GetDevices(SlimDX.DirectInput.DeviceType.Joystick,
                        DeviceEnumerationFlags.AllDevices))
                    joystickGuid = deviceInstance.InstanceGuid;

            // If Joystick not found, throws an error
            if (joystickGuid == Guid.Empty)
            {
                Console.WriteLine("No Joystick Found");
                return null;
            }

            Console.WriteLine("Found Joystick/Gamepad with GUID: {0}", joystickGuid);
            // Instantiate the joystick
            return new Joystick(directInput, joystickGuid);
        }
    }
}
