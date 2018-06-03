using System;
using Scarlet;
using Scarlet.IO.BeagleBone;
using System.Collections.Generic;
using Scarlet.Components;
using Scarlet.Components.Sensors;
using Scarlet.Communications;
using Scarlet.Utilities;
using System.Threading;
namespace MainRover
{
    public class MainRover
    {
        public const string SERVER_IP = "192.168.0.5";

        public static bool Quit;
        public static List<ISensor> Sensors;
        public static QueueBuffer Packets;

        public static void PinConfig()
        {
            BBBPinManager.AddBusCAN(0);
            BBBPinManager.AddMappingUART(Pins.MTK3339_RX);
            BBBPinManager.AddMappingUART(Pins.MTK3339_TX);
            BBBPinManager.AddMappingsI2C(Pins.BNO055_SCL, Pins.BNO055_SDA);
            BBBPinManager.AddMappingGPIO(Pins.SteeringLimitSwitch, false, Scarlet.IO.ResistorState.PULL_UP, true);
            BBBPinManager.AddMappingPWM(Pins.SteeringMotor);
            BBBPinManager.ApplyPinSettings(BBBPinManager.ApplicationMode.NO_CHANGES);
        }

        public static void InitBeagleBone()
        {
            StateStore.Start("MainRover");
            BeagleBone.Initialize(SystemMode.NO_HDMI, true);
            PinConfig();
            Sensors = new List<ISensor>();
            Sensors.Add(new BNO055(I2CBBB.I2CBus2));
            Sensors.Add(new MTK3339(UARTBBB.UARTBus4));
            LimitSwitch Switch = new LimitSwitch(new DigitalInBBB(Pins.SteeringLimitSwitch));
            Switch.SwitchToggle += (object sender, LimitSwitchToggle e) => Console.WriteLine("PRESSED!");
            Sensors.Add(Switch);
            //Add encoders
        }

        public static void SetupClient()
        {
            Client.Start(SERVER_IP, 1025, 1026, "MainRover");
            Packets = new QueueBuffer();
            for (byte i = 0; i < 200; i++)
                Parse.SetParseHandler(i, (Packet) => Packets.Enqueue(Packet, 0));
        }

        public static void ProcessInstructions()
        {
            const int NUM_PACKETS_TO_PROCESS = 20;
            for (int i = 0; !Packets.IsEmpty() && i < NUM_PACKETS_TO_PROCESS; i++)
            {
                Packet p = Packets.Dequeue();
                switch ((PacketID)p.Data.ID)
                {
                    case PacketID.RPMAllDriveMotors:
                        MotorControl.SetAllRPM((sbyte)p.Data.Payload[0]);
                        break;
                    case PacketID.RPMFrontRight:
                    case PacketID.RPMFrontLeft:
                    case PacketID.RPMBackRight:
                    case PacketID.RPMBackLeft:
                        int MotorID = p.Data.ID - (byte)PacketID.RPMFrontRight + 1;
                        MotorControl.SetRPM(MotorID, (sbyte)p.Data.Payload[0]);
                        break;
                    case PacketID.RPMSteeringMotor:
                        float SteerSpeed = UtilData.ToFloat(p.Data.Payload);
                        MotorControl.SetSteerSpeed(SteerSpeed);
                        break;
                    case PacketID.SteerPosition:
                        float Position = UtilData.ToFloat(p.Data.Payload);
                        MotorControl.SetRackAndPinionPosition(Position);
                        break;
                    case PacketID.SpeedAllDriveMotors:
                        float Speed = UtilData.ToFloat(p.Data.Payload);
                        MotorControl.SetAllSpeed(Speed);
                        break;
                }
            }
        }

        public static void SendSensorData()
        {
            foreach (ISensor Sensor in Sensors)
            {
                if (Sensor is MTK3339)
                {
                    var Tup = ((MTK3339)Sensor).GetCoords();
                    float Lat = Tup.Item1;
                    float Long = Tup.Item2;
                    Packet Pack = new Packet((byte)PacketID.DataGPS, true);
                    Pack.AppendData(UtilData.ToBytes(Lat));
                    Pack.AppendData(UtilData.ToBytes(Long));
                    Client.SendNow(Pack);
                }
                if (Sensor is BNO055)
                {
                    var Tup = ((BNO055)Sensor).GetVector(BNO055.VectorType.VECTOR_MAGNETOMETER);
                    float X = Tup.Item1;
                    float Y = Tup.Item2;
                    float Z = Tup.Item3;
                    Packet Pack = new Packet((byte)PacketID.DataMagnetometer, true);
                    Pack.AppendData(UtilData.ToBytes(X));
                    Pack.AppendData(UtilData.ToBytes(Y));
                    Pack.AppendData(UtilData.ToBytes(Z));
                    Client.SendNow(Pack);
                }
            }
        }

        public static void Main()
        {
            Quit = false;
            InitBeagleBone();
            SetupClient();
            MotorControl.Initialize();
            do
            {
                SendSensorData();
                ProcessInstructions();
                Thread.Sleep(50);
            } while (!Quit);
        }
    }
}
