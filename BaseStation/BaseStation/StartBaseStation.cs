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
        private static readonly SharpDX.XInput.Controller DriveController = GamepadFactory.GetDriveGamePad();
        private static readonly SharpDX.XInput.Controller ArmController = GamepadFactory.GetArmGamepad();

        /// <summary>
        /// The entry point of the base station system. Starts the base station user interface
		/// and communication with the rover.
        /// </summary>
        /// <param name="args">command-line arguments</param>
		[STAThread]
        public static void Main(String[] args)
        {
            BaseServer.Setup();
            CameraControl.Setup();
            new Thread(StartUpdates).Start();
            Application app = new Application();
            app.Exit += (sd, ev) => StopUpdates();
			app.Run(new MainWindow());
        }

        private static void Update()
        {
            BaseServer.Update(DriveController, ArmController);
            CameraControl.Update(DriveController);
            Thread.Sleep(1);
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
            BaseServer.Shutdown();
        }
    }
}
