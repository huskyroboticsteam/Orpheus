using Scarlet.Communications;
using Scarlet.Utilities;
using SharpDX.XInput;
using System;
using System.Net;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

namespace HuskyRobotics.BaseStation.Server {
    /// <summary>
    /// Contains functionality to communicate with the client/rover.
    /// Acts as an abstraction over Scarlet communications API.
    /// </summary>
    public static class BaseServer {

        private static bool shutdown = false;
        private static Controller gamepad;
        private static int LeftThumbDeadzone = 7849;
        private static int RightThumbDeadzone = 8689;
        private static int TriggerThreshold = 30;

        public static void Start()
        {
            Scarlet.Communications.Server.Start(1025, 1026);
            Parse.SetParseHandler(0xC0, gpsHandler);
            Parse.SetParseHandler(0xC1, magnetomerHandler);
            gamepad = GetGamepad();
        }

        private static Controller GetGamepad()
        {
            Controller c = new Controller(UserIndex.One);
            return c;
        }

        public static void EventLoop()
        {
            while (!shutdown) {
                State state = gamepad.GetState();
                byte rightTrigger = state.Gamepad.RightTrigger;
                byte leftTrigger = state.Gamepad.LeftTrigger;
                short leftThumbX = state.Gamepad.LeftThumbX;

                if (rightTrigger < TriggerThreshold) { rightTrigger = 0; }
                if (leftTrigger < TriggerThreshold) { leftTrigger = 0; }
                if (leftThumbX < LeftThumbDeadzone) { leftThumbX = 0; }

                float speed = rightTrigger - leftTrigger;
                float steerPos = leftThumbX;
                speed = (float) UtilMain.LinearMap(speed, -255, 255, -1, 1);
                steerPos = (float)UtilMain.LinearMap(steerPos, -32768, 32767, -1, 1);
                
                //Console.WriteLine("Speed: " + speed);
                //Console.WriteLine("Steer Pos: " + steerPos);

                Packet SteerPack = new Packet(0x94, true);
                SteerPack.AppendData(UtilData.ToBytes(steerPos));
                Scarlet.Communications.Server.Send(SteerPack);

                Packet SpeedPack = new Packet(0x95, true);
                SpeedPack.AppendData(UtilData.ToBytes(speed));
                Scarlet.Communications.Server.Send(SpeedPack);

                Thread.Sleep(10);
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
