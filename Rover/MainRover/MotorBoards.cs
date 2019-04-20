using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scarlet.Controllers;
using Scarlet.IO;
using Scarlet.IO.BeagleBone;
using Scarlet.Utilities;

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

            // Send Model Request to each Motor Board
            for (int i = 0; i < 7; i++)
            {
                UtilCan.ModeSelect(CANBus, true, 2, Convert.ToByte(16 + i), 0);
            }
            /*
            int count = 10; // How many reads to do before giving up
            for (int j = 0; j < count; j++)
            {
                Task<Tuple<uint, byte[]>> CanRead = CANBus.ReadAsync();
                int msec = 0;
                while (!CanRead.IsCompleted || (msec > 10))
                    CanRead.Wait(100);

                if (CanRead.IsCompleted)
                {
                    Tuple<uint, byte[]> temp = CanRead.Result;
                    byte sender = Convert.ToByte(((temp.Item1) >> 0x1F) & 0x1F);
                    byte receiver = Convert.ToByte((temp.Item1) & 0x1F);
                    byte[] data = temp.Item2;

                    
                    if (data[0] == 0x12 && receiver == 0x2)
                    {
                        MotorBoardData MBD;
                        switch (sender)
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
                                Log.Output(Log.Severity.WARNING, Log.Source.HARDWAREIO, "CAN ID " + sender + " is not a known Motorboard ID");
                                break;
                        }
                        sendPIDT(CANBus, sender, MBD);
                    }
                }
                else
                {
                    CanRead.Dispose();
                    count = 0;
                }
            }*/
            
        }

        private static void sendPIDT(ICANBus CANBus, byte receiver, MotorBoardData MBD)
        {
            if (MBD != null)
            {
                UtilCan.SetP(CANBus, true, 0x2, receiver, MBD.P, 0);
                UtilCan.SetI(CANBus, true, 0x2, receiver, MBD.I, 0);
                UtilCan.SetD(CANBus, true, 0x2, receiver, MBD.D, 0);
                UtilCan.SetTicksPerRev(CANBus, true, 0x2, receiver, MBD.TicksPerRev);
                Log.Output(Log.Severity.INFO, Log.Source.HARDWAREIO, 
                    "MotorBoard with CAN ID " + receiver + " set: Model=" + MBD.Model + " P=" + MBD.P + " I=" + MBD.I + " D=" + MBD.D);
            }
        }
    }
}
