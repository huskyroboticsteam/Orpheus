using Scarlet;
using Scarlet.Components;
using Scarlet.Components.Sensors;
using Scarlet.IO;
using Scarlet.IO.BeagleBone;
using Scarlet.Communications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Scarlet.Utilities;
using System.Windows;
using HuskyRobotics.UI;

namespace HuskyRobotics
{
    class Test_client
    {

        public static void Main(string[] args)
        {
            BeagleBone.Initialize(SystemMode.DEFAULT, false);
            StateStore.Start("TemporaryStuff");
            BBBPinManager.AddMappingUART(BBBPin.P9_24);
            BBBPinManager.AddMappingUART(BBBPin.P9_26);
            BBBPinManager.AddMappingsI2C(BBBPin.P9_19, BBBPin.P9_20);
            BBBPinManager.AddMappingGPIO(BBBPin.P9_12, false, ResistorState.NONE);
            BBBPinManager.ApplyPinSettings(BBBPinManager.ApplicationMode.APPLY_IF_NONE);
            IUARTBus UARTBus = UARTBBB.UARTBus1;
            I2CBusBBB I2C2 = I2CBBB.I2CBus2;
            //LimitSwitch Lswitch = new LimitSwitch(new DigitalInBBB(BBBPin.P9_12));
            BNO055 magnetomiter = new BNO055(I2C2);
            magnetomiter.SetMode(BNO055.OperationMode.OPERATION_MODE_MAGONLY);
            magnetomiter.SetSystem("magsys");
            //MTK3339 gps = new MTK3339(UARTBus);
            //gps.System = ("gpssys");
            //Globals.sensors.Add(Lswitch);
            Globals.sensors.Add(magnetomiter);
            //Globals.sensors.Add(gps);
            Client.Start("192.168.0.1", 1025, 1026, "BBB");
            while (true) { 
                for (int i = 0; i < 2; i++) {
                    foreach (ISensor s in Globals.sensors)
                    {
                        s.UpdateState();
                    }
                    LogSensors.logSensors();
                }
                    Thread.Sleep(200);
            }
        }
    }
}
