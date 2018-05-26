using Scarlet.IO;
using Scarlet.IO.BeagleBone;
using Scarlet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LimitTester
{
    class Program
    {
        static void Main(string[] args)
        {

            StateStore.Start("LimitSwitch");
            BeagleBone.Initialize(SystemMode.NO_HDMI, true);

            BBBPinManager.AddMappingGPIO(BBBPin.P9_29, true, Scarlet.IO.ResistorState.PULL_UP);
            BBBPinManager.ApplyPinSettings(BBBPinManager.ApplicationMode.APPLY_IF_NONE);

            //TODO Use Scarlet/Components/Sensors/LimitSwitch.cs 

            IDigitalIn theInput = new DigitalInBBB(BBBPin.P9_29);

            while (true)
            {
                Console.Write("Turn Value: ");
                Console.WriteLine(theInput.GetInput());
                Thread.Sleep(50);
            }


        }
    }
}
