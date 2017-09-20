using BBBCSIO;
using Scarlet.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
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
            PWMPortEnum Device = PWMBBB.PinToPWMID(this.Pins[0]);
            // Final path will be: /sys/devices/platform/ocp/4830_000.epwmss/4830_200.pwm/pwm/pwmchip_/pwm_
            //                                                   x               x                   y    z
            // Where x refers to device #. dev 0 = 0, dev 1 = 2, dev 2 = 4.
            //       y is an arbitrary number.
            //          To me, it looks like this is assigned from 0 in steps of +2, in the order that the PWM devices were loaded into the device tree.
            //          Operationally, it seems to have no significance, and there only ever seems to be one in each *.epwmss/*.pwm/ folder.
            //          This is probably because at that point it is already narrowed to a single device (2 pins).
            //       z refers to output #. out A = 0, out B = 2.
            // We need to write the pin number (z) into the "export" file, and a 1 into the "enable" file to prepare the pin for use.
            // At that point, whatever needed to be set in memory is ready for BBBCSIO's PWMPortMM to take over and work.
            // Why does Linux have to be so damn difficult with everything it does? :(
            string Path = "/sys/devices/platform/ocp";
            // Append the memory addresses.
            byte ExportNum = 0;
            switch(Device)
            {
                case PWMPortEnum.PWM0_A:
                    Path += "/48300000.epwmss/48300200.pwm/pwm/";
                    ExportNum = 0;
                    break;
                case PWMPortEnum.PWM0_B:
                    Path += "/48300000.epwmss/48300200.pwm/pwm/";
                    ExportNum = 1;
                    break;
                case PWMPortEnum.PWM1_A:
                    Path += "/48302000.epwmss/48302200.pwm/pwm/";
                    ExportNum = 0;
                    break;
                case PWMPortEnum.PWM1_B:
                    Path += "/48302000.epwmss/48302200.pwm/pwm/";
                    ExportNum = 1;
                    break;
                case PWMPortEnum.PWM2_A:
                    Path += "/48304000.epwmss/48304200.pwm/pwm/";
                    ExportNum = 0;
                    break;
                case PWMPortEnum.PWM2_B:
                    Path += "/48304000.epwmss/48304200.pwm/pwm/";
                    ExportNum = 1;
                    break;
                default: throw new Exception("Invalid PWM pin given.");
            }

            // Append the (arbitrary) pwmchip #, by using the first one we find.
            string[] PWMChips = Directory.GetDirectories(Path);
            Log.Output(Log.Severity.DEBUG, Log.Source.HARDWAREIO, "Attempting to find correct pwmchip number...");
            bool FoundPWMChip = false;
            foreach(string SubdirPath in PWMChips)
            {
                string Subdir = new DirectoryInfo(SubdirPath).Name;
                if (Subdir.StartsWith("pwmchip"))
                {
                    Path += Subdir;
                    FoundPWMChip = true;
                    Log.Output(Log.Severity.DEBUG, Log.Source.HARDWAREIO, "Found \"" + Subdir + "\", using.");
                }
                else { Log.Output(Log.Severity.DEBUG, Log.Source.HARDWAREIO, "Found \"" + Subdir + "\", no good."); }
            }
            if (!FoundPWMChip) { throw new Exception("Could not find PWM chip number. Is your device tree set correctly?"); }

            // Export and enable the pin.
            if (!Directory.Exists(Path + "/pwm" + ExportNum))
            {
                StreamWriter Exporter = File.AppendText(Path + "/export");
                Exporter.Write(ExportNum);
                Exporter.Flush();
                Exporter.Close();
            }
            Path += ("/pwm" + ExportNum);
            StreamWriter Enabler = File.AppendText(Path + "/enable");
            Enabler.Write("1");
            Enabler.Flush();
            Enabler.Close();
            this.Port = new PWMPortMM(Device);
        }

        public void SetFrequency(int Frequency)
        {
            throw new InvalidOperationException("Cannot set frequency on individual outputs on BBB. You must change the frequency on a PWM device level. Please see the documentation for more details.");
        }

        public void SetOutput(float DutyCycle)
        {
            this.Port.DutyPercent = DutyCycle * 100F;
            this.DutyCycle = DutyCycle * 100F;
        }

        public void SetEnabled(bool Enabled)
        {
            this.Port.RunState = Enabled;
        }

        internal void ResetOutput()
        {
            if (this.DutyCycle != -1) { this.Port.DutyPercent = this.DutyCycle; }
        }
    }
}
