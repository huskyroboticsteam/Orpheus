using System;
using System.Threading;
using Scarlet.Components;
using Scarlet.Components.Sensors;
using Scarlet.IO;
using Scarlet.IO.RaspberryPi;

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
        //private TLV2544ID ADC;

        // Sensor endpoints
        private MAX31855 Thermocouple;
        private VEML6070 UVLight;
        //private BME280 Atmospheric;
        //private MQ135 AirQuality;
        //private VH400 SoilMoisture;

        public AuxSensors()
        {
            this.Timer = new Timer(this.UpdateState, null, 0, 250);
        }

        public void EmergencyStop() { this.TakeReadings = false; }

        public void EventTriggered(object Sender, EventArgs Event) { }

        public void Initialize()
        {
            this.SPI0 = new SPIBusPi(0);
            this.I2C1 = new I2CBusPi();

            //this.ADC = new TLV2544ID(SPI0, new DigitalOutPi(16));

            this.Thermocouple = new MAX31855(this.SPI0, new DigitalOutPi(18));
            this.UVLight = new VEML6070(this.I2C1);
            //this.AirQuality = new MQ135(this.ADC.GetInputs[0]);
            //this.SoilMoisture = new VH400(this.ADC.GetInputs[1]);

            this.TakeReadings = true;
        }

        public void UpdateState(object TimerCallback) => this.UpdateState();

        public void UpdateState()
        {
            if(this.TakeReadings)
            {
                DateTime Sample = DateTime.Now;
                
            }
        }
    }
}
