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
        private readonly Timer Timer;

        // Comms buses
        private readonly II2CBus I2C1;
        private readonly ISPIBus SPI;

        // Sensor endpoints
        private INA226 SysCurrent, DrillCurrent, RailCurrent, TurntableCurrent, SpareMotorCurrent;

        public SysSensors(II2CBus I2C, ISPIBus SPI)
        {
            this.SPI = SPI;
            this.I2C1 = I2C;
            this.Timer = new Timer(this.UpdateState, null, 0, 100);
        }

        public void EmergencyStop() { this.TakeReadings = false; }

        public void Initialize()
        {
            this.DrillCurrent = new INA226(this.I2C1, 0x48, 15, 0.005);
            this.RailCurrent = new INA226(this.I2C1, 0x49, 30, 0.005);
            this.TurntableCurrent = new INA226(this.I2C1, 0x4A, 15, 0.005);
            this.SpareMotorCurrent = new INA226(this.I2C1, 0x4B, 15, 0.005);
            this.SysCurrent = new INA226(this.I2C1, 0x4C, 5, 0.100);

            this.TakeReadings = true;
        }

        public void UpdateState(object TimerCallback) => this.UpdateState();

        public void UpdateState()
        {
            if (this.TakeReadings)
            {
                DateTime Sample = DateTime.Now;
                this.SysCurrent.UpdateState();
                this.DrillCurrent.UpdateState();
                this.RailCurrent.UpdateState();
                this.TurntableCurrent.UpdateState();
                this.SpareMotorCurrent.UpdateState();

                double SysA = this.SysCurrent.GetCurrent();
                double SysV = this.SysCurrent.GetBusVoltage();
                double SysSV = this.SysCurrent.GetShuntVoltage();

                double DrillA = this.DrillCurrent.GetCurrent();

                double RailA = this.RailCurrent.GetShuntVoltage();

                double TurntableA = this.DrillCurrent.GetCurrent();

                double SpareMotorA = this.SpareMotorCurrent.GetCurrent();

                if (this.TraceLogging) { Log.Trace(this, string.Format("Sys: {0:N5}A, {1:N5}V (Shunt {2:N5}V).\nDrill: {3:N5}A. Rail: {4:N5}A. TTB: {5:N5}A. Spare: {6:N5}A.", SysA, SysV, SysSV, DrillA, RailA, TurntableA, SpareMotorA));  }

                byte[] Data = UtilData.ToBytes(Sample.Ticks)
                    .Concat(UtilData.ToBytes((float)SysV))
                    .Concat(UtilData.ToBytes((float)SysA))
                    .Concat(UtilData.ToBytes((float)DrillA))
                    .Concat(UtilData.ToBytes((float)RailA))
                    .Concat(UtilData.ToBytes((float)TurntableA))
                    .Concat(UtilData.ToBytes((float)SpareMotorA)).ToArray();
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

        public void Exit() { this.TakeReadings = false; }
    }
}
