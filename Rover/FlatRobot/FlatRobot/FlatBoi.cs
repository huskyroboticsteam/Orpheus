
using System;
using System.Threading;
using Scarlet;
using Scarlet.Utilities;
using Scarlet.Communications;
using Scarlet.Controllers;
using Scarlet.IO;
using Scarlet.IO.BeagleBone;
using OpenTK.Input;
using Scarlet.Components.Motors;


namespace FlatRobot
{
    public class FlatBoi
    {

        static bool ReceivingInput(GamePadState State)
        {
            return State.Triggers.Left <= Double.Epsilon && State.Triggers.Right <= Double.Epsilon;
        }

        public static void main(string[] args)
        {
            Console.WriteLine("Initializing");
            StateStore.Start("Evan and Jeremy");
            BeagleBone.Initialize(SystemMode.DEFAULT, true);

            //Add more mapping for more motor controllers
            BBBPinManager.AddMappingPWM(BBBPin.P9_14);
            BBBPinManager.ApplyPinSettings(BBBPinManager.ApplicationMode.APPLY_IF_NONE);
            // IDigitalOut Output = new DigitalOutBBB(BBBPin.P9_14);

            //if there are 4 motor controllers
            IPWMOutput OutA = PWMBBB.PWMDevice1.OutputA;
            IPWMOutput OutB = PWMBBB.PWMDevice1.OutputA;
            //IPWMOutput OutC = PWMBBB.PWMDevice1.OutputA;
            //IPWMOutput OutD = PWMBBB.PWMDevice1.OutputA;

            PWMBBB.PWMDevice1.SetFrequency(5000);
            //OutA.SetFrequency(5000); (Frequency is not set at the per-output level on BBB, but by device, 
            //                           which they are doing correctly in the line right above.)
            OutA.SetOutput(0.45F);
            OutA.SetEnabled(true);
            TalonMC[] Motor = new TalonMC[2];

            //if there are 4 motor controllers
            Motor[0] = new TalonMC(OutA, (float).5);
            Motor[1] = new TalonMC(OutB, (float).5);
            // Motor[2] = new TalonMC(OutC, (float).5);
            // Motor[3] = new TalonMC(OutD, (float).5);

            GamePadState State;
            do
            {
                Console.WriteLine("Reading!");
                State = GamePad.GetState(0);
                if (State.IsConnected)
                {
                    Console.WriteLine($"Left: {State.Triggers.Left}, Right: {State.Triggers.Right}");
                    Console.WriteLine($"Left Joystick X: {State.ThumbSticks.Left.X}, Y: {State.ThumbSticks.Left.Y}, Length: {State.ThumbSticks.Left.Length}");
                }
                else
                {
                    Console.WriteLine("NOT CONNECTED");
                }

            } while (State.Buttons.Start != ButtonState.Pressed);

        }
    }
}

