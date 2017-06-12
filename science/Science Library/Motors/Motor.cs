using System;
using RoboticsLibrary.Sensors;

namespace RoboticsLibrary.Motors
{
    
    public abstract class Motor : IMotor
    {
        /// <summary>
        /// A generic DC motor. This class is usually subclassed for individual motors.
        /// </summary>
        /// <param name="Pin">The pin on which to output from to drive this motor.</param>
        public Motor(int Pin)
        {

        }

        public abstract void EventTriggered(object Sender, EventArgs Event);

        public abstract void Initialize();

        public abstract void Stop();

        public abstract void UpdateState();
    }
}
