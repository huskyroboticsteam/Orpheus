using System;
using WiringPi;
using RoboticsLibrary.Filters;

namespace RoboticsLibrary.Components.Motors
{
    public class TalonMC : IMotor
    {
        private LowPass<double> LPF = new LowPass<double>();
        private readonly int Pin;
        private readonly float MaxSpeed;
        public float RampUp
        {
            get { return this.RampUp; }
            set
            {
                this.RampUp = value;
                if (value > 1) { this.RampUp = 1; }
                if (value < 0) { this.RampUp = 0; }
            }
        } // Float from 0 to 1, higher the number the longer the ramp up time
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
            this.LPF.LPFk = this.RampUp;
            this.LPF.Feed(this.Speed);
            double SetSpeed = this.LPF.Output;
            // TODO: Output the relevant PWM signal use SetSpeed as the actual value.
            
        }
    }
}
