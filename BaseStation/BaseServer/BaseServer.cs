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
        private static Controller DriveGamepad;
        private static Controller ArmGamepad;
        private static int LeftThumbDeadzone = 7849;
        private static int RightThumbDeadzone = 8689;
        private static int TriggerThreshold = 30;

        public static void Start(Controller dGamepad, Controller aGamepad)
        {
            Scarlet.Communications.Server.Start(1025, 1026);
            Scarlet.Communications.Server.ClientConnectionChange += ClientConnected;
            Parse.SetParseHandler(0xC0, gpsHandler);
            Parse.SetParseHandler(0xC1, magnetomerHandler);
            DriveGamepad = dGamepad;
            ArmGamepad = aGamepad;
        }

        private static void ClientConnected(object sender, EventArgs e)
        {
            Console.WriteLine("Clients Changed");
            Console.WriteLine(Scarlet.Communications.Server.GetClients());
        }

        private static short PreventOverflow(short shortVal)
        {
            if (shortVal == -32768)
            {
                shortVal++;
            }
            return shortVal;
        }

        public static void EventLoop()
        {
            Packet SteerPack;
            Packet SpeedPack;
            Packet WristPack;
            Packet ElbowPack;
            Packet ShoulderPack;
            Packet BasePack;

            while (!shutdown)
            {
                if (!DriveGamepad.IsConnected)
                {
                    Console.WriteLine("Gamepad not connected");
                    SteerPack = new Packet(0x8F, true, "MainRover");
                    SteerPack.AppendData(UtilData.ToBytes(0));
                    Scarlet.Communications.Server.Send(SteerPack);

                    SpeedPack = new Packet(0x95, true, "MainRover");
                    SpeedPack.AppendData(UtilData.ToBytes(0));
                    Scarlet.Communications.Server.Send(SpeedPack);

                    Packet ArmEmergencyStop = new Packet(0x80, true, "ArmMaster");
                    Scarlet.Communications.Server.Send(ArmEmergencyStop);
                    Thread.Sleep(100);
                    continue;
                }
                State driveState = DriveGamepad.GetState();
                State armState = ArmGamepad.GetState();
                byte rightTrigger = driveState.Gamepad.RightTrigger;
                byte leftTrigger = driveState.Gamepad.LeftTrigger;
                short leftThumbX = PreventOverflow(driveState.Gamepad.LeftThumbX);

                if (rightTrigger < TriggerThreshold) { rightTrigger = 0; }
                if (leftTrigger < TriggerThreshold) { leftTrigger = 0; }
                if (Math.Abs(leftThumbX) < LeftThumbDeadzone) { leftThumbX = 0; }

                float speed = (float)UtilMain.LinearMap(rightTrigger - leftTrigger, -255, 255, -1, 1);
                float steerPos = (float)UtilMain.LinearMap(leftThumbX, -32768, 32767, -1, 1);

                Console.WriteLine("Speed: " + speed);
                Console.WriteLine("Steer Pos: " + steerPos);

                bool aPressedDrive = (driveState.Gamepad.Buttons & GamepadButtonFlags.A) != 0;
                bool bPressedDrive = (driveState.Gamepad.Buttons & GamepadButtonFlags.B) != 0;

                bool aPressedArm = (armState.Gamepad.Buttons & GamepadButtonFlags.A) != 0;
                bool bPressedArm = (armState.Gamepad.Buttons & GamepadButtonFlags.B) != 0;
                bool xPressedArm = (armState.Gamepad.Buttons & GamepadButtonFlags.X) != 0;
                bool yPressedArm = (armState.Gamepad.Buttons & GamepadButtonFlags.Y) != 0;

                bool upPressedArm = (armState.Gamepad.Buttons & GamepadButtonFlags.DPadUp) != 0;
                bool downPressedArm = (armState.Gamepad.Buttons & GamepadButtonFlags.DPadDown) != 0;
                bool leftPressedArm = (armState.Gamepad.Buttons & GamepadButtonFlags.DPadLeft) != 0;
                bool rightPressedArm = (armState.Gamepad.Buttons & GamepadButtonFlags.DPadRight) != 0;


                float steerSpeed = 0.0f;
                if (aPressedDrive)
                    steerSpeed = 0.3f;
                if (bPressedDrive)
                    steerSpeed = -0.3f;

                float wristArmSpeed = 0.0f;
                if (xPressedArm)
                    wristArmSpeed = 0.5f;
                if (yPressedArm)
                    wristArmSpeed = -0.5f;

                float elbowArmSpeed = 0.0f;
                if (aPressedArm)
                    elbowArmSpeed = 0.5f;
                if (bPressedArm)
                    elbowArmSpeed = -0.5f;

                float shoulderArmSpeed = 0.0f;
                if (upPressedArm)
                    shoulderArmSpeed = 1.0f;
                if (downPressedArm)
                    shoulderArmSpeed = -1.0f;

                float baseArmSpeed = 0.0f;
                if (leftPressedArm)
                    baseArmSpeed = 0.5f;
                if (rightPressedArm)
                    baseArmSpeed = -0.5f;

                SteerPack = new Packet(0x8F, true, "MainRover");
                SteerPack.AppendData(UtilData.ToBytes(steerSpeed));
                Scarlet.Communications.Server.Send(SteerPack);

                SpeedPack = new Packet(0x95, true, "MainRover");
                SpeedPack.AppendData(UtilData.ToBytes(speed));
                Scarlet.Communications.Server.Send(SpeedPack);

                WristPack = new Packet(0x9D, true, "ArmMaster");
                WristPack.AppendData(UtilData.ToBytes(wristArmSpeed));
                Scarlet.Communications.Server.Send(WristPack);

                ElbowPack = new Packet(0x9C, true, "ArmMaster");
                ElbowPack.AppendData(UtilData.ToBytes(elbowArmSpeed));
                Scarlet.Communications.Server.Send(ElbowPack);

                ShoulderPack = new Packet(0x9B, true, "ArmMaster");
                ShoulderPack.AppendData(UtilData.ToBytes(shoulderArmSpeed));
                Scarlet.Communications.Server.Send(ShoulderPack);

                BasePack = new Packet(0x9A, true, "ArmMaster");
                BasePack.AppendData(UtilData.ToBytes(baseArmSpeed));
                Scarlet.Communications.Server.Send(BasePack);

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

            Packet SteerPack = new Packet(0x8F, true, "MainRover");
            SteerPack.AppendData(UtilData.ToBytes(0));
            Scarlet.Communications.Server.Send(SteerPack);

            Packet SpeedPack = new Packet(0x95, true, "MainRover");
            SpeedPack.AppendData(UtilData.ToBytes(0));
            Scarlet.Communications.Server.Send(SpeedPack);

            Packet ArmEmergencyStop = new Packet(0x80, true, "ArmMaster");
            Scarlet.Communications.Server.Send(ArmEmergencyStop);

            Scarlet.Communications.Server.Stop();
        }
    }
}
