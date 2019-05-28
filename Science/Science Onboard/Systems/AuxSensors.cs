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
        //private VEML6075 UVLight;
        private BME280 Atmospheric;
        private iAQCore AirQuality;
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

            //this.UVLight = new VEML6075(this.I2C1) { TraceLogging = false };
            this.Atmospheric = new BME280(this.I2C1);
            this.Atmospheric.Configure();
            
            this.Atmospheric.ChangeMode(BME280.Mode.NORMAL);
            this.AirQuality = new iAQCore(this.I2C1) { TraceLogging = false };
            //this.AirQuality.UpdateState();
            this.SoilMoisture = new VH400(this.ADC.Inputs[1]);

            this.TakeReadings = true;
        }

        public void UpdateState(object TimerCallback) => this.UpdateState();

        public void UpdateState()
        {
            if (this.TakeReadings)
            {
                DateTime SampleTime = DateTime.Now;
                //this.UVLight.UpdateState();
                this.Atmospheric.UpdateState();
                this.AirQuality.UpdateState();
                this.SoilMoisture.UpdateState();
                
                byte[] Data = UtilData.ToBytes(SampleTime.Ticks)
                    .Concat(UtilData.ToBytes((float)0))//this.UVLight.GetApproximateUVIndex()))
                    .Concat(UtilData.ToBytes((ushort)this.AirQuality.GetCO2Value()))
                    .Concat(UtilData.ToBytes((ushort)this.AirQuality.GetTVOCValue()))
                    .Concat(UtilData.ToBytes((float)this.SoilMoisture.GetReading()))
                    .Concat(UtilData.ToBytes((float)this.Atmospheric.Temperature))
                    .Concat(UtilData.ToBytes((float)this.Atmospheric.Pressure))
                    .Concat(UtilData.ToBytes((float)this.Atmospheric.Humidity))
                    .ToArray();
                if (this.TraceLogging) { Log.Trace(this, /*"UV: [A " + this.UVLight.GetReadingUVA() + ", B " + this.UVLight.GetReadingUVB() + ", I " + this.UVLight.GetApproximateUVIndex().ToString("N4") + */"], AirQ: [CO2 " + this.AirQuality.GetCO2Value() + ",TVOC " + this.AirQuality.GetTVOCValue() + "], Soil: " + this.SoilMoisture.GetReading() + ", AirTemp: " + this.Atmospheric.Temperature); }
                
                Packet Packet = new Packet(new Message(ScienceConstants.Packets.AUX_SENSOR, Data), false);
                Client.Send(Packet);
            }
        }

        public void Exit()
        {

        }
    }
}
