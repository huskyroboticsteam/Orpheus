using System;
using System.Net;
using System.Windows.Forms;
using Scarlet.Utilities;
using Scarlet.Communications;
using Science.Library;

namespace Science_Base
{
    class BaseMain
    {
        public static MainWindow Window;

        [STAThread]
        static void Main(string[] args)
        {
            Log.SetGlobalOutputLevel(Log.Severity.INFO);
            Log.SetSingleOutputLevel(Log.Source.NETWORK, Log.Severity.DEBUG);
            Log.ErrorCodes = ScienceErrors.ERROR_CODES;
            Log.SystemNames = ScienceErrors.SYSTEMS;
            Log.Begin();
            Log.ForceOutput(Log.Severity.INFO, Log.Source.OTHER, "Science Station - Base Side");

            Window = new MainWindow();
            Application.EnableVisualStyles();
            Server.Start(ScienceConstants.DEFAULT_PORT_TCP, ScienceConstants.DEFAULT_PORT_UDP);
            Server.ClientConnectionChange += Window.UpdateClientList;
            DataHandler.Start();
            Application.Run(Window);
        }
    }
}
