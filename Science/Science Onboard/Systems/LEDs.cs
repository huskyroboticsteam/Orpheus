using System;
using System.Threading;
using Scarlet.Communications;
using Scarlet.Components;
using Scarlet.Components.Outputs;
using Scarlet.IO;
using static Scarlet.Components.Outputs.PCA9685;

namespace Science.Systems
{
    public class LEDs : ISubsystem
    {
        public bool TraceLogging { get; set; }

        public RGBLED RunningColour { get => this.Lights[0]; }
        public RGBLED ServerStatus { get => this.Lights[1]; }
        public RGBLED SystemVoltage { get => this.Lights[2]; }
        public RGBLED SystemCurrent { get => this.Lights[3]; }
        public RGBLED DrillStatus { get => this.Lights[4]; }
        public RGBLED RailStatus { get => this.Lights[5]; }
        public RGBLED SystemStatus { get => this.Lights[6]; }
        public RGBLED Scarlet { get => this.Lights[7]; }

        private RGBLED[] Lights = new RGBLED[8];
        private bool DoUpdates = true;

        public LEDs(IPWMOutput[] LowFreqChannels, IPWMOutput[] HighFreqChannels)
        {
            this.Lights[0] = new RGBLED(HighFreqChannels[4], HighFreqChannels[5], HighFreqChannels[6]);
            this.Lights[1] = new RGBLED(HighFreqChannels[7], HighFreqChannels[8], HighFreqChannels[9]);
            this.Lights[2] = new RGBLED(HighFreqChannels[10], HighFreqChannels[11], HighFreqChannels[12]);
            this.Lights[3] = new RGBLED(HighFreqChannels[13], HighFreqChannels[14], HighFreqChannels[15]);

            this.Lights[4] = new RGBLED(LowFreqChannels[4], LowFreqChannels[5], LowFreqChannels[6]);
            this.Lights[5] = new RGBLED(LowFreqChannels[7], LowFreqChannels[8], LowFreqChannels[9]);
            this.Lights[6] = new RGBLED(LowFreqChannels[10], LowFreqChannels[11], LowFreqChannels[12]);
            this.Lights[7] = new RGBLED(LowFreqChannels[13], LowFreqChannels[14], LowFreqChannels[15]);
            for (int i = 4; i < 16; i++)
            {
                ((PWMOutputPCA9685)HighFreqChannels[i]).Reset();
                ((PWMOutputPCA9685)LowFreqChannels[i]).Reset();
            }
        }

        public void EmergencyStop()
        {
            this.DoUpdates = false;
            foreach(RGBLED LED in this.Lights) { LED.SetEnabled(false); }
        }

        public void Initialize()
        {
            foreach (RGBLED LED in this.Lights) { LED.SetEnabled(false); }
            foreach (RGBLED LED in this.Lights) { LED.RedScale = 0.05F; LED.GreenScale = 0.05F; LED.BlueScale = 0.05F; }
            foreach (RGBLED LED in this.Lights) { LED.SetOutput(0x7F7F7F); }
            foreach (RGBLED LED in this.Lights) { LED.SetEnabled(true); }
            
            new Thread(new ThreadStart(this.DoColourCycle)).Start();
            this.Scarlet.SetOutput(0x811426);
            this.ServerStatus.SetOutput(Client.IsConnected ? (uint)0x00FF00 : (uint)0xFF0000);
            Client.ClientConnectionChanged += this.ClientUpdate;
        }

        private void DoColourCycle()
        {
            int i = 0;
            this.RunningColour.SetEnabled(true);
            while (this.DoUpdates)
            {
                byte Red = (byte)((Math.Max(Math.Sin(i * 0.2F), -0.5) + 0.5) / 1.5 * 0xFF);
                byte Green = (byte)((Math.Max(Math.Sin((i + (10.0 / 3.0 * Math.PI)) * 0.2F), -0.5) + 0.5) / 1.5 * 0xFF);
                byte Blue = (byte)((Math.Max(Math.Sin((i + (20.0 / 3.0 * Math.PI)) * 0.2F), -0.5) + 0.5) / 1.5 * 0xFF);
                this.RunningColour.SetOutput((uint)(Red << 16 | Green << 8 | Blue));
                this.ServerStatus.SetOutput(Client.IsConnected ? (uint)0x00FF00 : (uint)0xFF0000); // TODO: Remove this when events are reliable.
                Thread.Sleep(100);
                i++;
            }
        }

        private void ClientUpdate(object Sender, ConnectionStatusChanged Event)
        {
            if (this.DoUpdates)
            {
                this.ServerStatus.SetOutput(Event.StatusConnected ? (uint)0x00FF00 : (uint)0xFF0000);
            }
        }

        public void UpdateState() { }
    }
}
