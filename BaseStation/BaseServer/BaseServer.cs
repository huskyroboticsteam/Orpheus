using Scarlet.Communications;
using Scarlet.Utilities;
using SharpDX.XInput;
using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using HuskyRobotics.Utilities;

namespace HuskyRobotics.BaseStation.Server
{
    /// <summary>
    /// Contains functionality to communicate with the client/rover.
    /// Acts as an abstraction over Scarlet communications API.
    /// </summary>
    public static class BaseServer
    {
        private static int LeftThumbDeadzone = 7849;
        private static int RightThumbDeadzone = 8689;
        private static int TriggerThreshold = 30;

        private const long CONTROL_SEND_INTERVAL_NANOSECONDS = 100_000_000;

        public static void Setup()
        {
            Scarlet.Communications.Server.Start(1025, 1026);
            Scarlet.Communications.Server.ClientConnectionChange += ClientConnected;
            Parse.SetParseHandler(0xC0, GpsHandler);
            Parse.SetParseHandler(0xC1, MagnetomerHandler);
        }

        public static void Shutdown()
        {
            Scarlet.Communications.Server.Stop();
        }

        private static void ClientConnected(object sender, EventArgs e)
        {
            Console.WriteLine("Clients Changed");
            Console.WriteLine(Scarlet.Communications.Server.GetClients());
        }

        private static long lastControlSend = 0;

        public static void Update(Controller controller)
        {
            if (controller.IsConnected && (TimeNanoseconds() - lastControlSend) > CONTROL_SEND_INTERVAL_NANOSECONDS)
            {
                State state = controller.GetState();
                byte rightTrigger = state.Gamepad.RightTrigger;
                byte leftTrigger = state.Gamepad.LeftTrigger;
                short leftThumbX = Utility.PreventOverflow(state.Gamepad.LeftThumbX);

                Console.WriteLine(leftThumbX);

                if (rightTrigger < TriggerThreshold) { rightTrigger = 0; }
                if (leftTrigger < TriggerThreshold) { leftTrigger = 0; }
                if (Math.Abs(leftThumbX) < LeftThumbDeadzone) { leftThumbX = 0; }

                float speed = (float)UtilMain.LinearMap(rightTrigger - leftTrigger, -255, 255, -1, 1);
                float steerPos = (float)UtilMain.LinearMap(leftThumbX, -32768, 32767, -1, 1);

                Console.WriteLine("Speed: " + speed);
                Console.WriteLine("Steer Pos: " + steerPos);

                bool aPressed = (state.Gamepad.Buttons & GamepadButtonFlags.A) != 0;
                bool bPressed = (state.Gamepad.Buttons & GamepadButtonFlags.B) != 0;

                float steerSpeed = 0.0f;
                if (aPressed)
                    steerSpeed = 1.0f;
                if (bPressed)
                    steerSpeed = -1.0f;

                Console.WriteLine(steerSpeed);

                Packet SteerPack = new Packet(0x8F, true, "MainRover");
                SteerPack.AppendData(UtilData.ToBytes(steerSpeed));
                //SteerPack.AppendData(UtilData.ToBytes(steerPos));
                Scarlet.Communications.Server.Send(SteerPack);

                Packet SpeedPack = new Packet(0x95, true, "MainRover");
                SpeedPack.AppendData(UtilData.ToBytes(speed));
                Scarlet.Communications.Server.Send(SpeedPack);
                lastControlSend = TimeNanoseconds(); //time in nanoseconds
            }
        }

        private static long TimeNanoseconds()
        {
            return DateTime.UtcNow.Ticks * 100;
        }

        private static List<float> ConvertToFloatArray(Packet data)
        {
            List<float> ret = new List<float>();

            byte[][] chunks = data.Data.Payload
                        .Select((s, i) => new { Value = s, Index = i })
                        .GroupBy(x => x.Index / 4)
                        .Select(grp => grp.Select(x => x.Value).ToArray())
                        .ToArray();

            foreach (var chunk in chunks)
            {
                ret.Add(UtilData.ToFloat(chunk));
            }

            return ret;
        }

        private static void GpsHandler(Packet gpsData)
        {
            List<float> vals = ConvertToFloatArray(gpsData);

            float lat = vals[0];
            float lng = vals[1];

            Console.WriteLine(lat + ", " + lng);
        }

        private static void MagnetomerHandler(Packet magData)
        {
            List<float> vals = ConvertToFloatArray(magData);

            float x = vals[0];
            float y = vals[1];
            float z = vals[2];

            Console.WriteLine(x + ", " + y + ", " + z);
        }
    }
}
