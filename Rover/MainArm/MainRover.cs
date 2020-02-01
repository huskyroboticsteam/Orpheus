using System;
using Scarlet;
using Scarlet.IO.BeagleBone;
using Scarlet.IO;
using System.Collections.Generic;
using Scarlet.Components;
using Scarlet.Components.Sensors;
using Scarlet.Communications;
using Scarlet.Utilities;
using System.Threading;
using System.Threading.Tasks;

namespace MainRover
{
    public class MainRover
    {
        public static string SERVER_IP = "192.168.0.5";
        public const int NUM_PACKETS_TO_PROCESS = 20;

        public static bool Quit;
        public static List<ISensor> Sensors;
        public static QueueBuffer StopPackets;
        public static QueueBuffer DrivePackets;
        public static Arm armInterface;

        public static void PinConfig()
        {
            BBBPinManager.AddBusCAN(0, false);
            //BBBPinManager.AddMappingUART(Pins.MTK3339_RX);
            //BBBPinManager.AddMappingUART(Pins.MTK3339_TX);
            //BBBPinManager.AddMappingsI2C(Pins.BNO055_SCL, Pins.BNO055_SDA);
            //BBBPinManager.AddMappingGPIO(Pins.SteeringLimitSwitch, false, Scarlet.IO.ResistorState.PULL_UP, true);
            //BBBPinManager.AddMappingPWM(Pins.SteeringMotor);
            //BBBPinManager.AddMappingPWM(Pins.ServoMotor);
            //BBBPinManager.ApplyPinSettings(BBBPinManager.ApplicationMode.NO_CHANGES);
            BBBPinManager.ApplyPinSettings(BBBPinManager.ApplicationMode.APPLY_IF_NONE);
        }

        public static void InitBeagleBone()
        {
            StateStore.Start("MainArm");
            BeagleBone.Initialize(SystemMode.NO_HDMI, true);
            PinConfig();

        }

        public static void SetupClient()
        {
            Client.Start(SERVER_IP, 1025, 1026, "MainArm");
            DrivePackets = new QueueBuffer();
            StopPackets = new QueueBuffer();
            Parse.SetParseHandler(0x80, (Packet) => StopPackets.Enqueue(Packet, 0));
            for (byte i = 0x8E; i <= 0xA3; i++)
                Parse.SetParseHandler(i, (Packet) => DrivePackets.Enqueue(Packet, 0));
        }

        public static void ProcessInstructions()
        {
            if (!StopPackets.IsEmpty())
            {
                StopPackets = new QueueBuffer();

            }
            else
            {
                ProcessBasePackets();

            }
        }

