using System;
using System.Net;
using System.Threading;
using BBBCSIO;
using Scarlet.Communications;
using Scarlet.Components;
using Scarlet.Components.Motors;
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
        private static BBBPinManager.ApplicationMode ApplyDevTree = BBBPinManager.ApplicationMode.APPLY_IF_NONE;
        private static IDigitalIn IntTestIn;

        static void Main(string[] Args)
		{
            ParseArgs(Args);
            StateStore.Start("SciRover");
            Log.SetGlobalOutputLevel(Log.Severity.INFO);
            Log.SetSingleOutputLevel(Log.Source.NETWORK, Log.Severity.DEBUG);
            Log.ErrorCodes = ScienceErrors.ERROR_CODES;
            Log.SystemNames = ScienceErrors.SYSTEMS;
            Log.Begin();
            Log.ForceOutput(Log.Severity.INFO, Log.Source.OTHER, "Science Station - Rover Side");

            BeagleBone.Initialize(SystemMode.DEFAULT, true);
            Log.SetSingleOutputLevel(Log.Source.HARDWAREIO, Log.Severity.DEBUG);
            TestPWM();

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
            BBBPinManager.ApplyPinSettings(ApplyDevTree);
            IDigitalOut Output = new DigitalOutBBB(BBBPin.P8_08);
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
            //if (ApplyDevTree) { BBBPinManager.ApplyPinSettings(); }
            IDigitalIn Input = new DigitalInBBB(BBBPin.P9_12);
            for(int i = 0; i < 50; i++)
            {
                Log.Output(Log.Severity.DEBUG, Log.Source.HARDWAREIO, "Input is " + Input.GetInput());
                Thread.Sleep(250);
            }
        }

        private static void TestPWM()
        {
            BBBPinManager.AddMappingPWM(BBBPin.P9_14);
            BBBPinManager.AddMappingPWM(BBBPin.P9_16);
            BBBPinManager.ApplyPinSettings(ApplyDevTree);
            IPWMOutput OutA = PWMBBB.PWMDevice1.OutputA;
            IPWMOutput OutB = PWMBBB.PWMDevice1.OutputB;
            PWMBBB.PWMDevice1.SetFrequency(5000);
            OutA.SetEnabled(true);
            OutB.SetEnabled(true);
            int Cycle = 0;
            while (true)
            {
                float A = (float)((Math.Sin(Cycle * Math.PI / 180.000D) + 1) / 2); // Sine waves! Fun!
                float B = (float)((Math.Sin(Cycle * Math.PI / 360.000D) + 1) / 2);

                OutA.SetOutput(A);
                OutB.SetOutput(B);

                Thread.Sleep(50);
                Cycle += 20;
            }
        }

        private static void TestMotor()
        {
            BBBPinManager.AddMappingPWM(BBBPin.P9_14);
            BBBPinManager.ApplyPinSettings(ApplyDevTree);
            IPWMOutput MotorOut = PWMBBB.PWMDevice1.OutputA;
            MotorOut.SetEnabled(true);
            TalonMC Motor = new TalonMC(MotorOut, 0.2F);
            Log.SetSingleOutputLevel(Log.Source.MOTORS, Log.Severity.DEBUG);
            Motor.RampUp = 0.5F;
            //Motor.Speed = 0.2F;
            Motor.UpdateState();
            while (true)
            {
                Log.Output(Log.Severity.DEBUG, Log.Source.MOTORS, "Outputs: " + Motor.Speed + ", " + ((PWMOutputBBB)MotorOut).GetOutput() + ", " + ((PWMOutputBBB)MotorOut).GetFrequency());
                //Motor.UpdateState();
                Thread.Sleep(100);
            }
            /*int Cycle = 0;
            while (true)
            {
                Motor.UpdateState();
                float Spd = (float)((Math.Sin(Cycle * Math.PI / 360.000D) + 1) / 40) + 0.1F;
                Log.Output(Log.Severity.DEBUG, Log.Source.MOTORS, "Outputting " + Spd + ", currently " + Motor.Speed);
                Motor.Speed = Spd;
                Thread.Sleep(100);
                Cycle += 15;
            }*/
        }

        private static void TestPWMLow()
        {
            BBBPinManager.AddMappingPWM(BBBPin.P9_14);
            BBBPinManager.ApplyPinSettings(ApplyDevTree);

            PWMPortMM Port = new PWMPortMM(PWMPortEnum.PWM1_A);
            Port.PeriodNS = 1000000;
            Port.DutyPercent = 0;
            Port.RunState = true;
            while (true)
            {
                for (int i = 0; i < 100; i++)
                {
                    Port.DutyPercent = i;
                    Thread.Sleep(10);
                }
                Port.DutyPercent = 100;
                Thread.Sleep(50);
                for (int i = 100; i > 0; i--)
                {
                    Port.DutyPercent = i;
                    Thread.Sleep(10);
                }
                Port.DutyPercent = 0;
                Thread.Sleep(50);
            }
        }
        
        private static void TestI2C()
        {
            BBBPinManager.AddMappingGPIO(BBBPin.P8_08, true, Scarlet.IO.ResistorState.PULL_DOWN);
            BBBPinManager.AddMappingsI2C(BBBPin.P9_24, BBBPin.P9_26);
            BBBPinManager.ApplyPinSettings(ApplyDevTree);
            VEML6070 UV = new VEML6070(I2CBBB.I2CBus1);
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
            BBBPinManager.AddMappingsSPI(BBBPin.P9_21, BBBPin.NONE, BBBPin.P9_22);
            BBBPinManager.AddMappingSPI_CS(BBBPin.P9_12);
            BBBPinManager.ApplyPinSettings(ApplyDevTree);
            IDigitalOut CS_Thermo = new DigitalOutBBB(BBBPin.P9_12);
            MAX31855 Thermo = new MAX31855(SPIBBB.SPIBus0, CS_Thermo);
            Log.SetSingleOutputLevel(Log.Source.SENSORS, Log.Severity.DEBUG);
            for (int i = 0; i < 100; i++)
            {
                Thermo.UpdateState();
                Log.Output(Log.Severity.DEBUG, Log.Source.SENSORS, "Thermocouple Data, Faults: " + string.Format("{0:G}", Thermo.GetFaults()) + ", Internal: " + Thermo.GetInternalTemp() + ", External: " + Thermo.GetExternalTemp() + " (Raw: " + Thermo.GetRawData() + ")");
                Thread.Sleep(500);
            }
        }

        private static void TestADC()
        {
            BBBPinManager.AddMappingADC(BBBPin.P9_36);
            BBBPinManager.ApplyPinSettings(ApplyDevTree);
            IAnalogueIn Input = new AnalogueInBBB(BBBPin.P9_36);
            for(int i = 0; i < 200; i++)
            {
                Log.Output(Log.Severity.DEBUG, Log.Source.HARDWAREIO, "ADC Input: " + Input.GetInput() + " (Raw: " + Input.GetRawInput() + ")");
                Thread.Sleep(100);
            }
        }

        private static void TestInterrupt()
        {
            BBBPinManager.AddMappingGPIO(BBBPin.P9_12, true, Scarlet.IO.ResistorState.PULL_DOWN);
            BBBPinManager.ApplyPinSettings(ApplyDevTree);
            IntTestIn = new DigitalInBBB(BBBPin.P9_12);
            IntTestIn.RegisterInterruptHandler(GetInterrupt, InterruptType.ANY_EDGE);
            Log.Output(Log.Severity.DEBUG, Log.Source.HARDWAREIO, "Interrupt handler added.");
            while(true)
            {
                //Log.Output(Log.Severity.DEBUG, Log.Source.HARDWAREIO, "State: " + IntTestIn.GetInput());
                Thread.Sleep(100);
            }
        }

        public static void GetInterrupt(object Senser, InputInterrupt Event)
        {
            Log.Output(Log.Severity.DEBUG, Log.Source.HARDWAREIO, "Interrupt Received! Now " + Event.NewState);
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
                    Console.WriteLine("  --no-dt : Do not attempt to remove/add device tree overlays.");
                    Console.WriteLine("  --replace-dt : Remove all Scarlet DT overlays, then apply the new one. DANGEROUS!");
                    Console.WriteLine("  --add-dt : Add device tree overlay even if there is one already.");
                }
                if(Args[i] == "--no-dt")
                {
                    ApplyDevTree = BBBPinManager.ApplicationMode.NO_CHANGES;
                }
                if(Args[i] == "--replace-dt")
                {
                    ApplyDevTree = BBBPinManager.ApplicationMode.REMOVE_AND_APPLY;
                }
                if(Args[i] == "--add-dt")
                {
                    ApplyDevTree = BBBPinManager.ApplicationMode.APPLY_REGARDLESS;
                }
            }
        }
	}
}
