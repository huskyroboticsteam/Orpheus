using System;

namespace RoboticsLibrary.Motors
{
    public abstract class Servo : IMotor
    {

        public Servo(int Pin)
        {

        }

        public abstract void EventTriggered(object Sender, EventArgs Event);

        public abstract void Initialize();

        public abstract void Stop();

        public abstract void UpdateState();
    }
}
