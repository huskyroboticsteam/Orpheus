using System;
using System.Net;
using System.Threading;
using BBBCSIO;
using Scarlet.Communications;
using Scarlet.Components;
using Scarlet.Components.Sensors;
using Scarlet.IO;
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

            BeagleBone.Initialize(SystemMode.DEFAULT, true);
            Log.SetSingleOutputLevel(Log.Source.HARDWAREIO, Log.Severity.DEBUG);
            TestI2C();

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

        private static void TestDigO()
        {
            BBBPinManager.AddMappingGPIO(BBBPin.P8_08, true, Scarlet.IO.ResistorState.PULL_DOWN);
            BBBPinManager.ApplyPinSettings();
            IDigitalOut Output = new DigitalOutBBB(BBBPin.P8_08);
            Output.Initialize();
            bool Value = false;
            for (int i = 0; i < 50; i++)
            {
                Output.SetOutput(Value);
                Value = !Value;
                Thread.Sleep(100);
            }
            Output.SetOutput(false);
        }

        private static void TestDigI()
        {
            BBBPinManager.AddMappingGPIO(BBBPin.P9_12, false, Scarlet.IO.ResistorState.PULL_DOWN);
            //BBBPinManager.ApplyPinSettings();
            IDigitalIn Input = new DigitalInBBB(BBBPin.P9_12);
            Input.Initialize();
            for(int i = 0; i < 50; i++)
            {
                Log.Output(Log.Severity.DEBUG, Log.Source.HARDWAREIO, "Input is " + Input.GetInput());
                Thread.Sleep(250);
            }
        }

        private static void TestPWM()
        {
            BBBPinManager.AddMappingGPIO(BBBPin.P8_08, true, Scarlet.IO.ResistorState.PULL_DOWN); // TODO: Remove this dependency from DT
            BBBPinManager.AddMappingPWM(BBBPin.P9_14);
            BBBPinManager.ApplyPinSettings();
            IPWMOutput Output = PWMBBB.PWMDevice1.OutputA;
            Output.Initialize();
            PWMBBB.PWMDevice1.SetFrequency(5000);
            for(int i = 0; i < 100; i++)
            {
                Output.SetOutput(i / 100.000F);
                Thread.Sleep(50);
            }
            Output.SetOutput(0);
        }

        private static void TestPWMLow()
        {
            PWMPortMM Port = new PWMPortMM(PWMPortEnum.PWM1_A);
            Port.PeriodNS = 250000;
            Port.DutyPercent = 50;
            Port.RunState = true;
        }

        private static void TestI2C()
        {
            BBBPinManager.AddMappingGPIO(BBBPin.P8_08, true, Scarlet.IO.ResistorState.PULL_DOWN);
            BBBPinManager.AddMappingsI2C(BBBPin.P9_24, BBBPin.P9_26);
            BBBPinManager.ApplyPinSettings();
            VEML6070 UV = new VEML6070(I2CBBB.I2CBus1);
            UV.Initialize();
            Log.SetSingleOutputLevel(Log.Source.SENSORS, Log.Severity.DEBUG);
            for (int i = 0; i < 20; i++)
            {
                UV.UpdateState();
                Log.Output(Log.Severity.DEBUG, Log.Source.SENSORS, "UV Reading: " + UV.GetData());
                Thread.Sleep(200);
            }
        }

        private static void TestSPI()
        {
            BBBPinManager.AddMappingsSPI(BBBPin.P9_18, BBBPin.NONE, BBBPin.P9_22);
            BBBPinManager.AddMappingSPI_CS(BBBPin.P9_12);
            BBBPinManager.ApplyPinSettings();
            // Do SPI stuff
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
