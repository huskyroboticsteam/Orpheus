using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scarlet.Components;
using Scarlet.Components.Outputs;
using Scarlet.IO;

namespace Science.Systems
{
    public class LEDs : ISubsystem
    {
        private RGBLED[] Lights = new RGBLED[8];

        public LEDs(IPWMOutput[] LowFreqChannels, IPWMOutput[] HighFreqChannels)
        {
            this.Lights[0] = new RGBLED(HighFreqChannels[6], HighFreqChannels[5], HighFreqChannels[4]);
            this.Lights[1] = new RGBLED(HighFreqChannels[9], HighFreqChannels[8], HighFreqChannels[7]);
            this.Lights[2] = new RGBLED(HighFreqChannels[12], HighFreqChannels[11], HighFreqChannels[10]);
            this.Lights[3] = new RGBLED(HighFreqChannels[15], HighFreqChannels[14], HighFreqChannels[13]);

            this.Lights[4] = new RGBLED(LowFreqChannels[6], LowFreqChannels[5], LowFreqChannels[4]);
            this.Lights[5] = new RGBLED(LowFreqChannels[9], LowFreqChannels[8], LowFreqChannels[7]);
            this.Lights[6] = new RGBLED(LowFreqChannels[12], LowFreqChannels[11], LowFreqChannels[10]);
            this.Lights[7] = new RGBLED(LowFreqChannels[15], LowFreqChannels[14], LowFreqChannels[13]);
        }

        public void EmergencyStop()
        {
            foreach(RGBLED LED in this.Lights) { LED.SetEnabled(false); }
        }

        public void EventTriggered(object Sender, EventArgs Event)
        {
            
        }

        public void Initialize()
        {
            foreach (RGBLED LED in this.Lights) { LED.SetOutput(0x811426); }
        }

        public void UpdateState()
        {
            
        }
    }
}
