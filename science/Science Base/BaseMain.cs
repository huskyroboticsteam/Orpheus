using System;
using System.Net;
using System.Windows.Forms;
using Scarlet.Utilities;
using Scarlet.Communications;
using Scarlet.Science;

namespace Science_Base
{
	class BaseMain
	{
		static void Main(string[] args)
		{
            Log.OutputLevel = Log.Severity.DEBUG;
            Log.OutputType = Log.Source.ALL;
            Log.ErrorCodes = ScienceErrors.ERROR_CODES;
            Log.SystemNames = ScienceErrors.SYSTEMS;
            Log.Begin();
            Log.ForceOutput(Log.Severity.INFO, Log.Source.OTHER, "Science Station - Base Side");

            //CommHandler.Start(600, 610, "192.168.0.112");
            Server.Start(600, 610);

            MainWindow Main = new MainWindow();
            Application.EnableVisualStyles();
            Application.Run(Main);
		}
	}
}
