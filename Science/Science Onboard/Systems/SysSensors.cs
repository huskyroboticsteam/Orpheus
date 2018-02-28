using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Scarlet.Communications;
using Scarlet.Components;
using Scarlet.IO;
using Scarlet.IO.RaspberryPi;
using Science.Library;

namespace Science.Systems
{
    public class SysSensors : ISubsystem
    {
        private bool TakeReadings = false;
        private Timer Timer;

        // Comms buses
        private II2CBus I2C1;

        // Sensor endpoints
        private INA226 SystemSensor, DrillSensor, RailSensor;

        public SysSensors()
        {
            this.Timer = new Timer(this.UpdateState, null, 0, 500);
        }

        public void EmergencyStop() { this.TakeReadings = false; }

        public void EventTriggered(object Sender, EventArgs Event) { }

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

                byte[] Data; // TODO: Add sensor data!
                Packet Packet = new Packet(new Message(ScienceConstants.Packets.SYS_SENSOR, Data), false);
                Client.Send(Packet);
            }
        }
    }
}
