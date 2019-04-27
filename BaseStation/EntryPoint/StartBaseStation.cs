using System;
using HuskyRobotics.UI;
using System.Threading;
using System.Windows;
using HuskyRobotics.BaseStation.Server;

/// <summary>
/// Contains the entry point to the base station system.
/// </summary>
namespace HuskyRobotics.BaseStation.Start
{
    public static class StartBaseStation
    {
        /// <summary>
        /// The entry point of the base station system. Starts the base station user interface
		/// and communication with the rover.
        /// </summary>
        /// <param name="args">command-line arguments</param>
		[STAThread]
        public static void Main(String[] args)
        {
            PacketSender.Setup();
            CameraControl.Setup();
            new Thread(StartUpdates).Start();
            Application app = new Application();
            app.Exit += (sd, ev) => StopUpdates();
			app.Run(new MainWindow());
        }

        private static void Update()
        {
            PacketSender.Update();
            CameraControl.Update();
            Thread.Sleep(100);
        }

        private static volatile bool exit;

        public static void StartUpdates()
        {
            while(!exit)
            {
                Update();
            }
        }

        public static void StopUpdates()
        {
            exit = true;
            PacketSender.Shutdown();
        }
    }
}
