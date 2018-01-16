
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
    public class BaseRotation
    {

        static bool ReceivingInput(GamePadState State)
        {
            return State.Triggers.Left <= Double.Epsilon && State.Triggers.Right <= Double.Epsilon;
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("Initializing");
            StateStore.Start("Evan and Jeremy");
            BeagleBone.Initialize(SystemMode.DEFAULT, true);

            //Add more mapping for more motor controllers
            BBBPinManager.AddMappingPWM(BBBPin.P9_14);
            BBBPinManager.AddMappingPWM(BBBPin.P9_16);
            BBBPinManager.AddMappingGPIO(BBBPin.P9_15, true, ResistorState.NONE, true);
            BBBPinManager.AddMappingGPIO(BBBPin.P9_27, true, ResistorState.NONE, true);

            BBBPinManager.ApplyPinSettings(BBBPinManager.ApplicationMode.APPLY_IF_NONE);

            //creates output for the two motors for the flat boi
            IDigitalOut Motor1Output = new DigitalOutBBB(BBBPin.P9_15);
            IDigitalOut Motor2Output = new DigitalOutBBB(BBBPin.P9_27);

            //if there are 2 motor controllers
            IPWMOutput OutA = PWMBBB.PWMDevice1.OutputA;
            IPWMOutput OutB = PWMBBB.PWMDevice1.OutputB;
            // IPWMOutput OutC = PWMBBB.PWMDevice1.OutputA;
            // IPWMOutput OutD = PWMBBB.PWMDevice1.OutputA;

            //Sets the Direction Outputs in the cytron to false (don't know what they do)
            Motor1Output.SetOutput(false);
            Motor2Output.SetOutput(false);

            PWMBBB.PWMDevice1.SetFrequency(10000); //5000 for talon motor, 10000 for cytron PWM 
            //OutA.SetFrequency(5000); (Frequency is not set at the per-output level on BBB, but by device, 
            //                           which they are doing correctly in the line right above.)
            //OutA.SetOutput(0.45F);
            OutA.SetEnabled(true);
            //OutB.SetOutput(0.45F);
            OutB.SetEnabled(true);
            CytronMD30C[] Motor = new CytronMD30C[2];

            //if there are 4 motor controllers
            Motor[0] = new CytronMD30C(OutA, (float).5);
            Motor[1] = new CytronMD30C(OutB, (float).5);
            // Motor[2] = new TalonMC(OutC, (float).5);
            // Motor[3] = new TalonMC(OutD, (float).5);

            GamePadState State;
            do
            {
                Console.WriteLine("Reading!");
                State = GamePad.GetState(0);
                if (State.IsConnected)
                {
                    Console.WriteLine($"Left: {State.ThumbSticks.Left.Y}, Right: {State.ThumbSticks.Right.Y}");
                }
                else
                {
                    Console.WriteLine("NOT CONNECTED");
                }
                if (State.ThumbSticks.Left.Y <= .05 || State.ThumbSticks.Left.Y >= -.05)
                {
                    Motor[0].SetSpeed(0.0);
                }
                else
                {
                    Motor[0].SetSpeed((float)((State.ThumbSticks.Left.Y) * 1.0f));
                }
                if (State.ThumbSticks.Right.Y <= .05 || State.ThumbSticks.Right.Y >= -.05)
                {
                    Motor[1].SetSpeed(0.0);
                }
                else
                {
                    Motor[1].SetSpeed((float)((State.ThumbSticks.Right.Y) * 1.0f));
                }
            } while (State.Buttons.Start != ButtonState.Pressed);

        }
    }
}

