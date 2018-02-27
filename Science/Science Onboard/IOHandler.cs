using Scarlet.Components;
using Scarlet.Components.Outputs;
using Scarlet.IO;
using Scarlet.IO.BeagleBone;
using Scarlet.IO.RaspberryPi;
using Science.Systems;

namespace Science
{
    class IOHandler
    {
        public readonly Rail RailController;
        public readonly Drill DrillController;
        public readonly Sample SampleController;
        public readonly LEDs LEDController;
        public readonly AuxSensors AuxSensors;

        private II2CBus I2C;
        private PCA9685 PWMGenLowFreq, PWMGenHighFreq;

        public IOHandler()
        {
            RaspberryPi.Initialize();
            //this.I2C = new I2CBusPi();
            //this.PWMGenHighFreq = new PCA9685(this.I2C, 0x4C);
            //this.PWMGenLowFreq = new PCA9685(this.I2C, 0x4B);

            //this.RailController = new Rail(this.PWMGenHighFreq.Outputs[1], new DigitalInPi(11));
            //this.DrillController = new Drill(this.PWMGenHighFreq.Outputs[0], this.PWMGenLowFreq.Outputs[0]);
            //this.SampleController = new Sample(this.PWMGenLowFreq.Outputs[1]);
            //this.LEDController = new LEDs(this.PWMGenLowFreq.Outputs, this.PWMGenHighFreq.Outputs);
            this.AuxSensors = new AuxSensors();
        }

        /// <summary>
        /// Prepares all systems for use by zeroing them. This takes a while.
        /// </summary>
        public void InitializeSystems()
        {
            //this.RailController.Initialize();
            //this.DrillController.Initialize();
            //this.SampleController.Initialize();
            //this.LEDController.Initialize();
            this.AuxSensors.Initialize();
        }

        /// <summary>
        /// Immediately stops all systems.
        /// </summary>
        public void EmergencyStop()
        {
            //this.RailController.EmergencyStop();
            //this.DrillController.EmergencyStop();
            //this.SampleController.EmergencyStop();
            //this.LEDController.EmergencyStop();
            this.AuxSensors.EmergencyStop();
        }

        public void UpdateStates()
        {
            //this.RailController.UpdateState();
            //this.DrillController.UpdateState();
            //this.SampleController.UpdateState();
            //this.LEDController.UpdateState();
            this.AuxSensors.UpdateState();
        }
    }
}
