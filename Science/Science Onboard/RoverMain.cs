using System;
using System.IO;
using System.Threading;
using Scarlet.Communications;
using Scarlet.Utilities;
using Science.Library;
using Science.Systems;

namespace Science
{
	class RoverMain
	{
        public static IOHandler IOHandler { get; private set; }
        private static string IP = ScienceConstants.DEFAULT_SERVER_IP;
        private static int PortTCP = ScienceConstants.DEFAULT_PORT_TCP;
        private static int PortUDP = ScienceConstants.DEFAULT_PORT_UDP;

        private static Log.Severity LogLevel = Log.Severity.INFO;

        static void Main(string[] Args)
		{
            ParseArgs(Args);
            StateStore.Start(ScienceConstants.CLIENT_NAME);
            Log.SetGlobalOutputLevel(LogLevel);
            //Log.SetSingleOutputLevel(Log.Source.NETWORK, Log.Severity.DEBUG);
            Log.ErrorCodes = ScienceErrors.ERROR_CODES;
            Log.SystemNames = ScienceErrors.SYSTEMS;
            Log.Destination = Log.WriteDestination.ALL;
            Log.Begin();
            Log.ForceOutput(Log.Severity.INFO, Log.Source.OTHER, "Science Station - Rover Side");
            Client.Start(IP, PortTCP, PortUDP, ScienceConstants.CLIENT_NAME);
            PacketHandler Handler = new PacketHandler();
            //BeagleBone.Initialize(SystemMode.DEFAULT, true);

            IOHandler = new IOHandler();
            IOHandler.InitializeSystems();
            //((Turntable)IOHandler.TurntableController).TargetAngle = 50;

            while (Console.KeyAvailable) { Console.ReadKey(); } // Clear previous keypresses
            Log.ForceOutput(Log.Severity.INFO, Log.Source.OTHER, "Press any key to exit.");

            while (!Console.KeyAvailable)
            {
                IOHandler.UpdateStates();
                Thread.Sleep(100);
            }
            Environment.Exit(0);
		}

        private static void ParseArgs(string[] Args)
        {
            if (Args == null || Args.Length == 0) { return; } // Nothing to parse.
            for(int i = 0; i < Args.Length; i++)
            {
                if (Args.Length > i + 2) // Triple-part arguments
                {
                    if(Args[i] == "--music")
                    {
                        string MIDIFileName = Args[i + 1];
                        if (!int.TryParse(Args[i + 2], out int OctaveShift)) { Console.WriteLine("Given octave shift is invalid. Must be integer."); continue; }
                        if (!File.Exists(MIDIFileName)) { Console.WriteLine("MIDI file not found."); continue; }
                        MusicPlayer.MIDIFileName = MIDIFileName;
                        MusicPlayer.OctaveShift = OctaveShift;
                        i += 2;
                    }
                }
                if (Args.Length > i + 1) // Dual-part arguments.
                {
                    if (Args[i] == "-s" || Args[i] == "--server") { IP = Args[i + 1]; i++; }
                    if (Args[i] == "-pt" || Args[i] == "--port-tcp") { PortTCP = int.Parse(Args[i + 1]); i++; }
                    if (Args[i] == "-pu" || Args[i] == "--port-udp") { PortUDP = int.Parse(Args[i + 1]); i++; }
                    if (Args[i] == "-l" || Args[i] == "--log")
                    {
                        if (Args[i + 1].Equals("DEBUG", StringComparison.OrdinalIgnoreCase)) { LogLevel = Log.Severity.DEBUG; i++; }
                        else if (Args[i + 1].Equals("INFO", StringComparison.OrdinalIgnoreCase)) { LogLevel = Log.Severity.INFO; i++; }
                        else if (Args[i + 1].Equals("WARNING", StringComparison.OrdinalIgnoreCase)) { LogLevel = Log.Severity.WARNING; i++; }
                        else { Console.WriteLine("Unknown log level specified. Use 'DEBUG', 'INFO', or 'WARNING'."); i++; }
                    }
                }
                // Single-part arguments
                if(Args[i] == "-?" || Args[i] == "/?" || Args[i] == "-h" || Args[i] == "/h" || Args[i] == "help" || Args[i] == "-help" || Args[i] == "--help")
                {
                    Console.WriteLine("Command-line paramters:");
                    Console.WriteLine("  -?|/?|-h|/h|help|-help|--help : Outputs this text.");
                    Console.WriteLine("  -l|--log <Level> : Sets the default log level to 'DEBUG', 'INFO', or 'WARNING'.");
                    Console.WriteLine(" Networking:");
                    Console.WriteLine("  -s|--server <IP> : Connects to the given server instead of the default.");
                    Console.WriteLine("  -pt|--port-tcp <Port> : Connects to the server via TCP using the given port instead of the default.");
                    Console.WriteLine("  -pu|--port-udp <Port> : Connects to the server via UDP using the given port instead of the default.");
                    Console.WriteLine(" Fun:");
                    Console.WriteLine("  --music <MIDI file> <Octave Shift> : Syntesizes a MIDI file using the drill motor.");
                    /*Console.WriteLine(" Device Tree (BBB):");
                    Console.WriteLine("  --no-dt : Do not attempt to remove/add device tree overlays.");
                    Console.WriteLine("  --replace-dt : Remove all Scarlet DT overlays, then apply the new one. DANGEROUS!");
                    Console.WriteLine("  --add-dt : Add device tree overlay even if there is one already.");*/
                }
                /*if(Args[i] == "--no-dt") { ApplyDevTree = BBBPinManager.ApplicationMode.NO_CHANGES; }
                if(Args[i] == "--replace-dt") { ApplyDevTree = BBBPinManager.ApplicationMode.REMOVE_AND_APPLY; }
                if(Args[i] == "--add-dt") { ApplyDevTree = BBBPinManager.ApplicationMode.APPLY_REGARDLESS; }*/
            }
        }
    }
}
