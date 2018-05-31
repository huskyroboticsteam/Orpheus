using Scarlet.Communications;
using Scarlet.Utilities;
using SharpDX.XInput;
using System;
using System.Net;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

namespace HuskyRobotics.BaseStation.Server
{
    /// <summary>
    /// Contains functionality to communicate with the client/rover.
    /// Acts as an abstraction over Scarlet communications API.
    /// </summary>
    public static class BaseServer
    {

        private static bool shutdown = false;
        private static Controller gamepad;
        private static int LeftThumbDeadzone = 7849;
        private static int RightThumbDeadzone = 8689;
        private static int TriggerThreshold = 30;

        public static void Start()
        {
            Scarlet.Communications.Server.Start(1025, 1026);
            Scarlet.Communications.Server.ClientConnectionChange += memes;
            Parse.SetParseHandler(0xC0, gpsHandler);
            Parse.SetParseHandler(0xC1, magnetomerHandler);
            gamepad = GetGamepad();
        }

        private static void memes(object sender, EventArgs e)
        {
            Console.WriteLine("Clients Changed");
            Console.WriteLine(Scarlet.Communications.Server.GetClients());
        }

        private static Controller GetGamepad()
        {
            Controller c = new Controller(UserIndex.One);
            return c;
        }

        public static void EventLoop()
        {
            while (!shutdown)
            {
                if (!gamepad.IsConnected)
                {
                    Console.WriteLine("Gamepad not connected");
                    Thread.Sleep(100);
                    continue;
                }
                State state = gamepad.GetState();
                byte rightTrigger = state.Gamepad.RightTrigger;
                byte leftTrigger = state.Gamepad.LeftTrigger;
                short leftThumbX = state.Gamepad.LeftThumbX;

                Console.WriteLine(leftThumbX);

                if (leftThumbX == -32768)
                {
                    leftThumbX++;
                }

                if (rightTrigger < TriggerThreshold) { rightTrigger = 0; }
                if (leftTrigger < TriggerThreshold) { leftTrigger = 0; }
                if (Math.Abs(leftThumbX) < LeftThumbDeadzone) { leftThumbX = 0; }

                float speed = rightTrigger - leftTrigger;
                float steerPos = leftThumbX;

                speed = (float)UtilMain.LinearMap(speed, -255, 255, -1, 1);
                steerPos = (float)UtilMain.LinearMap(steerPos, -32768, 32767, -1, 1);

                Console.WriteLine("Speed: " + speed);
                Console.WriteLine("Steer Pos: " + steerPos);

                bool a = (state.Gamepad.Buttons & GamepadButtonFlags.A) != 0;
                bool b = (state.Gamepad.Buttons & GamepadButtonFlags.B) != 0;
                float speed1 = 0.0f;
                if (a)
                    speed1 = 1.0f;
                if (b)
                    speed1 = -1.0f;
                Console.WriteLine(speed1);
                Packet SteerPack = new Packet(0x8F, true, "MainRover");
                SteerPack.AppendData(UtilData.ToBytes(speed1));
                //SteerPack.AppendData(UtilData.ToBytes(steerPos));
                Scarlet.Communications.Server.Send(SteerPack);

                Packet SpeedPack = new Packet(0x95, true, "MainRover");
                SpeedPack.AppendData(UtilData.ToBytes(speed));
                Scarlet.Communications.Server.Send(SpeedPack);

                Thread.Sleep(100);
            }
        }

        private static List<float> convertToFloatArray(Packet data)
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

        private static void gpsHandler(Packet gpsData)
        {
            List<float> vals = convertToFloatArray(gpsData);

            float lat = vals[0];
            float lng = vals[1];

            Console.WriteLine(lat + ", " + lng);
        }

        private static void magnetomerHandler(Packet magData)
        {
            List<float> vals = convertToFloatArray(magData);

            float x = vals[0];
            float y = vals[1];
            float z = vals[2];

            Console.WriteLine(x + ", " + y + ", " + z);
        }

        public static void Shutdown()
        {
            shutdown = true;
            Scarlet.Communications.Server.Stop();
        }
    }
}
