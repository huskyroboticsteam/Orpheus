using System;

namespace RoboticsLibrary.Components
{
    public interface ISensor
    {
        /// <summary>
        /// Prepares the sensor for use (e.g. sets up GPIO pins, initializes communications, calibrates).
        /// </summary>
        void Initialize();

        /// <summary>
        /// Determine whether or not the sensor is working.
        /// </summary>
        bool Test();

        /// <summary>
        /// Updates the state of the sensor, usually done by getting a new reading.
        /// </summary>
        void UpdateState();

        /// <summary>
        /// Used to send events to sensors.
        /// </summary>
        void EventTriggered(object Sender, EventArgs Event);
    }
}
