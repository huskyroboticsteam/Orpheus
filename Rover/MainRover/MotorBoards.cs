using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scarlet.Controllers;
using Scarlet.IO;

namespace MainRover
{
    static class MotorBoards
    {

        public static void Initialize(ICANBus CANBus)
        {
            GetCSVData CSVobj = new GetCSVData();
            CSVobj.Initialize();

            // Send Model Request to each Motor Board
            for (int i = 0; i < 7; i++)
            {
                UtilCan.ModelReq(CANBus, true, 2, Convert.ToByte(16 + i));
            }

            // TODO Read through can input until needed ones are seen. 
            Tuple<uint, byte[]> readCan = CANBus.Read();
        }
    }
}
