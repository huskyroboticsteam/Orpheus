using System;
using WiringPi;

namespace RoboticsLibrary.Motors
{
    public class TalonMC : Motor
    {

        private readonly int Pin;

        public TalonMC(int Pin) : base(Pin)
        {
            this.Pin = Pin;
        }
        
        public override void EventTriggered(object Sender, EventArgs Event)
        {

        }

        public override void Initialize()
        {
            GPIO.pinMode(this.Pin, (int)GPIO.GPIOpinmode.PWMOutput);
            GPIO.pwmSetClock(1000); // TODO: Set this to an actual value.
        }

        public override void Stop()
        {
            GPIO.pwmWrite(this.Pin, 0);
        }

        public override void UpdateState()
        {

        }
    }
}
