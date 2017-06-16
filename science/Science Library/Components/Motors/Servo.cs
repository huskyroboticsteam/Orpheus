using System;
using WiringPi;

namespace RoboticsLibrary.Components.Motors
{
    public class Servo : IMotor
    {
        private readonly int Pin;
        private int TargetPosition;
        public int Position
        {
            get { return this.Position; }
            set
            {
                this.TargetPosition = value % 360;
                this.UpdateState();
            }
        }

        public Servo(int Pin)
        {
            this.Pin = Pin;
        }

        public void EventTriggered(object Sender, EventArgs Event)
        {
            
        }

        public void Initialize()
        {
            GPIO.pinMode(this.Pin, (int)GPIO.GPIOpinmode.PWMOutput);
            GPIO.pwmSetClock(50); // TODO: Set this to an actual value, and check if this overrides others.
        }

        public void Stop()
        {
            this.TargetPosition = this.Position;
            this.UpdateState();
        }

        public void UpdateState()
        {
            if (this.Position == this.TargetPosition) { return; }
            // TODO: Do filtering (move Position towards TargetPosition) and output new Position via PWM.
        }
    }
}
