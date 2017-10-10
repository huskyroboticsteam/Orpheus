using Scarlet.IO;
using System;

namespace Scarlet.Components.Motors
{
    public class Servo : IMotor
    {
        private readonly IPWMOutput PWMOut;
        public int Position { get; private set; }
        private int P_TargetPosition;
        public int TargetPosition
        {
            get { return this.P_TargetPosition; }
            set
            {
                this.P_TargetPosition = value % 360;
                this.UpdateState();
            }
        }

        public Servo(IPWMOutput PWMOut)
        {
            this.PWMOut = PWMOut;
            this.PWMOut.SetFrequency(50); // TODO: Set this to an actual value, and check if this overrides others.
        }

        public void EventTriggered(object Sender, EventArgs Event)
        {
            
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
