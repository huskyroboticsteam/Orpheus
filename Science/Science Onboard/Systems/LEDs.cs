using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Scarlet.Components;
using Scarlet.Components.Outputs;
using Scarlet.IO;
using static Scarlet.Components.Outputs.PCA9685;

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
            for (int i = 4; i < 16; i++)
            {
                ((PWMOutputPCA9685)HighFreqChannels[i]).Reset();
                ((PWMOutputPCA9685)LowFreqChannels[i]).Reset();
            }
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
            foreach (RGBLED LED in this.Lights) { LED.SetEnabled(false); }
            foreach (RGBLED LED in this.Lights) { LED.SetOutput(0x811426); }
            foreach (RGBLED LED in this.Lights) { LED.RedScale = 0.05F; LED.GreenScale = 0.05F; LED.BlueScale = 0.05F; }
            foreach (RGBLED LED in this.Lights) { LED.SetEnabled(true); }
            new Thread(new ThreadStart(DoBlink)).Start();
        }

        private void DoBlink()
        {
            int i = 0;
            foreach (RGBLED LED in this.Lights) { LED.SetEnabled(true); }
            while (true)
            {
                foreach (RGBLED LED in this.Lights)
                {
                    byte Red = (byte)((Math.Sin(i * 0.3F) + 1) * 0.5 * 0xFF);
                    byte Green = (byte)((Math.Sin(i * 0.4F) + 1) * 0.5 * 0xFF);
                    byte Blue = (byte)((Math.Sin(i * 0.5F) + 1) * 0.5 * 0xFF);
                    LED.SetOutput((uint)(Blue << 16 | Green << 8 | Red));
                }
                Thread.Sleep(50);
                i++;
            }
        }

        public void UpdateState()
        {
            
        }
    }
}
