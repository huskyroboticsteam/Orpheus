using System;

namespace Scarlet.Components
{
    public interface IMotor
    {
        /// <summary>Updates the state of the motor, changing the speed if relevant.</summary>
        void UpdateState();

        /// <summary>Stops to motor immediately.</summary>
        void Stop();

        /// <summary>Used to send events to Motors.</summary>
        void EventTriggered(object Sender, EventArgs Event);
    }
}