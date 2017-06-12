using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoboticsLibrary.Errors;

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
                    case 0:
                        Message = "[DBG] " + Message;
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case 1:
                        Message = "[INF] " + Message;
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case 2:
                        Message = "[WRN] " + Message;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case 3:
                        Message = "[ERR] " + Message;
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case 4:
                        Message = "[FAT] " + Message;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Red;
                        break;
                }
                Message = "[" + DateTime.Now.ToLongTimeString() + "] " + Message;
                System.Console.WriteLine(Message);
                Console.ResetColor();
            }
        }

        public static void Exception(Source Src, Exception Ex)
        {
            string Prefix = "[" + DateTime.Now.ToLongTimeString() + "] [EXC] ";
            string[] Lines = Ex.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            Lines = Array.ConvertAll(Lines, Line => (Prefix + Line));
            Lines.ToList().ForEach(Console.WriteLine);
        }

        public static void Error(short Error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            byte System = (byte)(Error >> 8);
            byte Code = (byte)(Error & 0x00FF);
            Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [ERR] [0x" + Error.ToString("X4") + "] [" + ErrorCodes.Systems[System] + "] " + ErrorCodes.List[System][Code]);
            Console.ResetColor();
        }

        public enum Source
        {
            ALL, MOTORS, NETWORK, GUI, SENSORS, CAMERAS, OTHER
        }

    }
}
