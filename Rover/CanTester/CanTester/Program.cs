using Scarlet.IO;
using Scarlet.IO.BeagleBone;
using Scarlet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CanTester
{
    class Program
    {
        static void Main(string[] args)
        {
            StateStore.Start("CanTester");
            BeagleBone.Initialize(SystemMode.NO_HDMI, true);
            BBBPinManager.AddBusCAN(0, false);
            BBBPinManager.ApplyPinSettings(BBBPinManager.ApplicationMode.APPLY_IF_NONE);
            
            bool Priority = false;
            byte Sender = 0;
            byte Receiver = 0;
            byte DataID = 0;
            bool continuez = true;
            UInt16 Val1 = 0;
            UInt16 Val2 = 0;
            byte Speed = 0;
            byte Direction = 0;
            byte Mode = 0;
            byte position = 0;
            byte lasertoggle = 0;

            Console.WriteLine("CAN TESTER TOOL");

            while (continuez)
            {
                Console.Write("Write Mode(true) or Read Mode(false)? (true/false): ");
                bool Write = oldNew(Console.ReadLine(), true);
                if (Write) // Write Mode
                {
                    Console.Write("Enter Priority (true/false) [Previous " + Priority + " ]: ");
                    Priority = oldNew(Console.ReadLine(), Priority);
                    Console.Write("Enter Sender (byte) [Previous " + Sender + "]: ");
                    Sender = oldNew(Console.ReadLine(), Sender);
                    Console.Write("Enter Reciever (byte) [Previous " + Receiver + "]: ");
                    Receiver = oldNew(Console.ReadLine(), Receiver);
                    Console.Write("Enter DataID (byte) [Previous " + DataID + "]: ");
                    DataID = oldNew(Console.ReadLine(), DataID);
                    switch (DataID)
                    {
                        case 0x0:
                            Console.Write("Enter in mode [Previous " + Mode + "]: ");
                            Mode = oldNew(Console.ReadLine(), Mode);
                            break;
                        case 0x2:
                            Console.Write("Enter in speed [Previous " + Speed + "]: ");
                            Speed = oldNew(Console.ReadLine(), Speed);
                            Console.Write("Enter in direction (1 or 0) [Previous " + Direction + "]: ");
                            Direction = oldNew(Console.ReadLine(), Direction);
                            break;
                        case 0x4:
                        case 0xA:
                        case 0xC:
                        case 0xE:
                        case 0x14:
                        case 0x18:
                            Console.Write("Enter in first value [Previous " + Val1 + "]: ");
                            Val1 = oldNew(Console.ReadLine(), Val1);
                            Console.Write("Enter in second value [Previous " + Val2 + "]: ");
                            Val2 = oldNew(Console.ReadLine(), Val2);
                            break;
                        case 0x16:
                            break;
                        case 0x22:
                            Console.Write("Enter in servo position [Previous " + position + "]: ");
                            position = oldNew(Console.ReadLine(), position);
                            break;
                        case 0x24:
                            Console.Write("Enter in laser toggle [Previous " + lasertoggle + "]: ");
                            lasertoggle = oldNew(Console.ReadLine(), lasertoggle);
                            break;
                        default:
                            Console.WriteLine("Unknown or unsupported data value");
                            break;
                    }

                    bool keepSending = true;
                    while (keepSending)
                    {
                        switch (DataID)
                        {
                            case 0x0:
                                ModeSelect(CANBBB.CANBus0, Priority, Sender, Receiver, Mode);
                                break;
                            case 0x2:
                                SpeedDir(CANBBB.CANBus0, Priority, Sender, Receiver, Speed, Direction);
                                break;
                            case 0x4:
                            case 0xA:
                            case 0xC:
                            case 0xE:
                            case 0x14:
                            case 0x18:
                                TwoData(CANBBB.CANBus0, Priority, Sender, Receiver, DataID, Val1, Val2);
                                break;
                            case 0x10:
                                ModelReq(CANBBB.CANBus0, Priority, Sender, Receiver);
                                break;
                            case 0x22:
                                ServoPos(CANBBB.CANBus0, Priority, Sender, Receiver, position);
                                break;
                            case 0x24:
                                LaserToggle(CANBBB.CANBus0, Priority, Sender, Receiver, lasertoggle);
                                break;
                            default:
                                Console.WriteLine("Unknown or unsupported data value");
                                break;
                        }
                        Console.Write("Data Sent. Hold enter to continue sending (true/false): ");
                        try
                        {
                            keepSending = oldNew(Console.ReadLine(), true);
                        }
                        catch
                        {
                            if (DataID == 2)
                            {
                                Console.WriteLine("Speed set to zero");
                                SpeedDir(CANBBB.CANBus0, Priority, Sender, Receiver, 0, Direction);
                                keepSending = false;
                            }
                        }
                        
                    }
                }
                else // Read Mode
                {
                    bool reading = true;
                    while (reading)
                    {
                        int count = 0; 
                        Task<Tuple<uint, byte[]>> CanRead = CANBBB.CANBus0.ReadAsync();
                        while (!CanRead.IsCompleted || (count > 10))
                        {
                            CanRead.Wait(1000);
                            count++;
                        }

                        if (CanRead.IsCompleted)
                        {
                            Tuple<uint, byte[]> temp = CanRead.Result;
                            byte priority = Convert.ToByte(((temp.Item1) >> 0b1111111111) & 0x1F);
                            byte sender = Convert.ToByte(((temp.Item1) >> 0x1F) & 0x1F);
                            byte receiver = Convert.ToByte((temp.Item1) & 0x1F);
                            Console.WriteLine("ID: " + Convert.ToString(temp.Item1, 2));
                            //Console.WriteLine("ID: " + priority + " " + sender + " " + receiver);

                            Console.WriteLine("Data: {0}",
                                string.Join(", ", temp.Item2.Select(v => v.ToString()))
                            );
                        }
                        else
                        {
                            Console.WriteLine("TIMEOUT, No CANID found");
                        }
                        Console.Write("Continue read?");
                        reading = oldNew(Console.ReadLine(), true);
                    }
                }

                Console.Write("Continue Program (true) or exit (false): ");
                continuez = oldNew(Console.ReadLine(), true);

            }    

        }

        private static UInt16 oldNew(String newVal, UInt16 oldVal)
        {
            if (newVal == "") return oldVal;
            else return Convert.ToUInt16(newVal);
        }

        private static byte oldNewHex(String newVal, byte oldVal)
        {
            if (newVal == "") return oldVal;
            else return Convert.ToByte(newVal, 16);
        }

        private static byte oldNew(String newVal, byte oldVal)
        {
            if (newVal == "") return oldVal;
            else return Convert.ToByte(newVal);
        }

        private static bool oldNew(String newVal, bool oldVal)
        {
            if (newVal == "") return oldVal;
            else return Convert.ToBoolean(newVal);
        }

        private static uint ConstructCanID(bool Priority, byte Sender, byte Receiver)
        {
            return Convert.ToUInt16((Convert.ToUInt16(!Priority) << 10) + (Sender << 5) + Receiver);
        }

        /// <summary>
        /// Write two 16-bit values to a can frame.
        /// </summary>
        private static void TwoData(ICANBus CANBus, bool Priority, byte Sender, byte Receiver, byte DataID, UInt16 val1, UInt16 val2)
        {
            byte[] Payload = new byte[5];
            Payload[0] = DataID;
            Payload[1] = (byte)(val1 & 0xFF);
            Payload[2] = (byte)((val1 >> 8) & 0xFF);
            Payload[3] = (byte)(val2 & 0xFF);
            Payload[4] = (byte)((val2 >> 8) & 0xFF);
            CANBus.Write(ConstructCanID(Priority, Sender, Receiver), Payload);
        }

        public static void ModeSelect(ICANBus CANBus, bool Priority, byte Sender, byte Receiver, byte Mode)
        {
            byte[] Payload = new byte[2];
            Payload[0] = 0;
            Payload[1] = Mode;
            CANBus.Write(ConstructCanID(Priority, Sender, Receiver), Payload);
        }

        public static void SpeedDir(ICANBus CANBus, bool Priority, byte Sender, byte Receiver, byte Speed, byte Direction)
        {
            byte[] Payload = new byte[3];
            Payload[0] = 2;
            Payload[1] = Speed;
            Payload[2] = Direction;
            CANBus.Write(ConstructCanID(Priority, Sender, Receiver), Payload);
        }

        public static void AngleSpeed(ICANBus CANBus, bool Priority, byte Sender, byte Receiver, UInt16 Angle, UInt16 Velocity)
        {
            TwoData(CANBus, Priority, Sender, Receiver, 4, Angle, Velocity);
        }

        public static void SetP(ICANBus CANBus, bool Priority, byte Sender, byte Receiver, UInt16 val1, UInt16 val2)
        {
            TwoData(CANBus, Priority, Sender, Receiver, 10, val1, val2);
        }

        public static void SetI(ICANBus CANBus, bool Priority, byte Sender, byte Receiver, UInt16 val1, UInt16 val2)
        {
            TwoData(CANBus, Priority, Sender, Receiver, 12, val1, val2);
        }

        public static void SetD(ICANBus CANBus, bool Priority, byte Sender, byte Receiver, UInt16 val1, UInt16 val2)
        {
            TwoData(CANBus, Priority, Sender, Receiver, 14, val1, val2);
        }

        public static void SetTicksPerRev(ICANBus CANBus, bool Priority, byte Sender, byte Receiver, UInt16 TPR)
        {
            byte[] Payload = new byte[3];
            Payload[0] = 15;
            Payload[1] = (byte)(TPR & 0xFF);
            Payload[2] = (byte)((TPR >> 8) & 0xFF);
            CANBus.Write(ConstructCanID(Priority, Sender, Receiver), Payload);
        }

        public static void ModelReq(ICANBus CANBus, bool Priority, byte Sender, byte Receiver)
        {
            byte[] Payload = new byte[1];
            Payload[0] = 16;
            CANBus.Write(ConstructCanID(Priority, Sender, Receiver), Payload);
        }

        public static void ServoPos(ICANBus CANBus, bool Priority, byte Sender, byte Receiver, byte position)
        {
            byte[] Payload = new byte[2];
            Payload[0] = 0x22;
            Payload[1] = position;
            CANBus.Write(ConstructCanID(Priority, Sender, Receiver), Payload);
        }

        public static void LaserToggle(ICANBus CANBus, bool Priority, byte Sender, byte Receiver, byte toggle)
        {
            byte[] Payload = new byte[2];
            Payload[0] = 0x24;
            Payload[1] = toggle;
            CANBus.Write(ConstructCanID(Priority, Sender, Receiver), Payload);
        }

    }
}
