using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scarlet.Components;
using Scarlet.Components.Motors;
using Scarlet.IO;

namespace Science.Systems
{
    public class Sample : ISubsystem
    {
        public bool TraceLogging { get; set; }
        private Servo Servo;

        public Sample(IPWMOutput ServoPWM)
        {
            this.Servo = new Servo(ServoPWM);
        }

        public void EmergencyStop()
        {
            
        }

        public void Initialize()
        {
            
        }

        public void UpdateState()
        {
            
        }

        public void Exit() { }
    }
}
