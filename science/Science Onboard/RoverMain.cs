using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RoboticsLibrary.Utilities;

namespace Science_Onboard
{
	class RoverMain
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello. This is science on the rover. Check back later!");
            Log.OutputLevel = 0;
            Log.OutputType = Log.Source.ALL;
            RoboticsLibrary.Communications.Packet Pack = new RoboticsLibrary.Communications.Packet(0x00);
            Pack.AppendData(new byte[] {0x00, 0x58, 0x6E, 0xFF, 0xEE, 0xCC, 0xBB, 0xAA, 0x99, 0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11, 0x00, 0x00, 0x00});
            Log.Output(0, Log.Source.OTHER, Pack.ToString());
            Thread.Sleep(10000);
		}
	}
}
