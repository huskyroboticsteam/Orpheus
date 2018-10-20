using Scarlet.Communications;
using Scarlet.Utilities;
using SharpDX.XInput;
using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

namespace HuskyRobotics.BaseStation.Server
{
    /// <summary>
    /// Contains functionality to communicate with the client/rover.
    /// Acts as an abstraction over Scarlet communications API.
    /// </summary>
    public static class PacketSender
    {
		private static readonly int LeftThumbDeadzone = 7849;
        //private static readonly int RightThumbDeadzone = 8689;
        private static readonly int TriggerThreshold = 30;
        private const long CONTROL_SEND_INTERVAL_NANOSECONDS = 100_000_000; //100,000,000 ns == 100 ms
        private static long lastControlSend = 0;

        public static event EventHandler<(float, float)> GPSUpdate;
        public static event EventHandler<(double, double)> RFUpdate;
        public static event EventHandler<(float, float, float)> MagnetometerUpdate;

        public static void Setup()
        {
            Scarlet.Communications.Server.Start(1025, 1026, OperationPeriod:1);
            Scarlet.Communications.Server.ClientConnectionChange += ClientConnected;
            Parse.SetParseHandler(0xC0, GpsHandler);
            Parse.SetParseHandler(0xC1, MagnetomerHandler);
            Parse.SetParseHandler(0xD4, RFSignalHandler);
        }

        public static void Shutdown()
        {
            HaltRoverMotion();
            Scarlet.Communications.Server.Stop();
        }

        private static void ClientConnected(object sender, EventArgs e)
        {
            Console.WriteLine("Clients Changed");
            Console.WriteLine(Scarlet.Communications.Server.GetClients());
        }

		/// <summary>
		/// Send rover movement control packets.
		/// </summary>
		/// <param name="driveController"></param>
        public static void Update()
        {
			Controller driveController = GamepadFactory.DriveGamepad;
			Controller armController = GamepadFactory.ArmGamepad;

			if (SendIntervalElapsed()) {
                if(driveController.IsConnected && armController.IsConnected) {
                    State driveState = driveController.GetState();
                    State armState = armController.GetState();
                    byte rightTrigger = driveState.Gamepad.RightTrigger;
                    byte leftTrigger = driveState.Gamepad.LeftTrigger;
                    short leftThumbX = PreventOverflow(driveState.Gamepad.LeftThumbX);

                    if (rightTrigger < TriggerThreshold) { rightTrigger = 0; }
                    if (leftTrigger < TriggerThreshold) { leftTrigger = 0; }
                    if (Math.Abs(leftThumbX) < LeftThumbDeadzone) { leftThumbX = 0; }

                    float speed = (float)UtilMain.LinearMap(rightTrigger - leftTrigger, -255, 255, -1, 1);
                    float steerPos = (float)UtilMain.LinearMap(leftThumbX, -32768, 32767, -1, 1);

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
                    if (bPressedDrive)
                        steerSpeed = -0.3f;
                    else if (aPressedDrive)
                        steerSpeed = 0.3f;

                    float wristArmSpeed = 0.0f;
                    if (yPressedArm)
                        wristArmSpeed = -0.5f;
                    else if (xPressedArm)
                        wristArmSpeed = 0.5f;

                    float elbowArmSpeed = 0.0f;
                    if (bPressedArm)
                        elbowArmSpeed = -0.5f;
                    else if (aPressedArm)
                        elbowArmSpeed = 0.5f;

                    float shoulderArmSpeed = 0.0f;
                    if (downPressedArm)
                        shoulderArmSpeed = -1.0f;
                    else if (upPressedArm)
                        shoulderArmSpeed = 1.0f;

                    float baseArmSpeed = 0.0f;
                    if (rightPressedArm)
                        baseArmSpeed = -0.5f;
                    else if (leftPressedArm)
                        baseArmSpeed = 0.5f;

                    Packet SteerPack = new Packet(0x8F, true, "MainRover");
                    SteerPack.AppendData(UtilData.ToBytes(steerSpeed));
                    Scarlet.Communications.Server.Send(SteerPack);

                    Packet SpeedPack = new Packet(0x95, true, "MainRover");
                    SpeedPack.AppendData(UtilData.ToBytes(speed));
                    Scarlet.Communications.Server.Send(SpeedPack);

                    Packet WristPack = new Packet(0x9D, true, "ArmMaster");
                    WristPack.AppendData(UtilData.ToBytes(wristArmSpeed));
                    Scarlet.Communications.Server.Send(WristPack);

                    Packet ElbowPack = new Packet(0x9C, true, "ArmMaster");
                    ElbowPack.AppendData(UtilData.ToBytes(elbowArmSpeed));
                    Scarlet.Communications.Server.Send(ElbowPack);

                    Packet ShoulderPack = new Packet(0x9B, true, "ArmMaster");
                    ShoulderPack.AppendData(UtilData.ToBytes(shoulderArmSpeed));
                    Scarlet.Communications.Server.Send(ShoulderPack);

                    Packet BasePack = new Packet(0x9A, true, "ArmMaster");
                    BasePack.AppendData(UtilData.ToBytes(baseArmSpeed));
                    Scarlet.Communications.Server.Send(BasePack);
                } else {
                    Console.WriteLine("Gamepad not connected!");
                    HaltRoverMotion();
                }

                lastControlSend = TimeNanoseconds();
            }
        }

