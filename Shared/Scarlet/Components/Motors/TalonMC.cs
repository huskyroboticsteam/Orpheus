using System;
using Scarlet.Filters;
using Scarlet.IO;

namespace Scarlet.Components.Motors
{
    public class TalonMC : IMotor
    {
        private LowPass<double> LPF = new LowPass<double>();
        private readonly IPWMOutput Pin;
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

        public TalonMC(IPWMOutput Pin, float MaxSpeed)
        {
            this.Pin = Pin;
            this.MaxSpeed = MaxSpeed;
        }
        
        public void EventTriggered(object Sender, EventArgs Event)
        {

        }

        public void Initialize()
        {
            this.Pin.Initialize(); // TODO: Clean up the pin when finished using.
            this.Pin.SetFrequency(333);
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
            this.Pin.SetOutput(((float)SetSpeed / 2) + 0.5F);
        }
    }
}
