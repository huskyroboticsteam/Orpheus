using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboticsLibrary.Utilities
{
    public static class Log
    {

        public static Source OutputType = Source.SENSORS;
        public static byte OutputLevel = 0;

        /// <summary>
        /// 
        /// </summary>
        public static void Output(byte Severity, Source Src, String Message)
        {
            if((Severity >= OutputLevel) && ((OutputType == Source.ALL) || (Src == OutputType)))
            {
                switch (Severity)
                {
                    case 0: Message = "[DBG] " + Message; break;
                    case 1: Message = "[INF] " + Message; break;
                    case 2: Message = "[WRN] " + Message; break;
                    case 3: Message = "[ERR] " + Message; break;
                    case 4: Message = "[FAT] " + Message; break;
                }
                Message = "[" + DateTime.Now.ToLongTimeString() + "] " + Message;
                System.Console.WriteLine(Message);
            }
        }

        public enum Source
        {
            ALL, MOTORS, NETWORK, GUI, SENSORS, OTHER
        }

    }
}
