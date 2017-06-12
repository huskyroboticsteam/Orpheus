using System;
using System.Threading;
using RoboticsLibrary.Sensors;
using RoboticsLibrary.Utilities;

namespace Science_Onboard
{
	class RoverMain
	{
		static void Main(string[] args)
		{
            Log.OutputLevel = Log.Severity.DEBUG;
            Log.OutputType = Log.Source.ALL;
            Log.Begin();
            Log.ForceOutput(Log.Severity.INFO, Log.Source.OTHER, "Science Station - Rover Side");

            RunTests();

            while (Console.KeyAvailable) { Console.ReadKey(); }
            Log.ForceOutput(Log.Severity.INFO, Log.Source.OTHER, "Press any key to exit.");
            Console.ReadKey();
		}

        static void RunTests()
        {
            RoboticsLibrary.Communications.Packet Pack = new RoboticsLibrary.Communications.Packet(0x00);
            Pack.AppendData(new byte[] { 0x00, 0x58, 0x6E, 0xFF, 0xEE, 0xCC, 0xBB, 0xAA, 0x99, 0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11, 0x00, 0x00, 0x00 });
            Log.Output(Log.Severity.DEBUG, Log.Source.OTHER, Pack.ToString());

            LimitSwitch LimSw = new LimitSwitch(3, true);
            LimSw.SwitchToggle += LimitSwitchEvent;

            Encoder Enc = new Encoder(0, 0, 360);
            Enc.Turned += EncoderTurnedEvent;
            while (true)
            {
                LimSw.UpdateState();
                Enc.UpdateState();
                Thread.Sleep(1000);
            }
        }

        static void LimitSwitchEvent(object Sender, LimitSwitchToggle Event)
        {
            Log.Output(Log.Severity.DEBUG, Log.Source.SENSORS, "Limit switch toggle!");
        }

        static void EncoderTurnedEvent(object Sender, EncoderTurn Event)
        {
            Log.Output(Log.Severity.DEBUG, Log.Source.SENSORS, "Encoder turned by " + Event.TurnAmount);
        }
	}
}
