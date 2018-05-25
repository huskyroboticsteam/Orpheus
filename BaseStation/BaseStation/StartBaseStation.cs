using System;
using HuskyRobotics.UI;
using System.Threading;
using System.Windows;
using Scarlet.Utilities;

/// <summary>
/// Contains the entry point to the base station system.
/// </summary>
namespace HuskyRobotics.BaseStation.Start {
    public static class StartBaseStation {
        /// <summary>
        /// The entry point of the base station system. Starts the base station user interface
		/// and communication with the rover.
        /// </summary>
        /// <param name="args">command-line arguments</param>
		[STAThread]
        public static void Main(String[] args) {
            //temporary example code
            Server.BaseServer.Start();
            Thread eventloop = new Thread(Server.BaseServer.EventLoop);
            eventloop.IsBackground = true;
            eventloop.Start();
            Application app = new Application();
            app.Exit += (sd, ev) => Server.BaseServer.Shutdown();
			app.Run(new MainWindow());
        }
    }
}
