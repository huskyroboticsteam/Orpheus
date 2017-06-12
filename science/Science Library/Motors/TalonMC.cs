using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboticsLibrary.Motors
{
    public abstract class TalonMC : Motor
    {

        public TalonMC(int Pin) : base(Pin)
        {

        }
        
        public override void EventTriggered(object Sender, EventArgs Event)
        {

        }

        public override void Initialize()
        {

        }

        public override void Stop()
        {

        }

        public override void UpdateState()
        {

        }
    }
}
