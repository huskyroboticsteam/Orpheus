using Scarlet.IO;
using System;

namespace Scarlet.Components.Motors
{
    public class Servo : IServo
    {
        private readonly IPWMOutput PWMOut;
        public int Position { get; private set; }

        public Servo(IPWMOutput PWMOut)
        {
            this.PWMOut = PWMOut;
            this.PWMOut.SetFrequency(50); // TODO: Set this to an actual value, and check if this overrides others.
        }

        public void EventTriggered(object Sender, EventArgs Event)
        {
            
        }

        public void Stop() { SetPosition(this.Position); }

        public void SetPosition(int NewPosition)
        {
            if (this.Position == NewPosition) { return; }
            // TODO: Do filtering (move Position towards TargetPosition) and output new Position via PWM.
            this.Position = NewPosition;
        }
    }
}
