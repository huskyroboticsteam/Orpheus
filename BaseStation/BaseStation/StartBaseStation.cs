using System;
using HuskyRobotics.UI;
using HuskyRobotics.BaseStation;
using System.Threading;
using System.Windows;
using Scarlet.Utilities;
using SharpDX.XInput;

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
            //temporary example code
            Server.BaseServer.Start(GamepadFactory.GetDriveGamePad(), GamepadFactory.GetArmGamepad());
            Thread eventloop = new Thread(Server.BaseServer.EventLoop);
            eventloop.IsBackground = true;
            eventloop.Start();
            Thread ipCameraControl = new Thread(() => CameraControl.Start(GamepadFactory.GetDriveGamePad()));
            ipCameraControl.IsBackground = true;
            ipCameraControl.Start();
            //Thread armControllerID = new Thread(() => VibrateArmController(GamepadFactory.GetArmGamepad()));
            //armControllerID.IsBackground = true;
            //armControllerID.Start();
            Application app = new Application();
            app.Exit += (sd, ev) => Server.BaseServer.Shutdown();
			app.Run(new MainWindow());
        }

        //private static void VibrateArmController(Controller ArmController)
        //{
        //    Vibration vibration = new Vibration();
        //    vibration.LeftMotorSpeed = 255;
        //    ArmController.SetVibration(vibration);

        //    Thread.Sleep(5000);

        //    vibration.LeftMotorSpeed = 0;
        //    ArmController.SetVibration(vibration);
        //}
    }
}
