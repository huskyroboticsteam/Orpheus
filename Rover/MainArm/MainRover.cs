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
            for (byte i = 0x9A; i <= 0xA1; i++)
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
                            UtilCan.SpeedDir(CANBBB.CANBus0, false, 2, address, (byte)(-p.Data.Payload[1]), direction);
                            Console.WriteLine("ADDRESS :" + address + "DIR :" + direction + "PAY :" + (byte)(-p.Data.Payload[1]));
                        }
                        else
                        {
                            direction = 0x00;
                            UtilCan.SpeedDir(CANBBB.CANBus0, false, 2, address, p.Data.Payload[1], direction);
                            Console.WriteLine("ADDRESS :" + address + "DIR :" + direction + "PAY :" + p.Data.Payload[1]);
                        }


                        break;
                }
            }
        }

        public static void readCan()
        {
            Task<Tuple<uint, byte[]>> CanRead = CANBBB.CANBus0.ReadAsync();
            int msec = 0;
            while (!CanRead.IsCompleted || (msec > 10))
                CanRead.Wait(100);

            if (CanRead.IsCompleted)
            {
                Tuple<uint, byte[]> temp = CanRead.Result;
                byte sender = Convert.ToByte(((temp.Item1) >> 0x1F) & 0x1F);
                byte receiver = Convert.ToByte((temp.Item1) & 0x1F);
                
                if (receiver == 2)
                {
                    if (temp.Item2[0] == 0x18)
                    {
                        Tuple<short, short> voltCur= UtilCan.GetTele(temp.Item2);
                        
                        Packet Pack = new Packet((byte)PacketID.CanVoltage, true);
                        Pack.AppendData(UtilData.ToBytes(voltCur.Item1));
                        Client.SendNow(Pack);

                        Packet Pack2 = new Packet((byte)PacketID.CanCurrent, true);
                        Pack.AppendData(UtilData.ToBytes(voltCur.Item2));
                        Client.SendNow(Pack);
                    }
                }


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
            //readCan();
            MotorBoards.Initialize(CANBBB.CANBus0);
            Console.WriteLine("Finished the initalize");
            do
            {
                ProcessInstructions();
                Thread.Sleep(50);
            } while (!Quit);
        }
    }
}
