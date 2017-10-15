using System;

namespace Scarlet.Components
{
    public interface ISensor
    {
        /// <summary>Determine whether or not the sensor is working.</summary>
        bool Test();

        /// <summary>Updates the state of the sensor, usually done by getting a new reading.</summary>
        void UpdateState();

        /// <summary>Used to send events to sensors.</summary>
        void EventTriggered(object Sender, EventArgs Event);
    }
}
