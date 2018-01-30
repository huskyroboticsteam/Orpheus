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
        static void Main(string[] args)
        {
            Log.SetGlobalOutputLevel(Log.Severity.INFO);
            Log.SetSingleOutputLevel(Log.Source.NETWORK, Log.Severity.DEBUG);
            Log.ErrorCodes = ScienceErrors.ERROR_CODES;
            Log.SystemNames = ScienceErrors.SYSTEMS;
            Log.Begin();
            Log.ForceOutput(Log.Severity.INFO, Log.Source.OTHER, "Science Station - Base Side");

            byte[] StrB = UtilData.ToBytes("Erza-{}'");
            byte[] Fail = new byte[] { 0x8C };
            bool Success = UtilData.TryToString(Fail, out string Str);
            if (Success)
            {
                Console.WriteLine(Str);
            }
            else
            {
                Console.WriteLine(UtilMain.BytesToNiceString(StrB, true));
            }

            MainWindow Main = new MainWindow();
            Application.EnableVisualStyles();
            Server.ClientConnectionChange += Main.UpdateClientList;
            Application.Run(Main);
        }
    }
}
