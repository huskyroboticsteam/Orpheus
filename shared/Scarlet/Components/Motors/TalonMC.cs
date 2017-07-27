using System;
using Scarlet.Filters;

namespace Scarlet.Components.Motors
{
    public class TalonMC : IMotor
    {
        private LowPass<double> LPF = new LowPass<double>();
        private readonly int Pin;
        private readonly float MaxSpeed;
        private float P_RampUp, P_Speed;

        // Float from 0 to 1, higher the number the longer the ramp up time
        public float RampUp
        {
            get { return this.P_RampUp; }
            set
            {
                this.P_RampUp = value;
                if (value > 1) { this.P_RampUp = 1; }
                if (value < 0) { this.P_RampUp = 0; }
            }
        }

        public float Speed
        {
            get { return this.P_Speed; }
            set
            {
                this.P_Speed = value;
                if (value > this.MaxSpeed) { this.P_Speed = this.MaxSpeed; }
                if (value * -1 > this.MaxSpeed) { this.P_Speed = this.MaxSpeed * -1; }
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
            //GPIO.pinMode(this.Pin, (int)GPIO.GPIOpinmode.PWMOutput);
            //GPIO.pwmSetClock(1000); // TODO: Set this to an actual value.
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
