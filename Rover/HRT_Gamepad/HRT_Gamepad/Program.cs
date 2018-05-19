using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.DirectInput;

namespace HRT_Gamepad
{
    class Program
    {
        static void Main()
        {
            // Initialize DirectInput
            var directInput = new DirectInput();

            // Find a Joystick Guid
            var joystickGuid = Guid.Empty;

            foreach (var deviceInstance in directInput.GetDevices(DeviceType.Gamepad,
                        DeviceEnumerationFlags.AllDevices))
                joystickGuid = deviceInstance.InstanceGuid;

            /*
            // If Gamepad not found, look for a Joystick
            if (joystickGuid == Guid.Empty)
                foreach (var deviceInstance in directInput.GetDevices(DeviceType.Joystick,
                        DeviceEnumerationFlags.AllDevices))
                    joystickGuid = deviceInstance.InstanceGuid;
*/
            // If Joystick not found, throws an error
            if (joystickGuid == Guid.Empty)
            {
                Console.WriteLine("No joystick/Gamepad found.");
                Console.ReadKey();
                Environment.Exit(1);
            }

            // Instantiate the joystick
            var joystick = new Joystick(directInput, joystickGuid);

            Console.WriteLine("Found Joystick/Gamepad with GUID: {0}", joystickGuid);

            // Query all suported ForceFeedback effects
            var allEffects = joystick.GetEffects();
            foreach (var effectInfo in allEffects)
                Console.WriteLine("Effect available {0}", effectInfo.Name);

            // Set BufferSize in order to use buffered data.
            joystick.Properties.BufferSize = 128;

            // Acquire the joystick
            joystick.Acquire();

            // Poll events from joystick
            while (true)
            {
                var joystickState = new JoystickState();
                joystick.GetCurrentState(ref joystickState);
                var datas = joystick.GetBufferedData();
                foreach (var state in datas)
                {
                    string temp = state.ToString();
                    if (temp.Contains("Offset: Z, Value:"))
                    {
                        int endOfNum = 5;
                        for (int i = 0; i < 5; i++)
                        {
                            if (temp[21 + i] == ' ')
                            {
                                endOfNum = i;
                                break;
                            }
                        }
                            //22
                        Console.Write("Z :");
                        int value = int.Parse(temp.Substring(19, endOfNum + 2));
                        //value -= 2767;
                        Console.WriteLine(value);
                    }
                    else if (temp.Contains("Offset: RotationX, Value:"))
                    {
                        int endOfNum = 5;
                        for (int i = 0; i < 5; i++)
                        {
                            if (temp[27 + i] == ' ')
                            {
                                endOfNum = i;
                                break;
                            }
                        }
                        //22
                        Console.Write("X :");
                        int center = 32895;
                        int value = int.Parse(temp.Substring(25, endOfNum + 2));
                        double printt = 0;
                        if (value < center)
                        {
                            printt = center - value;
                            printt = -printt / (double)center;

                        }
                        else
                        {
                            printt = value - center;
                            printt = printt / (double)center;
                        }
                        Console.WriteLine(printt);
                    }
                    else if (temp.Contains("Offset: RotationY, Value:"))
                    {
                        int endOfNum = 5;
                        for (int i = 0; i < 5; i++)
                        {
                            if (temp[27 + i] == ' ')
                            {
                                endOfNum = i;
                                break;
                            }
                        }
                        //22
                        Console.Write("              Y :");
                        int center = 32638;
                        int value = int.Parse(temp.Substring(25, endOfNum + 2));
                        double printt = 0;
                        if (value < center)
                        {
                            printt = center - value;
                            printt = printt / (double)center;

                        } else
                        {
                            printt = value - center;
                            printt = -printt / (double)center;
                        }
                        Console.WriteLine(printt);
                    }
                    else
                    {
                        Console.WriteLine(state);
                    }
                }
            }
        }
    }
}
