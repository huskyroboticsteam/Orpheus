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
            Log.OutputLevel = 2;
            Log.OutputType = Log.Source.ALL;
            RoboticsLibrary.Communications.Packet Pack = new RoboticsLibrary.Communications.Packet(0x00);
            Pack.AppendData(new byte[] {0x00, 0x58, 0x6E, 0xFF});
            Log.Output(0, Log.Source.OTHER, Pack.ToString());
            Thread.Sleep(10000);
		}
	}
}
