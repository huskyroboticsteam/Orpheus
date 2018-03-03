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
using System.Threading.Tasks;


namespace SensorDiagnostics
{
    class Test
    {
        
        public static void Main(string[] args)
        {
            BBBPinManager.AddMappingUART(BBBPin.P9_24);
            BBBPinManager.AddMappingUART(BBBPin.P9_26);
            BBBPinManager.AddMappingsI2C(BBBPin.P9_21, BBBPin.P9_22);
            //BBBPinManager.AddMappingGPIO(BBBPin.P9_12,false);
            IUARTBus UARTBus = UARTBBB.UARTBus1;
            I2CBusBBB I2C1 = I2CBBB.I2CBus1;
            I2CBusBBB I2C2 = I2CBBB.I2CBus2;
            //LimitSwitch Lswitch = new LimitSwitch();
            BNO055 magnetomiter = new BNO055(I2C2);
            MTK3339 gps = new MTK3339(UARTBus);
            //Globals.sensors.Add(Lswitch);
            magnetomiter.SetMode(BNO055.OperationMode.OPERATION_MODE_MAGONLY);
            Globals.sensors.Add(magnetomiter);
            Globals.sensors.Add(gps);
            for (int i = 0; i < 20; i++) {
                LogSensors.logSensors();
            }
        }
    }
}
