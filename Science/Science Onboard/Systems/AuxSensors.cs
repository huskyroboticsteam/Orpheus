using System;
using System.Linq;
using System.Threading;
using Scarlet.Communications;
using Scarlet.Components;
using Scarlet.Components.Inputs;
using Scarlet.Components.Sensors;
using Scarlet.IO;
using Scarlet.IO.RaspberryPi;
using Scarlet.Utilities;
using Science.Library;

namespace Science.Systems
{
    public class AuxSensors : ISubsystem
    {
        public bool TraceLogging { get; set; }

        private bool TakeReadings = false;
        private readonly Timer Timer;

        // Comms buses
        private readonly ISPIBus SPI0;
        private readonly II2CBus I2C1;

        // Intermediate devices
        private TLV2544 ADC;

        // Sensor endpoints
        private MAX31855 Thermocouple;
        private VEML6070 UVLight;
        private BME280 Atmospheric;
        private MQ135 AirQuality;
        private VH400 SoilMoisture;

        public AuxSensors(ISPIBus SPI, II2CBus I2C)
        {
            this.Timer = new Timer(this.UpdateState, null, 0, 250);
            this.SPI0 = SPI;
            this.I2C1 = I2C;
        }

        public void EmergencyStop() { this.TakeReadings = false; }

        public void Initialize()
        {
            this.ADC = new TLV2544(this.SPI0, new DigitalOutPi(16));// { TraceLogging = true };
            TLV2544.Configuration Config = TLV2544.DefaultConfig;
            Config.VoltageRef = TLV2544.VoltageReference.INTERNAL_2V;
            Config.ConversionClockSrc = TLV2544.ConversionClockSrc.INTERNAL;
            Config.UseEOCPin = true;
            //Config.UseLongSample = true;
            this.ADC.Configure(Config);

            this.Thermocouple = new MAX31855(this.SPI0, new DigitalOutPi(18));
            this.UVLight = new VEML6070(this.I2C1);
            this.Atmospheric = new BME280(this.I2C1);
            this.Atmospheric.Configure();
            
            this.Atmospheric.ChangeMode(BME280.Mode.NORMAL);
            this.AirQuality = new MQ135(this.ADC.Inputs[0], 3300, 6400, 4.6);
            this.SoilMoisture = new VH400(this.ADC.Inputs[1]);

            this.TakeReadings = true;
        }

        public void UpdateState(object TimerCallback) => this.UpdateState();

        public void UpdateState()
        {
            if (this.TakeReadings)
            {
                DateTime Sample = DateTime.Now;
                this.Thermocouple.UpdateState();
                if (this.Thermocouple.GetFaults() != MAX31855.Fault.NONE) { Log.Output(Log.Severity.WARNING, Log.Source.SENSORS, "Thermocouple has faults: " + this.Thermocouple.GetFaults());}
                this.UVLight.UpdateState();
                this.Atmospheric.UpdateState();
                this.AirQuality.UpdateState();
                this.SoilMoisture.UpdateState();

                if (this.TraceLogging) { Log.Trace(this, "Thermocouple: int " + this.Thermocouple.GetInternalTemp() + " ext " + this.Thermocouple.GetExternalTemp() + " faults " + this.Thermocouple.GetFaults()); }

                //Log.Output(Log.Severity.INFO, Log.Source.SENSORS, "Temp: " + this.Atmospheric.Temperature + ", press: " + this.Atmospheric.Pressure + ", humid: " + this.Atmospheric.Humidity + ", on: " + this.Atmospheric.Test());
                byte[] Data = UtilData.ToBytes(this.UVLight.GetReading())
                    .Concat(UtilData.ToBytes(this.AirQuality.GetReadingUncalibrated()))
                    .Concat(UtilData.ToBytes(this.SoilMoisture.GetReading()))
                    .Concat(UtilData.ToBytes(this.Thermocouple.GetExternalTemp()))
                    .Concat(UtilData.ToBytes(this.Atmospheric.Temperature))
                    .Concat(UtilData.ToBytes(this.Atmospheric.Pressure))
                    .Concat(UtilData.ToBytes(this.Atmospheric.Humidity))
                    .ToArray();
                if (this.TraceLogging) { Log.Trace(this, "UV: " + this.UVLight.GetReading() + ", AirQ: " + this.AirQuality.GetReadingUncalibrated() + ", Soil: " + this.SoilMoisture.GetReading() + ", AirTemp: " + this.Atmospheric.Temperature); }
                
                Packet Packet = new Packet(new Message(ScienceConstants.Packets.AUX_SENSOR, Data), false);
                Client.Send(Packet);
            }
        }

        public void Exit()
        {

        }
    }
}
