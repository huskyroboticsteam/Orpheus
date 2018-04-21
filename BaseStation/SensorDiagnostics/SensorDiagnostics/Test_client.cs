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
            for (int i = 0; i < 2; i++) {
                foreach (ISensor s in Globals.sensors)
                {
                    s.UpdateState();
                }
                LogSensors.logSensors();
            }
            DataLog test = new DataLog("test");
            DataUnit testUnit = new DataUnit("test")
            {
                { "a", 10},
                { "b", 20},
                { "c", 30}
            };
            testUnit.SetSystem("testsys");
            test.Output(testUnit);
            Random rnd = new Random();
            while (true)
            {
                testUnit = new DataUnit("test")
            {
                { "a", rnd.NextDouble() * 100},
                { "b", rnd.NextDouble() * 100},
                { "c", rnd.NextDouble() * 100}
            };
                Packet MyPack = new Packet(0x65,false);
                Type[] dicTypes = testUnit.Values.GetType().GetGenericArguments();
                Type valueType = dicTypes[1];
                MyPack.AppendData(UtilData.ToBytes(testUnit.System));
                MyPack.AppendData(UtilData.ToBytes("|"));
                foreach (string key in testUnit.Keys) {
                    MyPack.AppendData(UtilData.ToBytes(key));
                    MyPack.AppendData(UtilData.ToBytes("="));
                    MyPack.AppendData(UtilData.ToBytes(Convert.ToString(testUnit.GetValue<ValueType>(key))));
                    MyPack.AppendData(UtilData.ToBytes(","));
                    Console.Write(UtilData.ToString(MyPack.Data.Payload));
                }       
                Console.WriteLine(MyPack.Data);
                Client.Send(MyPack);
                Thread.Sleep(500);
            }
        }
    }
}
