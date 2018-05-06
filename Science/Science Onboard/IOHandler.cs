using System;
using Scarlet.Components;
using Scarlet.Components.Outputs;
using Scarlet.IO;
using Scarlet.IO.BeagleBone;
using Scarlet.IO.RaspberryPi;
using Scarlet.Utilities;
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
        public readonly SysSensors SysSensors;
        public readonly MusicPlayer Music;

        private II2CBus I2C;
        private PCA9685 PWMGenLowFreq, PWMGenHighFreq;

        public IOHandler()
        {
            RaspberryPi.Initialize();
            this.I2C = new I2CBusPi();
            this.PWMGenHighFreq = new PCA9685(this.I2C, 0x4C, -1, PCA9685.OutputInvert.Inverted, PCA9685.OutputDriverMode.OpenDrain);
            this.PWMGenLowFreq = new PCA9685(this.I2C, 0x74, -1, PCA9685.OutputInvert.Inverted, PCA9685.OutputDriverMode.OpenDrain);
            this.PWMGenHighFreq.SetFrequency(333);
            this.PWMGenLowFreq.SetFrequency(50);

            //this.RailController = new Rail(this.PWMGenHighFreq.Outputs[1], new DigitalInPi(11));
            this.DrillController = new Drill(this.PWMGenHighFreq.Outputs[0], this.PWMGenLowFreq.Outputs[0]);
            //this.SampleController = new Sample(this.PWMGenLowFreq.Outputs[1]);
            this.LEDController = new LEDs(this.PWMGenLowFreq.Outputs, this.PWMGenHighFreq.Outputs);
            this.AuxSensors = new AuxSensors();
            this.SysSensors = new SysSensors();
            this.Music = new MusicPlayer();
        }

        /// <summary>
        /// Prepares all systems for use by zeroing them. This takes a while.
        /// </summary>
        public void InitializeSystems()
        {
            //this.RailController.Initialize();
            this.DrillController.Initialize();
            //this.SampleController.Initialize();
            this.LEDController.Initialize();
            this.AuxSensors.Initialize();
            this.SysSensors.Initialize();
            this.Music.Initialize();
        }

        /// <summary>
        /// Immediately stops all systems.
        /// </summary>
        public void EmergencyStop()
        {
            try { this.Music.EmergencyStop(); }
            catch (Exception Exc) { Log.Output(Log.Severity.ERROR, Log.Source.SUBSYSTEM, "Music subsystem failed emergency stop!"); Log.Exception(Log.Source.SUBSYSTEM, Exc); }

            try { this.RailController.EmergencyStop(); }
            catch (Exception Exc) { Log.Output(Log.Severity.ERROR, Log.Source.SUBSYSTEM, "Rail subsystem failed emergency stop!"); Log.Exception(Log.Source.SUBSYSTEM, Exc); }

            try { this.DrillController.EmergencyStop(); }
            catch (Exception Exc) { Log.Output(Log.Severity.ERROR, Log.Source.SUBSYSTEM, "Drill subsystem failed emergency stop!"); Log.Exception(Log.Source.SUBSYSTEM, Exc); }

            try { this.SampleController.EmergencyStop(); }
            catch (Exception Exc) { Log.Output(Log.Severity.ERROR, Log.Source.SUBSYSTEM, "Sample subsystem failed emergency stop!"); Log.Exception(Log.Source.SUBSYSTEM, Exc); }

            try { this.LEDController.EmergencyStop(); }
            catch (Exception Exc) { Log.Output(Log.Severity.ERROR, Log.Source.SUBSYSTEM, "LED subsystem failed emergency stop!"); Log.Exception(Log.Source.SUBSYSTEM, Exc); }

            try { this.AuxSensors.EmergencyStop(); }
            catch (Exception Exc) { Log.Output(Log.Severity.ERROR, Log.Source.SUBSYSTEM, "Aux Sensor subsystem failed emergency stop!"); Log.Exception(Log.Source.SUBSYSTEM, Exc); }

            try { this.SysSensors.Initialize(); }
            catch (Exception Exc) { Log.Output(Log.Severity.ERROR, Log.Source.SUBSYSTEM, "System Sensor subsystem failed emergency stop!"); Log.Exception(Log.Source.SUBSYSTEM, Exc); }
        }

        public void UpdateStates()
        {
            //this.RailController.UpdateState();
            this.DrillController.UpdateState();
            //this.SampleController.UpdateState();
            this.LEDController.UpdateState();
            this.AuxSensors.UpdateState();
            this.SysSensors.UpdateState();
        }
    }
}
