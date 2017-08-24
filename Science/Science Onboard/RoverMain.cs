using System;
using System.Net;
using System.Threading;
using Scarlet.Communications;
using Scarlet.IO.BeagleBone;
using Scarlet.Science;
using Scarlet.Utilities;

namespace Science
{
	class RoverMain
	{
        public static IOHandler IOHandler { get; private set; }
        private static string IP = Constants.DEFAULT_SERVER_IP;
        private static int PortTCP = Constants.DEFAULT_PORT_TCP;
        private static int PortUDP = Constants.DEFAULT_PORT_UDP;

        static void Main(string[] Args)
		{
            ParseArgs(Args);
            Log.SetGlobalOutputLevel(Log.Severity.INFO);
            Log.SetSingleOutputLevel(Log.Source.NETWORK, Log.Severity.DEBUG);
            Log.ErrorCodes = ScienceErrors.ERROR_CODES;
            Log.SystemNames = ScienceErrors.SYSTEMS;
            Log.Begin();
            Log.ForceOutput(Log.Severity.INFO, Log.Source.OTHER, "Science Station - Rover Side");

            Test();

            IOHandler = new IOHandler();
            Client.Start(IP, PortTCP, PortUDP, Constants.CLIENT_NAME);
            PacketHandler PackHan = new PacketHandler();

            

            while(true)
            {
                Thread.Sleep(100);
            }

            while (Console.KeyAvailable) { Console.ReadKey(); }
            Log.ForceOutput(Log.Severity.INFO, Log.Source.OTHER, "Press any key to exit.");
            Console.ReadKey();
            Environment.Exit(0);
		}

        private static void Test()
        {
            BeagleBone.Initialize(SystemMode.DEFAULT, true);
            BBBPinManager.AddMapping(BBBPin.P8_08, true, Scarlet.IO.ResistorState.PULL_DOWN, BBBPinMode.GPIO);
            BBBPinManager.ApplyPinSettings();
        }

        private static void ParseArgs(string[] Args)
        {
            if (Args == null || Args.Length == 0) { return; } // Nothing to parse.
            for(int i = 0; i < Args.Length; i++)
            {
                if (Args.Length > i + 1) // Dual-part arguments.
                {
                    if (Args[i] == "-s" || Args[i] == "--server")
                    {
                        IP = Args[i + 1];
                        i++;
                    }
                    if(Args[i] == "-pt" || Args[i] == "--port-tcp")
                    {
                        PortTCP = int.Parse(Args[i + 1]);
                        i++;
                    }
                    if (Args[i] == "-pu" || Args[i] == "--port-udp")
                    {
                        PortUDP = int.Parse(Args[i + 1]);
                        i++;
                    }
                }
                // Single-part arguments
                if(Args[i] == "-?" || Args[i] == "/?" || Args[i] == "-h" || Args[i] == "/h" || Args[i] == "help" || Args[i] == "-help" || Args[i] == "--help")
                {
                    Console.WriteLine("Command-line paramters:");
                    Console.WriteLine("  -?|/?|-h|/h|help|-help|--help : Outputs this text.");
                    Console.WriteLine("  -s|--server <IP> : Connects to the given server instead of the default.");
                    Console.WriteLine("  -pt|--port-tcp <Port> : Connects to the server via TCP using the given port instead of the default.");
                    Console.WriteLine("  -pu|--port-udp <Port> : Connects to the server via UDP using the given port instead of the default.");
                }
            }
        }
	}
}