        public static void ProcessBasePackets()
        {

            for (int i = 0; !DrivePackets.IsEmpty() && i < NUM_PACKETS_TO_PROCESS; i++)
            {
                Packet p = DrivePackets.Dequeue();
                switch ((PacketID)p.Data.ID)
                {
                    case PacketID.RPMFrontRight:
                    case PacketID.RPMFrontLeft:
                        byte driveaddress = (byte)(p.Data.ID - 0x8A + 18);
                        byte drivedirection = 0x00;
                        if (p.Data.Payload[0] > 0)
                        {
                            drivedirection = 0x01;
                            UtilCan.SpeedDir(CANBBB.CANBus0, false, 2, driveaddress, (byte)(-p.Data.Payload[1]), drivedirection);
                        }
                        else
                        {
                            UtilCan.SpeedDir(CANBBB.CANBus0, false, 2, driveaddress, p.Data.Payload[1], drivedirection);
                        }
                        break;
                    case PacketID.RPMBackRight:
                        byte drivedirectionbr = 0x00;
                        if (p.Data.Payload[0] > 0)
                        {
                            drivedirectionbr = 0x01;
                            UtilCan.SpeedDir(CANBBB.CANBus0, false, 2, 0x1A, (byte)(-p.Data.Payload[1]), drivedirectionbr);
                        }
                        else
                        {
                            UtilCan.SpeedDir(CANBBB.CANBus0, false, 2, 0x1A, p.Data.Payload[1], drivedirectionbr);
                        }
                        break;
                    case PacketID.RPMBackLeft:
                        byte drivedirectionbl = 0x00;
                        if (p.Data.Payload[0] > 0)
                        {
                            drivedirectionbl = 0x01;
                            UtilCan.SpeedDir(CANBBB.CANBus0, false, 2, 0x1B, (byte)(-p.Data.Payload[1]), drivedirectionbl);
                        }
                        else
                        {
                            UtilCan.SpeedDir(CANBBB.CANBus0, false, 2, 0x1B, p.Data.Payload[1], drivedirectionbl);
                        }
                        break;
                    case PacketID.BaseSpeed:
                    case PacketID.ShoulderSpeed:
                    case PacketID.ElbowSpeed:
                    case PacketID.WristSpeed:
                    case PacketID.DifferentialVert:
                    case PacketID.DifferentialRotate:
                    case PacketID.HandGrip:
                        byte address = (byte)(p.Data.ID - 0x8A);
                        byte direction = 0x00;
                        if (p.Data.Payload[0] > 0)
                        {
                            direction = 0x01;
                            //SetSpeed((Device)address, (byte)(-1 * p.Data.Payload[1]), direction);
                            UtilCan.SpeedDir(CANBBB.CANBus0, false, 2, address, (byte)(-p.Data.Payload[1]), direction);
                            //Console.WriteLine("ADDRESS :" + address + "DIR :" + direction + "PAY :" + (byte)(-p.Data.Payload[1]));
                        }
                        else
                        {
                            direction = 0x00;
                            //SetSpeed((Device)address, p.Data.Payload[1], direction);
                            UtilCan.SpeedDir(CANBBB.CANBus0, false, 2, address, p.Data.Payload[1], direction);
                            //Console.WriteLine("ADDRESS :" + address + "DIR :" + direction + "PAY :" + p.Data.Payload[1]);
                        }
                        break;
                    case PacketID.ArmServo:
                        if ((sbyte)p.Data.Payload[1] == 1)
                        {
                            UtilCan.ServoPos(CANBBB.CANBus0, false, 0x2, 22, 170);
                        }
                        else if ((sbyte)p.Data.Payload[1] == -1)
                        {
                            UtilCan.ServoPos(CANBBB.CANBus0, false, 0x2, 22, 10);
                        }
                        break;
                    case PacketID.ArmLaser:
                        UtilCan.LaserToggle(CANBBB.CANBus0, false, 0x2, 22, p.Data.Payload[1]);
                        break;
                }
            }
        }

        public static void SetupArm()
        {
            armInterface = new Arm(CANBBB.CANBus0);
            foreach(Device device in Vals.DEVICES)
            {
                SetMode(device, 0x00);
            }
        }

        private static void SetMode(Device armDevice, byte mode)
        {
            armInterface.Send(new ArmPacket() { 
                TargetDeviceID = armDevice,
                Priority = true,
                PacketType = CANPacket.MODE_SELECT,
                Payload = new byte[] { mode }
            });
        }

        private static void SetSpeed(Device armDevice, byte speed, byte direction)
        {
            armInterface.Send(new ArmPacket() {
                TargetDeviceID = armDevice,
                Priority = true,
                PacketType = CANPacket.SPEED_DIR,
                Payload = new byte[] { (byte)(speed << 1 | direction) }
            });
        }

        /// <summary>
        /// TODO: Interpret CAN Packets.
        /// </summary>
        private static void ProcessCAN()
        {
            if (armInterface.ReceiveQueueSize() > 0)
            {
                ArmPacket nextPacket = (ArmPacket)armInterface.ReadNext();
            }
        }

        public static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                SERVER_IP = args[0];
                Console.WriteLine("Using argument Specified IP of: " + SERVER_IP);
            }
            else
            {
                Console.WriteLine("Using default IP of: " + SERVER_IP);
            }
            Quit = false;
            InitBeagleBone();
            SetupClient();
            SetupArm();
            // readCan();
            MotorBoards.Initialize(CANBBB.CANBus0);
            Console.WriteLine("Finished the initalize");
            do
            {
                ProcessInstructions();
                //ProcessCAN();
                Thread.Sleep(50);
            } while (!Quit);
        }
    }
}
