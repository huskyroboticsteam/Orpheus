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
    class AuxSensors : ISubsystem
    {
        private bool TakeReadings = false;
        private Timer Timer;

        // Comms buses
        private ISPIBus SPI0;
        private II2CBus I2C1;

        // Intermediate devices
        private TLV2544ID ADC;

        // Sensor endpoints
        private MAX31855 Thermocouple;
        private VEML6070 UVLight;
        //private BME280 Atmospheric;
        //private MQ135 AirQuality;
        //private VH400 SoilMoisture;
        private IAnalogueIn AIn0, AIn1;

        public AuxSensors()
        {
            this.Timer = new Timer(this.UpdateState, null, 0, 250);
        }

        public void EmergencyStop() { this.TakeReadings = false; }

        public void Initialize()
        {
            this.SPI0 = new SPIBusPi(0);
            this.I2C1 = new I2CBusPi();

            //TLV2544ID.Configuration Config = TLV2544ID.DefaultConfiguration;
            //Config.InternalReferenceMode = true;

            this.ADC = new TLV2544ID(this.SPI0, new DigitalOutPi(16));//, Config);

            this.Thermocouple = new MAX31855(this.SPI0, new DigitalOutPi(18));
            this.UVLight = new VEML6070(this.I2C1);
            //this.Atmospheric = new BME280(this.I2C1);
            //this.Atmospheric.Configure();
            //this.Atmospheric.ChangeMode(BME280.Mode.NORMAL);
            //this.AirQuality = new MQ135(this.ADC.GetInputs[0]);
            //this.SoilMoisture = new VH400(this.ADC.GetInputs[1]);
            this.AIn0 = this.ADC.Inputs[0];
            this.AIn1 = this.ADC.Inputs[1];

            this.TakeReadings = true;
        }

        public void UpdateState(object TimerCallback) => this.UpdateState();

        public void UpdateState()
        {
            if(this.TakeReadings)
            {
                DateTime Sample = DateTime.Now;
                //this.Thermocouple.UpdateState();
                //this.UVLight.UpdateState();
                //this.Atmospheric.UpdateState();
                ((TLV2544ID.TLV2544IDInput)this.AIn0).UpdateState();
                ((TLV2544ID.TLV2544IDInput)this.AIn1).UpdateState();
                //Log.Output(Log.Severity.INFO, Log.Source.SENSORS, "Temp: " + this.Atmospheric.Temperature + ", press: " + this.Atmospheric.Pressure + ", humid: " + this.Atmospheric.Humidity + ", on: " + this.Atmospheric.Test());
                byte[] Data = UtilData.ToBytes(this.UVLight.GetReading())
                    .Concat(UtilData.ToBytes(this.Thermocouple.GetRawData()))
                    .Concat(UtilData.ToBytes(this.AIn0.GetInput()))
                    .Concat(UtilData.ToBytes(this.AIn1.GetInput())).ToArray();
                Packet Packet = new Packet(new Message(ScienceConstants.Packets.GND_SENSOR, Data), false);
                Client.Send(Packet);
            }
        }
    }
}
