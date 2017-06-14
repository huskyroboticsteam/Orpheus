using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows.Forms;
using RoboticsLibrary.Utilities;
using RoboticsLibrary.Communications;

namespace Science_Base
{
	class BaseMain
	{
		static void Main(string[] args)
		{
            // Set Packet default endpoint (IP Address and port to send to by default)
            Packet.DefaultEndpoint = new IPEndPoint(IPAddress.Parse("10.1.10.140"), 600);
            CommHandler.Start(600); // Start Comms on listening port
            Log.OutputLevel = Log.Severity.DEBUG;
            Log.OutputType = Log.Source.ALL;
            Log.Begin();
            Log.ForceOutput(Log.Severity.INFO, Log.Source.OTHER, "Science Station - Base Side");

            MainWindow Main = new MainWindow();
            Application.EnableVisualStyles();
            Application.Run(Main);
		}
	}
}
