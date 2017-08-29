using BBBCSIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scarlet.IO.BeagleBone
{
    public static class PWMBBB
    {
        public static PWMDeviceBBB PWMDevice0 { get; private set; }
        public static PWMDeviceBBB PWMDevice1 { get; private set; }
        public static PWMDeviceBBB PWMDevice2 { get; private set; }

        // Called by BeagleBone.Initialize() as part of system preparation.
        static internal void Initialize()
        {
            //                                                  A1            A2                             B1            B2
            PWMDevice0 = new PWMDeviceBBB(new BBBPin[] { BBBPin.P9_22, BBBPin.P9_31 }, new BBBPin[] { BBBPin.P9_21, BBBPin.P9_29 });
            PWMDevice1 = new PWMDeviceBBB(new BBBPin[] { BBBPin.P9_14, BBBPin.P8_36 }, new BBBPin[] { BBBPin.P9_16, BBBPin.P8_34 });
            PWMDevice2 = new PWMDeviceBBB(new BBBPin[] { BBBPin.P8_19, BBBPin.P8_45 }, new BBBPin[] { BBBPin.P8_13, BBBPin.P8_46 });
        }

        static internal PWMPortEnum PinToPWMID(BBBPin Pin)
        {
            switch(Pin)
            {
                case BBBPin.P9_22:
                case BBBPin.P9_31: return PWMPortEnum.PWM0_A;

                case BBBPin.P9_21:
                case BBBPin.P9_29: return PWMPortEnum.PWM0_B;

                case BBBPin.P9_14:
                case BBBPin.P8_36: return PWMPortEnum.PWM1_A;

                case BBBPin.P9_16:
                case BBBPin.P8_34: return PWMPortEnum.PWM1_B;

                case BBBPin.P8_19:
                case BBBPin.P8_45: return PWMPortEnum.PWM2_A;

                case BBBPin.P8_13:
                case BBBPin.P8_46: return PWMPortEnum.PWM2_B;
            }
            return PWMPortEnum.PWM_NONE;
        }
    }

    public class PWMDeviceBBB
    {
        public PWMOutputBBB OutputA { get; private set; }
        public PWMOutputBBB OutputB { get; private set; }

        internal PWMDeviceBBB(BBBPin[] PinsA, BBBPin[] PinsB)
        {
            this.OutputA = new PWMOutputBBB(PinsA);
            this.OutputB = new PWMOutputBBB(PinsB);
        }

        public void SetFrequency(int Frequency)
        {
            if (this.OutputA.Port != null) { this.OutputA.Port.FrequencyHz = (uint)Frequency; }
            else if (this.OutputB.Port != null) { this.OutputB.Port.FrequencyHz = (uint)Frequency; }
            else { throw new InvalidOperationException("Cannot change frequency of device before initialization."); }

            if (this.OutputA.Port != null) { this.OutputA.ResetOutput(); }
            if (this.OutputB.Port != null) { this.OutputB.ResetOutput(); }
        }
    }

    public class PWMOutputBBB : IPWMOutput
    {
        private BBBPin[] Pins;
        private float DutyCycle = -1;

        internal PWMPortMM Port;

        internal PWMOutputBBB(BBBPin[] Pins)
        {
            this.Pins = Pins;
        }

        public void Dispose() { }

        public void Initialize()
        {
            this.Port = new PWMPortMM(PWMBBB.PinToPWMID(this.Pins[0]));
        }

        public void SetFrequency(int Frequency)
        {
            throw new InvalidOperationException("Cannot set frequency on individual outputs on BBB. You must change the frequency on a PWM device level. Please see the documentation for more details.");
        }

        public void SetOutput(float DutyCycle)
        {
            this.Port.DutyPercent = DutyCycle;
            this.DutyCycle = DutyCycle;
        }

        internal void ResetOutput()
        {
            if (this.DutyCycle != -1) { this.Port.DutyPercent = this.DutyCycle; }
        }
    }
}
