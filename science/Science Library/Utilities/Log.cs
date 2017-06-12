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
        // Override these in your program.
        // OutputType determines which system you see output from.
        // OutputLevel determines the minimum severity required for a message to be output.
        public static Source OutputType = Source.SENSORS;
        public static byte OutputLevel = 0;

        /// <summary>
        /// Outputs a general log message if configured to output this type of message.
        /// </summary>
        /// <param name="Severity">How severe this is.
        /// 0 = Debug (Used for program flow debugging and light troubleshooting)
        /// 1 = Information (Used for troubleshooting)
        /// 2 = Warning (When an issue arises, but functionality sees little to no impact)
        /// 3 = Error (When an issue arises that causes significant loss of functionality to one system)
        /// 4 = Fatal (When an issue arises that causes loss of functionality to multiple systems or complete shutdown)
        /// </param>
        /// <param name="Src">The system where this log entry is originating.</param>
        /// <param name="Message">The actual log entry to output.</param>
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

        /// <summary>
        /// Outputs an Exception objet in the form of a stack trace to the console.
        /// Exceptions are always output.
        /// </summary>
        /// <param name="Src">The system that this Exception is originating from.</param>
        /// <param name="Ex">The Exception object.</param>
        public static void Exception(Source Src, Exception Ex)
        {
            string Prefix = "[" + DateTime.Now.ToLongTimeString() + "] [EXC] ";
            string[] Lines = Ex.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            Lines = Array.ConvertAll(Lines, Line => (Prefix + Line));
            Lines.ToList().ForEach(Console.WriteLine);
        }

        /// <summary>
        /// Outputs a defined error code to the console.
        /// </summary>
        /// <param name="Error">The error code, in standard form. E.g. 0x0000 for all OK.</param>
        public static void Error(short Error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            byte System = (byte)(Error >> 8);
            byte Code = (byte)(Error & 0x00FF);
            Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [ERR] [0x" + Error.ToString("X4") + "] [" + ErrorCodes.Systems[System] + "] " + ErrorCodes.List[System][Code]);
            Console.ResetColor();
        }

        /// <summary>
        /// The subsystem where the error occured, for use in output filtering.
        /// </summary>
        public enum Source
        {
            ALL, MOTORS, NETWORK, GUI, SENSORS, CAMERAS, OTHER
        }

    }
}
