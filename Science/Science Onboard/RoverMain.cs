using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using BBBCSIO;
using Scarlet.Communications;
using Scarlet.Components;
using Scarlet.Components.Motors;
using Scarlet.Components.Sensors;
using Scarlet.Filters;
using Scarlet.IO;
using Scarlet.IO.BeagleBone;
using Scarlet.IO.RaspberryPi;
using Scarlet.Science;
using Scarlet.Utilities;
using Science.Systems;

namespace Science
{
	class RoverMain
	{
        public static IOHandler IOHandler { get; private set; }
        private static string IP = Constants.DEFAULT_SERVER_IP;
        private static int PortTCP = Constants.DEFAULT_PORT_TCP;
        private static int PortUDP = Constants.DEFAULT_PORT_UDP;
        public static BBBPinManager.ApplicationMode ApplyDevTree = BBBPinManager.ApplicationMode.APPLY_IF_NONE;

        static void Main(string[] Args)
		{
            ParseArgs(Args);
            StateStore.Start("SciRover");
            Log.SetGlobalOutputLevel(Log.Severity.DEBUG);
            //Log.SetSingleOutputLevel(Log.Source.NETWORK, Log.Severity.DEBUG);
            Log.ErrorCodes = ScienceErrors.ERROR_CODES;
            Log.SystemNames = ScienceErrors.SYSTEMS;
            Log.Begin();
            Log.ForceOutput(Log.Severity.INFO, Log.Source.OTHER, "Science Station - Rover Side");
            Client.Start(IP, PortTCP, PortUDP, "SciRover");

            BeagleBone.Initialize(SystemMode.DEFAULT, true);
            IOHandler = new IOHandler();
            IOHandler.InitializeSystems();
            ((Turntable)IOHandler.TurntableController).TargetAngle = 50;

            while (Console.KeyAvailable) { Console.ReadKey(); } // Clear previous keypresses

            while (!Console.KeyAvailable)
            {
                IOHandler.UpdateStates();
                Thread.Sleep(20);
            }

            Log.ForceOutput(Log.Severity.INFO, Log.Source.OTHER, "Press any key to exit.");
            Console.ReadKey();
            Environment.Exit(0);
		}

        private static void ParseArgs(string[] Args)
        {
            if (Args == null || Args.Length == 0) { return; } // Nothing to parse.
            for(int i = 0; i < Args.Length; i++)
            {
                if (Args.Length > i + 1) // Dual-part arguments.
                {
                    if (Args[i] == "-s" || Args[i] == "--server") { IP = Args[i + 1]; i++; }
                    if (Args[i] == "-pt" || Args[i] == "--port-tcp") { PortTCP = int.Parse(Args[i + 1]); i++; }
                    if (Args[i] == "-pu" || Args[i] == "--port-udp") { PortUDP = int.Parse(Args[i + 1]); i++; }
                }
                // Single-part arguments
                if(Args[i] == "-?" || Args[i] == "/?" || Args[i] == "-h" || Args[i] == "/h" || Args[i] == "help" || Args[i] == "-help" || Args[i] == "--help")
                {
                    Console.WriteLine("Command-line paramters:");
                    Console.WriteLine("  -?|/?|-h|/h|help|-help|--help : Outputs this text.");
                    Console.WriteLine("  -s|--server <IP> : Connects to the given server instead of the default.");
                    Console.WriteLine("  -pt|--port-tcp <Port> : Connects to the server via TCP using the given port instead of the default.");
                    Console.WriteLine("  -pu|--port-udp <Port> : Connects to the server via UDP using the given port instead of the default.");
                    Console.WriteLine("  --no-dt : Do not attempt to remove/add device tree overlays.");
                    Console.WriteLine("  --replace-dt : Remove all Scarlet DT overlays, then apply the new one. DANGEROUS!");
                    Console.WriteLine("  --add-dt : Add device tree overlay even if there is one already.");
                }
                if(Args[i] == "--no-dt") { ApplyDevTree = BBBPinManager.ApplicationMode.NO_CHANGES; }
                if(Args[i] == "--replace-dt") { ApplyDevTree = BBBPinManager.ApplicationMode.REMOVE_AND_APPLY; }
                if(Args[i] == "--add-dt") { ApplyDevTree = BBBPinManager.ApplicationMode.APPLY_REGARDLESS; }
            }
        }
	}
}
