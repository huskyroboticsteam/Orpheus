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

        private readonly RGBLED[] Lights = new RGBLED[4];
        private bool DoUpdates = true;

        public LEDs(IPWMOutput[] PWMCh)
        {
            this.Lights[0] = new RGBLED(PWMCh[4], PWMCh[5], PWMCh[6]);
            this.Lights[1] = new RGBLED(PWMCh[7], PWMCh[8], PWMCh[9]);
            this.Lights[2] = new RGBLED(PWMCh[10], PWMCh[11], PWMCh[12]);
            this.Lights[3] = new RGBLED(PWMCh[13], PWMCh[14], PWMCh[15]);
            for (int i = 4; i < 16; i++) { ((PWMOutputPCA9685)PWMCh[i]).Reset(); }
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

        public void Exit()
        {
            this.DoUpdates = false;
            foreach (RGBLED LED in this.Lights) { LED.SetEnabled(false); }
        }
    }
}
