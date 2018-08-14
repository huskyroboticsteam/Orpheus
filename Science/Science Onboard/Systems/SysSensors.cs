using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Scarlet.Communications;
using Scarlet.Components;
using Scarlet.Components.Outputs;
using Scarlet.Components.Sensors;
using Scarlet.IO;
using Scarlet.IO.RaspberryPi;
using Scarlet.Utilities;
using Science.Library;

namespace Science.Systems
{
    public class SysSensors : ISubsystem
    {
        public bool TraceLogging { get; set; }
        private bool TakeReadings = false;
        private Timer Timer;

        // Comms buses
        private II2CBus I2C1;

        // Sensor endpoints
        private INA226 SystemSensor, DrillSensor, RailSensor;

        public SysSensors()
        {
            this.Timer = new Timer(this.UpdateState, null, 0, 100);
        }

        public void EmergencyStop() { this.TakeReadings = false; }

        public void Initialize()
        {
            this.I2C1 = new I2CBusPi();

            this.SystemSensor = new INA226(this.I2C1, 0x48, 10, 0.150);
            this.DrillSensor = new INA226(this.I2C1, 0x49, 15, 0.010);
            this.RailSensor = new INA226(this.I2C1, 0x4A, 60, 0.002);

            this.TakeReadings = true;
        }

        public void UpdateState(object TimerCallback) => this.UpdateState();

        public void UpdateState()
        {
            if (this.TakeReadings)
            {
                DateTime Sample = DateTime.Now;
                this.SystemSensor.UpdateState();
                this.DrillSensor.UpdateState();
                this.RailSensor.UpdateState();

                double RailA = this.RailSensor.GetShuntVoltage();
                double Drill = this.DrillSensor.GetCurrent();
                double SysA = this.SystemSensor.GetCurrent();
                double SysV = this.SystemSensor.GetBusVoltage();
                double SysSV = this.SystemSensor.GetShuntVoltage();
                double DrlSV = this.DrillSensor.GetShuntVoltage();

                if (this.TraceLogging) { Log.Trace(this, string.Format("Sys: {0:N5}A, {1:N5}V (Shunt {2:N5}V). Drill: {3:N5}A. Rail: {4:N5}A. Test: {5}", SysA, SysV, SysSV, Drill, RailA, this.SystemSensor.Test()));  }

                byte[] Data = UtilData.ToBytes(SysSV / 0.150).Concat(UtilData.ToBytes(DrlSV / 0.010)).Concat(UtilData.ToBytes(RailA / 0.002)).Concat(UtilData.ToBytes(SysV)).Concat(UtilData.ToBytes(Sample.Ticks)).ToArray(); // TODO: Decide if we want to calculate or use device
                Packet Packet = new Packet(new Message(ScienceConstants.Packets.SYS_SENSOR, Data), false);
                Client.Send(Packet);

                uint SysVoltColour;
                if (SysV <= 25) { SysVoltColour = RGBLED.RedGreenGradient(SysV, 22, 26); }
                else if (SysV <= 28) { SysVoltColour = 0x00FF00; }
                else { SysVoltColour = RGBLED.RedGreenGradient(SysV, 30, 28); }
                RoverMain.IOHandler.LEDController.SystemVoltage.SetOutput(SysVoltColour);

                uint SysCurrentColour;
                if (SysA <= 2) { SysCurrentColour = 0x00FF00; }
                else if (SysA >= 5) { SysCurrentColour = 0xFF0000; }
                else { SysCurrentColour = RGBLED.RedGreenGradient(SysA, 5, 2); }
                RoverMain.IOHandler.LEDController.SystemCurrent.SetOutput(SysCurrentColour);
            }
        }

        public void Exit()
        {
            this.TakeReadings = false;
        }
    }
}
