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
        public float TargetSpeed { get; private set; }

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
            this.SetSpeed(0);
        }

        public void SetSpeed(float Speed)
        {
            float NewSpeed = Speed;
            if (this.Filter != null)
            {
                this.Filter.Feed(this.TargetSpeed);
                NewSpeed = this.Filter.GetOutput();
            }
            if (this.Stopped) { NewSpeed = 0; }
            if (NewSpeed > this.MaxSpeed)
            {
                NewSpeed = this.MaxSpeed;
                Log.Output(Log.Severity.DEBUG, Log.Source.MOTORS, "Motor max speed exceeded. Capping.");
            }
            if (NewSpeed * -1 > this.MaxSpeed)
            {
                NewSpeed = this.MaxSpeed * -1;
                Log.Output(Log.Severity.DEBUG, Log.Source.MOTORS, "Motor max speed exceeded. Capping.");
            }
            Log.Output(Log.Severity.DEBUG, Log.Source.MOTORS, "Target: " + this.TargetSpeed + ", Filtered: " + NewSpeed);
            this.PWMOut.SetOutput((NewSpeed / 2) + 0.5F);
        }
    }
}
