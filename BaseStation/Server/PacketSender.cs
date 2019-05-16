using Scarlet.Communications;
using Scarlet.Utilities;
using SharpDX.XInput;
using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
        private static readonly int JoystickTreshold = 8000;
        private static readonly int TriggerThreshold = 30;
        private const long CONTROL_SEND_INTERVAL_NANOSECONDS = 200_000_000; //100,000,000 ns == 100 ms
        private static long lastControlSend = 0;
        private static bool ManualMode = true;
        private static bool SendModeChange = false;
        private static double scaler = 1.0;

        public static event EventHandler<(float, float)> GPSUpdate;
        public static event EventHandler<(double, double)> RFUpdate;
        public static event EventHandler<(float, float, float)> MagnetometerUpdate;
        public static List<Tuple<double, double>> coords;
        public static Tuple<double, double> target;
        public static double direction;

        public static void Setup()
        {
            coords = new List<Tuple<double, double>>();
            target = Tuple.Create(0.0, 0.0);
            Log.SetGlobalOutputLevel(Log.Severity.DEBUG);
            Scarlet.Communications.Server.Start(1025, 1026, OperationPeriod: 1);
            Scarlet.Communications.Server.ClientConnectionChange += ClientConnected;
            Parse.SetParseHandler(0xC0, GpsHandler);
            Parse.SetParseHandler(0xC1, MagnetomerHandler);
            Parse.SetParseHandler(0xD4, RFSignalHandler);
            direction = 0;
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

        public static void SwitchScaler(double num)
        {
            scaler = num;
        }

        public static void SwitchMode(bool manual)
        {
            ManualMode = manual;
            SendModeChange = true;
        }

        /// <summary>
        /// Send rover movement control packets.
        /// </summary>
        /// <param name="driveController"></param>
        public static void Update()
        {
            Controller driveController = GamepadFactory.DriveGamepad;
            Controller armController = GamepadFactory.ArmGamepad;
            if (SendIntervalElapsed())
            {
                if (driveController.IsConnected && armController.IsConnected)
                {
                    State driveState = driveController.GetState();
                    State armState = armController.GetState();
                    byte rightTrigger = driveState.Gamepad.RightTrigger;
                    byte leftTrigger = driveState.Gamepad.LeftTrigger;

                    short leftThumbX = PreventOverflow(driveState.Gamepad.LeftThumbX);

                    if (rightTrigger < TriggerThreshold) { rightTrigger = 0; }
                    if (leftTrigger < TriggerThreshold) { leftTrigger = 0; }
                    if (Math.Abs(leftThumbX) < LeftThumbDeadzone) { leftThumbX = 0; }

                    float speed = (float)UtilMain.LinearMap(rightTrigger - leftTrigger, -255, 255, -0.5, 0.5);
                    float steerPos = (float)UtilMain.LinearMap(leftThumbX, -32768, 32767, -0.5, 0.5);
                    if (Math.Abs(speed) < 0.0001f) { speed = 0; }
                    if (Math.Abs(steerPos) < 0.0001f) { steerPos = 0; }


                    //testing values to send for autonomous, remove later
                    bool aPressedAuto = (driveState.Gamepad.Buttons & GamepadButtonFlags.A) != 0;
                    bool bPressedAuto = (driveState.Gamepad.Buttons & GamepadButtonFlags.B) != 0;
                    //-----------------------------------------------------
                    bool startPressedArm = (armState.Gamepad.Buttons & GamepadButtonFlags.Start) != 0;
                    bool aPressedArm = (armState.Gamepad.Buttons & GamepadButtonFlags.A) != 0;
                    bool bPressedArm = (armState.Gamepad.Buttons & GamepadButtonFlags.B) != 0;
                    bool xPressedArm = (armState.Gamepad.Buttons & GamepadButtonFlags.X) != 0;
                    bool yPressedArm = (armState.Gamepad.Buttons & GamepadButtonFlags.Y) != 0;

                    bool upPressedArm = (armState.Gamepad.Buttons & GamepadButtonFlags.DPadUp) != 0;
                    bool downPressedArm = (armState.Gamepad.Buttons & GamepadButtonFlags.DPadDown) != 0;
                    bool leftPressedArm = (armState.Gamepad.Buttons & GamepadButtonFlags.DPadLeft) != 0;
                    bool rightPressedArm = (armState.Gamepad.Buttons & GamepadButtonFlags.DPadRight) != 0;

                    //------------------------------------------------------------------------------------------===
                    // Rover skid steering turn (uses x axis on right joystick)
                    short diffHorz = armState.Gamepad.RightThumbX;
                    if (diffHorz > -JoystickTreshold && diffHorz < JoystickTreshold) { diffHorz = 0; }
                    short diffHorzShort = (short)UtilMain.LinearMap(diffHorz, -32768, 32767, -128, 128);
                    //if (Math.Abs(diffHorzShort) < 1) { diffHorzShort = 0; }

                    // Rover skid steering speed (uses y axis on right joystick)
                    short diffVert = armState.Gamepad.RightThumbY;
                    if (diffVert > -JoystickTreshold && diffVert < JoystickTreshold) { diffVert = 0; }
                    short diffVertShort = (short)UtilMain.LinearMap(diffVert, -32768, 32767, -128, 128);
                    //------------------------------------------------------------------------------------------===

                    // Rover skid steering turn (uses x axis on right joystick)
                    short skidSteer = driveState.Gamepad.RightThumbX;
                    if (skidSteer > -JoystickTreshold && skidSteer < JoystickTreshold) { skidSteer = 0; }
                    float skidSteerSpeed = (float)UtilMain.LinearMap(skidSteer, -32768, 32767, -0.5, 0.5);
                    if (Math.Abs(skidSteerSpeed) < 0.0001f) { skidSteerSpeed = 0; }

                    // Rover skid steering speed (uses y axis on right joystick)
                    short skidDriving = driveState.Gamepad.RightThumbY;
                    if (skidDriving > -JoystickTreshold && skidDriving < JoystickTreshold) { skidDriving = 0; }
                    float skidDriveSpeed = (float)UtilMain.LinearMap(skidDriving, -32768, 32767, -0.5, 0.5);

                    if (skidDriving == 0) { skidDriveSpeed = 0; }
                    if (skidSteer == 0) { skidSteerSpeed = 0; }

                    float forward_back = skidDriveSpeed;
                    float left_right = skidSteerSpeed;

                    if (skidDriveSpeed == 0)
                    {
                        forward_back = speed;
                    }
                    if (skidSteerSpeed == 0)
                    {
                        left_right = steerPos;
                    }

                    //Console.WriteLine(skidSteerSpeed + " and " + skidDriveSpeed);

                    /*
                    short modeSet = -1;
                    if (manualDrive)
                    {
                        manualMode = true;
                        modeSet = 0;
                    }                        
                    else if (autoDrive)
                    {
                        manualMode = false;
                        modeSet = 1;
                    }
                    */
                    byte modeSet = 1;
                    if (ManualMode)
                    {
                        modeSet = 0;
                    }

                    //testing values to send for autonomous, remove later
                    float autoDriveSpeed = 0.0f;
                    if (bPressedAuto)
                        autoDriveSpeed = -0.5f;

                    float autoTurnSpeed = 0.0f;
                    if (aPressedAuto)
                        autoTurnSpeed = 0.5f;
                    //----------------------------------------------------
                    short fingerSpeed = 0;
                    if (rightTrigger > 0)
                        fingerSpeed = 128;
                    else if (leftTrigger > 0)
                        fingerSpeed = -128;

                    /*
                    if (startPressedArm)
                    {
                        if (scaler == 1.0)
                        {
                            scaler = 0.25; // precise arm movement
                        }
                        else
                        {
                            scaler = 1.0; // normal arm movement
                        }
                    } */

                    short wristArmSpeed = 0;
                    if (yPressedArm)
                        wristArmSpeed = (short)(-64 * scaler);
                    else if (xPressedArm)
                        wristArmSpeed = (short)(64 * scaler);

                    short elbowArmSpeed = 0;
                    if (bPressedArm)
                        elbowArmSpeed = (short)(64 * scaler);
                    else if (aPressedArm)
                        elbowArmSpeed = (short)(-64 * scaler);

                    short shoulderArmSpeed = 0;
                    if (downPressedArm)
                        shoulderArmSpeed = (short)(-94 * scaler);
                    else if (upPressedArm)
                        shoulderArmSpeed = (short)(94 * scaler);

                    short baseArmSpeed = 0;
                    if (rightPressedArm)
                        baseArmSpeed = (short)(64 * scaler);
                    else if (leftPressedArm)
                        baseArmSpeed = (short)(-64 * scaler);
                    /*
                    // Not being used due to rack and pinion steering not setup
                    Packet SteerPack = new Packet(0x8F, true, "MainRover");
                    SteerPack.AppendData(UtilData.ToBytes(steerSpeed));
                    Scarlet.Communications.Server.Send(SteerPack);*/
                    /*
                    Packet SpeedPack = new Packet(0x95, true, "MainRover");
                    SpeedPack.AppendData(UtilData.ToBytes(speed));
                    Scarlet.Communications.Server.Send(SpeedPack);
                    Console.WriteLine("Speed: " + speed + " Joystick " + skidDriveSpeed);*/

                    if (SendModeChange)
                    {
                        Console.WriteLine("calling mode changeer");
                        Packet ModePack = new Packet(0x99, true, "MainRover");
                        ModePack.AppendData(UtilData.ToBytes(modeSet));
                        Scarlet.Communications.Server.Send(ModePack);
                        Console.WriteLine("sending switching drive mode: " + modeSet);
                        SendModeChange = false;
                    }

                    if (ManualMode)
                    {

                        Packet SkidFrontRight = new Packet(0x90, true, "MainRover");
                        SkidFrontRight.AppendData(UtilData.ToBytes((sbyte)Math.Round((forward_back - left_right) * 120)));
                        Scarlet.Communications.Server.Send(SkidFrontRight);

                        Packet SkidRearRight = new Packet(0x92, true, "MainRover");
                        SkidRearRight.AppendData(UtilData.ToBytes((sbyte)Math.Round((forward_back - left_right) * 120)));
                        Scarlet.Communications.Server.Send(SkidRearRight);

                        Packet SkidFrontLeft = new Packet(0x91, true, "MainRover");
                        SkidFrontLeft.AppendData(UtilData.ToBytes((sbyte)Math.Round((forward_back + left_right) * 120)));
                        Scarlet.Communications.Server.Send(SkidFrontLeft);

                        Packet SkidRearLeft = new Packet(0x93, true, "MainRover");
                        SkidRearLeft.AppendData(UtilData.ToBytes((sbyte)Math.Round((0 - forward_back - left_right) * 120)));
                        Console.WriteLine("Test " + (-skidDriveSpeed - skidSteerSpeed));
                        Scarlet.Communications.Server.Send(SkidRearLeft);

                        Packet FingerPack = new Packet(0xA0, true, "MainArm");
                        FingerPack.AppendData(UtilData.ToBytes((short)fingerSpeed));
                        Scarlet.Communications.Server.Send(FingerPack);

                        Packet DiffHorzPack = new Packet(0x9F, true, "MainArm");
                        DiffHorzPack.AppendData(UtilData.ToBytes((short)(diffVertShort + diffHorzShort)));
                        Scarlet.Communications.Server.Send(DiffHorzPack);

                        Packet DiffVertPack = new Packet(0x9E, true, "MainArm");
                        DiffVertPack.AppendData(UtilData.ToBytes((short)(-diffVertShort + diffHorzShort)));
                        Scarlet.Communications.Server.Send(DiffVertPack);

                        Packet WristPack = new Packet(0x9D, true, "MainArm");
                        WristPack.AppendData(UtilData.ToBytes(wristArmSpeed));
                        Scarlet.Communications.Server.Send(WristPack);

                        Packet ElbowPack = new Packet(0x9C, true, "MainArm");
                        ElbowPack.AppendData(UtilData.ToBytes(elbowArmSpeed));
                        Scarlet.Communications.Server.Send(ElbowPack);

                        Packet ShoulderPack = new Packet(0x9B, true, "MainArm");
                        ShoulderPack.AppendData(UtilData.ToBytes(shoulderArmSpeed));
                        Scarlet.Communications.Server.Send(ShoulderPack);

                        Packet BasePack = new Packet(0x9A, true, "MainArm");
                        BasePack.AppendData(UtilData.ToBytes(baseArmSpeed));
                        Scarlet.Communications.Server.Send(BasePack);
                    }
                    else
                    {
                        /*
                        Packet SpeedPathPack = new Packet(0x96, true, "MainRover");
                        SpeedPathPack.AppendData(UtilData.ToBytes(skidDriveSpeed));
                        Scarlet.Communications.Server.Send(SpeedPathPack);

                        Packet TurnPathPack = new Packet(0x97, true, "MainRover");
                        TurnPathPack.AppendData(UtilData.ToBytes(skidSteerSpeed));
                        Scarlet.Communications.Server.Send(TurnPathPack);
                        */

                        // TODO 
                        // Sending desired location to jetson autonomous code should be done in mainwindow.xmal.cs
                        // in main rover should be in listening mode to read which way to go
                        // here, it should recieve any updates of its position from the main rover and print out
                        Console.WriteLine("desired location: " + target.Item1 + "   ,   " + target.Item2);

                    }

                }
                else
                {
                    HaltRoverMotion();
                    Console.WriteLine("desired location: " + target.Item1 + "   ,   " + target.Item2);
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

            Packet ArmEmergencyStop = new Packet(0x80, true, "MainArm");
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
            direction = magData.Data.Payload[1];
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
