using Scarlet.IO;
using Scarlet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scarlet.Components
{
    public class PCA9685
    {
        private II2CBus Bus;
        private byte Address;
        private int Frequency = 60;

        private PCA9685Output[] Outputs = new PCA9685Output[4];

        #region REGISTERS AND VALUES

        private const byte PCA9685_MODE1 = 0x00;
        private const byte PCA9685_MODE2 = 0x01;
        private const byte PCA9685_PRESCALE = 0xFE;

        private const byte LED0_ON_L = 0x06;
        private const byte LED0_ON_H = 0x07;
        private const byte LED0_OFF_L = 0x08;
        private const byte LED0_OFF_H = 0x09;
        private const byte ALL_LED_ON_L = 0xFA;
        private const byte ALL_LED_ON_H = 0xFB;
        private const byte ALL_LED_OFF_L = 0xFC;
        private const byte ALL_LED_OFF_H = 0xFD;

        private const byte SLEEP = 0x10;
        private const byte ALLCALL = 0x01;
        private const byte OUTDRV = 0x04;

        #endregion



        public void SetFrequency(int Frequency)
        {
            float Prescale = 25000000;
            Prescale /= 4096;
            Prescale /= Frequency;
            Prescale -= 1;
            byte AdjPrescale = (byte)Math.Floor(Prescale + 0.5);
            byte OldMode = this.Bus.ReadRegister(this.Address, PCA9685_MODE1, 1)[0];
            byte NewMode = (byte)((OldMode & 0x7F) | 0x10);

            this.Bus.WriteRegister(this.Address, PCA9685_MODE1, new byte[] { NewMode });
            this.Bus.WriteRegister(this.Address, PCA9685_PRESCALE, new byte[] { AdjPrescale });
            this.Bus.WriteRegister(this.Address, PCA9685_MODE1, new byte[] { OldMode });
            Thread.Sleep(5);

            this.Bus.WriteRegister(this.Address, PCA9685_MODE1, new byte[] { (byte)(OldMode | 0x80) });

            this.Frequency = Frequency;
        }

        internal void SetPWM(int Channel, short On, short Off)
        {
            this.Bus.WriteRegister(this.Address, (byte)(LED0_ON_L + 4 * Channel), new byte[] { (byte)(On & 0xFF) });
            this.Bus.WriteRegister(this.Address, (byte)(LED0_ON_H + 4 * Channel), new byte[] { (byte)(On >> 8) });
            this.Bus.WriteRegister(this.Address, (byte)(LED0_OFF_L + 4 * Channel), new byte[] { (byte)(Off & 0xFF) });
            this.Bus.WriteRegister(this.Address, (byte)(LED0_OFF_H + 4 * Channel), new byte[] { (byte)(Off >> 8) });
        }

        internal void SetAllPWM(byte On, byte Off)
        {
            this.Bus.WriteRegister(this.Address, ALL_LED_ON_L, new byte[] { (byte)(On & 0xFF) });
            this.Bus.WriteRegister(this.Address, ALL_LED_ON_H, new byte[] { (byte)(On >> 8) });
            this.Bus.WriteRegister(this.Address, ALL_LED_OFF_L, new byte[] { (byte)(Off & 0xFF) });
            this.Bus.WriteRegister(this.Address, ALL_LED_OFF_H, new byte[] { (byte)(Off >> 8) });
        }

        public PCA9685(II2CBus Bus, byte I2CAddress = 0x60)
        {
            SetAllPWM(0, 0);
            this.Bus.WriteRegister(this.Address, PCA9685_MODE2, new byte[] { OUTDRV });
            this.Bus.WriteRegister(this.Address, PCA9685_MODE1, new byte[] { ALLCALL });
            Thread.Sleep(5);

            byte mode1 = this.Bus.ReadRegister(this.Address, PCA9685_MODE1, 1)[0];
            this.Bus.WriteRegister(this.Address, PCA9685_MODE1, new byte[] { (byte)(mode1 & ~SLEEP) });
            Thread.Sleep(5);

            SetFrequency(Frequency);
        }

        public IPWMOutput GetMotor(byte Channel)
        {
            if (this.Outputs[Channel] == null) { this.Outputs[Channel] = new PCA9685Output(this, Channel); }
            return this.Outputs[Channel];
        }

    }

    public class PCA9685Output : IPWMOutput
    {
        private PCA9685 Parent;
        private byte Channel;

        // Pin mappings for each motor: 0, 1, 2, 3
        private static readonly int[][] PINS = new int[][] {
            new int[] { 8, 9, 10 },
            new int[] { 13, 12, 11 },
            new int[] { 2, 4, 3 },
            new int[] { 7, 6, 5 }
        };

        private bool P_Enabled;
        private bool Enabled
        {
            get { return P_Enabled; }
            set {
                if (!value) { Stop(); }
                P_Enabled = value;
            }
        }

        public PCA9685Output(PCA9685 Parent, byte Channel)
        {
            this.Parent = Parent;
            this.Channel = Channel;
        }

        public void Dispose() { throw new NotImplementedException(); }

        public void SetEnabled(bool Enable) { Enabled = Enable; }

        public void SetFrequency(int Frequency) { this.Parent.SetFrequency(Frequency); }

        private void Stop() { SetOutput(50.0f); }

        // Takes in value -255 (reverse) to 255 (forward)
        private void SetOutputExactly(int Value)
        {
            int ForwardPin = PINS[this.Channel][1];
            int ReversePin = PINS[this.Channel][2];
            int ThrottlePin = PINS[this.Channel][0];

            this.Parent.SetPWM(ForwardPin, 4096, 0);
            this.Parent.SetPWM(ReversePin, 4096, 0);
            this.Parent.SetPWM(ThrottlePin, (short)(2048 - Math.Abs(Value) * 8), (short)(2048 + Math.Abs(Value) * 8));

            if (Value > 0) { this.Parent.SetPWM(ForwardPin, 0, 4096); }
            else { this.Parent.SetPWM(ReversePin, 0, 4096); }
        }

        // 0.0 to 100.0
        public void SetOutput(float DutyCycle)
        {
            if (!this.Enabled) { return; }
            // Map 0.0 to 100.0 to -255 to 255
            int MappedValue = (int)((DutyCycle - 50.0) * 5.1);
            if (Math.Abs(MappedValue) > 255)
            {
                Log.Output(Log.Severity.DEBUG, Log.Source.MOTORS, "Cannot set PCA9685Output duty cycle to more than 100.0");
                return;
            }

            SetOutputExactly(MappedValue);
        }
    }

}
