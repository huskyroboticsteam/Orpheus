using System;
using Scarlet.Filters;
using Scarlet.IO;
using Scarlet.Utilities;

namespace Scarlet.Components.Motors
{
    public class CytronMD30C : IMotor
    {
        private IFilter<float> Filter;
        private readonly IPWMOutput PWMOut;
        private readonly float MaxSpeed;

        private bool Stopped;
        public float TargetSpeed { get; private set; }

        public CytronMD30C(IPWMOutput PWMOut, float MaxSpeed, IFilter<float> SpeedFilter = null)
        {
            this.PWMOut = PWMOut;
            this.MaxSpeed = MaxSpeed;
            this.Filter = SpeedFilter;
            this.PWMOut.SetFrequency(10000);
            this.PWMOut.SetEnabled(true);
        }

        public void EventTriggered(object Sender, EventArgs Event)
        {

        }

        /// <summary> Immediately stops the motor, bypassing the Filter. </summary>
        public void Stop()
        {
            this.TargetSpeed = 0;
            this.Stopped = true;
            this.SetSpeed(0);
        }

        /// <summary>
        /// Sets the motor speed. Output may vary from the given value under the following conditions:
        /// - Input exceeds maximum speed. Capped to given maximum.
        /// - Filter changes value. Filter's output used instead.
        ///     (If filter is null, this does not occur)
        /// - The motor is disabled. You must first re-enable the motor.
        /// </summary>
        /// <param name="Speed"> The new speed to set the motor at. </param>
        public void SetSpeed(float Speed)
        {
            float NewSpeed = Speed;
            if (this.Filter != null)
            {
                this.Filter.Feed(this.TargetSpeed);
                NewSpeed = this.Filter.GetOutput();
            }
            if (this.Stopped) { NewSpeed = 0; }
            if (NewSpeed > this.MaxSpeed) { NewSpeed = this.MaxSpeed; }
            if (NewSpeed * -1 > this.MaxSpeed) { NewSpeed = this.MaxSpeed * -1; }

            //Log.Output(Log.Severity.DEBUG, Log.Source.MOTORS, "Target: " + this.TargetSpeed + ", Filtered: " + SetSpeed);
            this.PWMOut.SetOutput((NewSpeed / 2) + 0.5F);
        }
    }
}