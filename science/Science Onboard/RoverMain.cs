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
            Log.Output(3, Log.Source.MOTORS, "EEK THINGS ARE BROKEN");
            Thread.Sleep(1000);
		}
	}
}