        private static void HaltRoverMotion()
        {
            Packet SteerPack = new Packet(0x8F, true, "MainRover");
            SteerPack.AppendData(UtilData.ToBytes(0));
            Scarlet.Communications.Server.Send(SteerPack);

            Packet SpeedPack = new Packet(0x95, true, "MainRover");
            SpeedPack.AppendData(UtilData.ToBytes(0));
            Scarlet.Communications.Server.Send(SpeedPack);

            Packet ArmEmergencyStop = new Packet(0x80, true, "ArmMaster");
            Scarlet.Communications.Server.Send(ArmEmergencyStop);
        }

        private static short PreventOverflow(short shortVal) => shortVal == -32768 ? (short)(shortVal + 1) : shortVal;

        private static bool SendIntervalElapsed() => (TimeNanoseconds() - lastControlSend) > CONTROL_SEND_INTERVAL_NANOSECONDS;
        private static long TimeNanoseconds() => DateTime.UtcNow.Ticks * 100;

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

        private static List<double> ConvertToDoubleArray(Packet data)
        {
            List<double> ret = new List<double>();

            byte[][] chunks = data.Data.Payload
                        .Select((s, i) => new { Value = s, Index = i })
                        .GroupBy(x => x.Index / 8)
                        .Select(grp => grp.Select(x => x.Value).ToArray())
                        .ToArray();

            foreach (var chunk in chunks)
            {
                ret.Add(UtilData.ToDouble(chunk));
            }

            return ret;
        }

        private static void GpsHandler(Packet gpsData)
        {
            List<float> vals = ConvertToFloatArray(gpsData);

            float lat = vals[0];
            float lng = vals[1];

            GPSUpdate(null, (lat, lng));

            Console.WriteLine(lat + ", " + lng);
        }

        private static void MagnetomerHandler(Packet magData)
        {
            List<float> vals = ConvertToFloatArray(magData);

            float x = vals[0];
            float y = vals[1];
            float z = vals[2];

            Console.WriteLine(x + ", " + y + ", " + z);
            MagnetometerUpdate(null, (x, y, z));
        }

        private static void RFSignalHandler(Packet data)
        {
            List<double> vals = ConvertToDoubleArray(data);

            double angle = vals[0];
            double strength = vals[1];

            RFUpdate(null, (angle, strength));
            try
            {
                Packet packet = new Packet(data.Data, true, "RDFGraph");
                Scarlet.Communications.Server.Send(packet);
            }
            catch { /* Do nothing */ }
        }
    }
}
