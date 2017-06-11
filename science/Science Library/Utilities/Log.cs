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
        public static void Output(byte Severity, Source Src, string Message)
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

        public static void Exception(Source Src, Exception Ex)
        {
            string Prefix = "[" + DateTime.Now.ToLongTimeString() + "] [EXC] ";
            string[] Lines = Ex.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            Lines = Array.ConvertAll(Lines, Line => (Prefix + Line));
            Lines.ToList().ForEach(Console.WriteLine);
        }

        public enum Source
        {
            ALL, MOTORS, NETWORK, GUI, SENSORS, OTHER
        }

    }
}
