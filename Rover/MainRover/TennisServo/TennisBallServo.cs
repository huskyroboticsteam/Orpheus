using System;
using Scarlet.IO;
using Scarlet.Utilities;
using Scarlet.IO.BeagleBone;
using Scarlet.Components;

namespace MainRover
{
    class TennisBallServo
    {
        static void Main(String[] args)
        {
            StateStore.Start("Start code");
            BeagleBone.Initialize(SystemMode.DEFAULT, true);

            BBBPinManager.AddMappingPWM(BBBPin.P9_14);

            BBBPinManager.ApplyPinSettings(BBBPinManager.ApplicationMode.APPLY_IF_NONE);

            IPWMOutput OutA = PWMBBB.PWMDevice1.OutputA;

            OutA.SetFrequency(50);

            OutA.SetOutput(0f);
            OutA.SetEnabled(true);

            float t = 0.1f;
            int count = 0;

            while (count < 500)
            {
                while (t < .9f)
                {
                    OutA.SetOutput(t);
                    t += 0.0001f;
                }
                while (t > .1f)
                {
                    OutA.SetOutput(t);
                    t -= 0.0001f;
                }
                count++;
            }
            OutA.Dispose();
        }
    }
}