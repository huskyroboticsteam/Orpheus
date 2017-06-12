using System;

namespace RoboticsLibrary.Motors
{
    public interface IMotor
    {
        // Sets speed, and checks parameters such as current and timeouts for issues.
        void UpdateState();

        // Re-homes the device attached to this motor. This can take a while.
        void Initialize();

        // Stops the motor.
        void Stop();

        // Processes an event trigger from something like a limit switch.
        void EventTriggered(object Sender, EventArgs Event);
    }
}
