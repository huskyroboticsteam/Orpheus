using System;
using WiringPi;

namespace RoboticsLibrary.Components.Motors
{
    public class TalonMC : IMotor
    {
        private readonly int Pin;
        private readonly float MaxSpeed;
        public float Speed
        {
            get { return this.Speed; }
            set
            {
                this.Speed = value;
                if (this.Speed > this.MaxSpeed) { this.Speed = this.MaxSpeed; }
                if (this.Speed * -1 > this.MaxSpeed) { this.Speed = this.MaxSpeed * -1; }
                this.UpdateState();
            }
        }

        public TalonMC(int Pin, float MaxSpeed)
        {
            this.Pin = Pin;
            this.MaxSpeed = MaxSpeed;
        }
        
        public void EventTriggered(object Sender, EventArgs Event)
        {

        }

        public void Initialize()
        {
            GPIO.pinMode(this.Pin, (int)GPIO.GPIOpinmode.PWMOutput);
            GPIO.pwmSetClock(1000); // TODO: Set this to an actual value.
        }

        public void Stop()
        {
            this.Speed = 0;
            this.UpdateState();
        }

        public void UpdateState()
        {
            // TODO: Output the relevant PWM signal.
        }
    }
}
