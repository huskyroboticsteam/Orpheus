using System;
using Scarlet.Filters;
using Scarlet.IO;
using Scarlet.Utilities;

namespace Scarlet.Components.Motors
{
    public class TalonMC : IMotor
    {
        private IFilter<float> Filter;
        private readonly IPWMOutput PWMOut;
        private readonly float MaxSpeed;

        private bool Stopped;
        private float P_TargetSpeed;
        public float TargetSpeed
        {
            get { return this.P_TargetSpeed; }
            set
            {
                this.Stopped = false;
                this.P_TargetSpeed = value;
                if (value > this.MaxSpeed) { this.P_TargetSpeed = this.MaxSpeed; }
                if (value * -1 > this.MaxSpeed) { this.P_TargetSpeed = this.MaxSpeed * -1; }
            }
        }

        public TalonMC(IPWMOutput PWMOut, float MaxSpeed, IFilter<float> SpeedFilter = null)
        {
            this.PWMOut = PWMOut;
            this.MaxSpeed = MaxSpeed;
            this.Filter = SpeedFilter;
            this.PWMOut.SetFrequency(333);
            this.PWMOut.SetEnabled(true);
        }
        
        public void EventTriggered(object Sender, EventArgs Event)
        {

        }

        /// <summary>Immediately stops the motor, bypassing the Filter.</summary>
        public void Stop()
        {
            this.TargetSpeed = 0;
            this.Stopped = true;
            this.UpdateState();
        }

        public void UpdateState()
        {
            float SetSpeed = this.TargetSpeed;
            if (this.Filter != null)
            {
                this.Filter.Feed(this.TargetSpeed);
                SetSpeed = this.Filter.GetOutput();
            }
            if (this.Stopped) { SetSpeed = 0; }
            if (SetSpeed > this.MaxSpeed) { SetSpeed = this.MaxSpeed; }
            if (SetSpeed * -1 > this.MaxSpeed) { SetSpeed = this.MaxSpeed * -1; }

            //Log.Output(Log.Severity.DEBUG, Log.Source.MOTORS, "Target: " + this.TargetSpeed + ", Filtered: " + SetSpeed);
            this.PWMOut.SetOutput((SetSpeed / 2) + 0.5F);
        }
    }
}
