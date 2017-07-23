using System;
using System.Net;
using System.Threading;
using Scarlet.Communications;
using Scarlet.Science;
using Scarlet.Utilities;

namespace Science
{
	class RoverMain
	{
        public static IOHandler IOHandler { get; private set; }

		static void Main(string[] args)
		{
            Log.OutputLevel = Log.Severity.DEBUG;
            Log.OutputType = Log.Source.ALL;
            Log.ErrorCodes = ScienceErrors.ERROR_CODES;
            Log.SystemNames = ScienceErrors.SYSTEMS;
            Log.Begin();
            Log.ForceOutput(Log.Severity.INFO, Log.Source.OTHER, "Science Station - Rover Side");

            IOHandler = new IOHandler();
            Client.Start("192.168.0.108", 10765, 11765, Scarlet.Science.Constants.CLIENT_NAME);
            PacketHandler PackHan = new PacketHandler();

            RunTests();

            while(true)
            {
                Thread.Sleep(100);
            }


			Log.Stop();

            while (Console.KeyAvailable) { Console.ReadKey(); }
            Log.ForceOutput(Log.Severity.INFO, Log.Source.OTHER, "Press any key to exit.");
            Console.ReadKey();
            Environment.Exit(0);
		}

        static void RunTests()
        {
            Packet Pack = new Packet(0x00, false);
            Pack.AppendData(new byte[] { 0x00, 0x58, 0x6E, 0xFF, 0xEE, 0xCC, 0xBB, 0xAA, 0x99, 0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11, 0x00, 0x00, 0x00 });
            Log.Output(Log.Severity.DEBUG, Log.Source.OTHER, Pack.ToString());
        }
	}
}
