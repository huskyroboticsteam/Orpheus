using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboticsLibrary.Motors
{
    public abstract class TalonMC : IMotor
    {

        public TalonMC(int Pin)
        {

        }
        
        public abstract void EventTriggered(object Sender, EventArgs Event);

        public abstract void Initialize();

        public abstract void Stop();

        public abstract void UpdateState();
    }
}
