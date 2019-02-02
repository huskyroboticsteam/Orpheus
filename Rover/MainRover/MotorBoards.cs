using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scarlet.Controllers;
using Scarlet.IO;
using Scarlet.IO.BeagleBone;

namespace MainRover
{
    static class MotorBoards
    {

        public static void Initialize(CANBusBBB CANBus)
        {
            GetCSVData CSVobj = new GetCSVData();
            CSVobj.Initialize();

            // Send Model Request to each Motor Board
            for (int i = 0; i < 7; i++)
            {
                UtilCan.ModelReq(CANBus, true, 2, Convert.ToByte(16 + i));
            }

            int count = 100; // How many reads to do before giving up
            for (int j = 0; j < count; j++)
            {
                Task<Tuple<uint, byte[]>> CanRead = CANBus.ReadAsync();
                int msec = 0;
                while (!CanRead.IsCompleted || (msec > 10))
                    CanRead.Wait(100);

                if (CanRead.IsCompleted)
                {
                    Tuple<uint, byte[]> temp = CanRead.Result;
                    uint sender = ((temp.Item1) >> 0x1F) & 0x1F;
                    byte receiver = Convert.ToByte((temp.Item1) & 0x1F);
                    byte[] data = temp.Item2;

                    
                    if (data[0] == 0x12 && receiver == 0x2)
                    {
                        MotorBoardData MBD;
                        switch (receiver)
                        {
                            case 0x10:
                                MBD = CSVobj.getMB1(data[1]);
                                break;
                            case 0x11:
                                MBD = CSVobj.getMB2(data[1]);
                                break;
                            case 0x12:
                                MBD = CSVobj.getMB3(data[1]);
                                break;
                            case 0x13:
                                MBD = CSVobj.getMB4(data[1]);
                                break;
                            case 0x14:
                                MBD = CSVobj.getMB5(data[1]);
                                break;
                            case 0x15:
                                MBD = CSVobj.getMB6(data[1]);
                                break;
                            case 0x16:
                                MBD = CSVobj.getMB7(data[1]);
                                break;
                            default:
                                MBD = null;
                                Console.WriteLine("CAN response receiver not known");
                                break;
                        }
                        sendPIDT(CANBus, receiver, MBD);
                    }
                }
                else
                {
                    CanRead.Dispose();
                    count = 100;
                }
            }
            
        }

        private static void sendPIDT(ICANBus CANBus, byte receiver, MotorBoardData MBD)
        {
            if (MBD == null)
            {
                Console.WriteLine("MotorBoard ID not found in CSV");
            }
            else
            {
                UtilCan.SetP(CANBus, true, 0x2, receiver, MBD.P, 0);
                UtilCan.SetI(CANBus, true, 0x2, receiver, MBD.I, 0);
                UtilCan.SetD(CANBus, true, 0x2, receiver, MBD.D, 0);
                UtilCan.SetTicksPerRev(CANBus, true, 0x2, receiver, MBD.TicksPerRev);
            }
        }
    }
}
