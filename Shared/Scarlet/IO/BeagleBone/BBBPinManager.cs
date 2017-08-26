using Scarlet.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Scarlet.IO.BeagleBone
{
    public static class BBBPinManager
    {
        private static Dictionary<BBBPin, PinAssignment> GPIOMappings, PWMMappings;

        public static void AddMappingGPIO(BBBPin SelectedPin, bool IsOutput, ResistorState Resistor, bool FastSlew = true)
        {
            byte Mode = Pin.GetModeID(SelectedPin, BBBPinMode.GPIO);
            if (Mode == 255) { throw new InvalidOperationException("This type of output is not supported on this pin."); }
            if (!Pin.CheckPin(SelectedPin, BeagleBone.Peripherals)) { throw new InvalidOperationException("This pin cannot be used without disabling some peripherals first."); }
            if (Pin.GetOffset(SelectedPin) == 0x000) { throw new InvalidOperationException("This pin is not valid for device tree registration. ADC pins do not need to be registered."); }
            lock (PWMMappings)
            {
                if (PWMMappings.ContainsKey(SelectedPin)) { throw new InvalidOperationException("This pin is already registered as PWM, cannot also use for GPIO."); }
            }

            if (GPIOMappings == null) { GPIOMappings = new Dictionary<BBBPin, PinAssignment>(); }
            PinAssignment NewMap = new PinAssignment(SelectedPin, Pin.GetPinMode(FastSlew, !IsOutput, Resistor, Mode));
            lock(GPIOMappings)
            {
                if (GPIOMappings.ContainsKey(SelectedPin))
                {
                    Log.Output(Log.Severity.WARNING, Log.Source.HARDWAREIO, "Overriding pin setting. This may mean that you have a pin usage conflict.");
                    GPIOMappings[SelectedPin] = NewMap;
                }
                else { GPIOMappings.Add(SelectedPin, NewMap); }
            }
        }

        public static void AddMappingPWM(BBBPin SelectedPin)
        {
            byte Mode = Pin.GetModeID(SelectedPin, BBBPinMode.PWM);
            if (Mode == 255) { throw new InvalidOperationException("This pin cannot be used for PWM output."); }
            if (!Pin.CheckPin(SelectedPin, BeagleBone.Peripherals)) { throw new InvalidOperationException("This pin cannot be used for PWM without disabling some peripherals first."); }
            if (Pin.GetOffset(SelectedPin) == 0x000) { throw new InvalidOperationException("This pin is not valid for device tree registration."); }
            lock (GPIOMappings)
            {
                if (GPIOMappings.ContainsKey(SelectedPin)) { throw new InvalidOperationException("This pin is already registered as GPIO, cannot also use for PWM."); }
            }

            if (PWMMappings == null) { PWMMappings = new Dictionary<BBBPin, PinAssignment>(); }
            PinAssignment NewMap = new PinAssignment(SelectedPin, Mode);
            lock(PWMMappings)
            {
                if (PWMMappings.ContainsKey(SelectedPin))
                {
                    Log.Output(Log.Severity.WARNING, Log.Source.HARDWAREIO, "Overriding PWM pin setting. This may mean that you have a pin usage conflict.");
                    PWMMappings[SelectedPin] = NewMap;
                }
                else { PWMMappings.Add(SelectedPin, NewMap); }
            }
        }

        /// <summary>
        /// Generates the device tree file, compiles it, and instructs the kernel to load the overlay though the cape manager. May take a while.
        /// Currently this can only be done once, as Scarlet does not have a way of removing the existing mappings.
        /// </summary>
        public static void ApplyPinSettings()
        {
            // Generate the device tree
            if(GPIOMappings == null || GPIOMappings.Count == 0) { Log.Output(Log.Severity.INFO, Log.Source.HARDWAREIO, "No pins defined, skipping device tree application."); return; }
            string FileName = "Scarlet-DT";
            string OutputDTFile = FileName + ".dts";
            List<string> DeviceTree = GenerateDeviceTree();

            // Save the device tree to file
            File.WriteAllLines(OutputDTFile, DeviceTree);

            // Compile the device tree file
            // Command: dtc -O dtb -o Scarlet-DT-00A0.dtbo -b 0 -@ Scarlet-DT.dts
            string CompiledDTFile = FileName + "-00A0.dtbo";
            Process Compile = new Process();
            Compile.StartInfo.FileName = "dtc";
            Compile.StartInfo.Arguments = "-O dtb -o \"" + CompiledDTFile + "\" -b 0 -@ \"" + OutputDTFile + "\"";
            Log.Output(Log.Severity.INFO, Log.Source.HARDWAREIO, "Compiling device tree...");
            Compile.Start();
            Compile.WaitForExit();

            // Copy the compiled file to the firmware folder
            // Command: cp Scarlet-DT-00A0.dtbo /lib/firmware
            if(!File.Exists(CompiledDTFile)) { throw new FileNotFoundException("Failed to get compiled device tree!"); }
            File.Copy(CompiledDTFile, "/lib/firmware/" + CompiledDTFile, true);

            // Delete the compiled tree file in execution folder
            File.Delete(CompiledDTFile);

            // Apply the device tree
            // Command: echo Scarlet-DT > /sys/devices/platform/bone_capemgr/slots
            File.WriteAllText("/sys/devices/platform/bone_capemgr/slots", "Scarlet-DT");
        }

        private class PinAssignment
        {
            public BBBPin Pin { get; private set; }
            public byte Mode { get; private set; }

            public PinAssignment(BBBPin Pin, byte Mode)
            {
                this.Pin = Pin;
                this.Mode = Mode;
            }
        }

        static List<string> GenerateDeviceTree()
        {
            List<string> Output = new List<string>();

            Output.Add("/dts-v1/;");
            Output.Add("/plugin/;");
            Output.Add("");
            Output.Add("/ {");
            Output.Add("    /* Generated by Scarlet. */");
            Output.Add("    compatible = \"ti,beaglebone\", \"ti,beaglebone-black\";");
            Output.Add("    part-number = \"scarlet-pins\";");
            Output.Add("    model = \"scarlet,scarlet-overlay\";");
            Output.Add("    version = \"00A1\";");
            Output.Add("    ");

            Dictionary<BBBPin, PinAssignment> PWMDev0 = new Dictionary<BBBPin, PinAssignment>();
            Dictionary<BBBPin, PinAssignment> PWMDev1 = new Dictionary<BBBPin, PinAssignment>();
            Dictionary<BBBPin, PinAssignment> PWMDev2 = new Dictionary<BBBPin, PinAssignment>();

            if (PWMMappings != null)
            {
                lock (PWMMappings)
                {
                    foreach (KeyValuePair<BBBPin, PinAssignment> Entry in PWMMappings)
                    {
                        switch (Entry.Key)
                        {
                            case BBBPin.P9_22: // 0A
                            case BBBPin.P9_31: // 0A
                            case BBBPin.P9_21: // 0B
                            case BBBPin.P9_29: // 0B
                                {
                                    PWMDev0.Add(Entry.Key, Entry.Value);
                                    continue;
                                }

                            case BBBPin.P9_14: // 1A
                            case BBBPin.P8_36: // 1A
                            case BBBPin.P9_16: // 1B
                            case BBBPin.P8_34: // 1B
                                {
                                    PWMDev1.Add(Entry.Key, Entry.Value);
                                    continue;
                                }

                            case BBBPin.P8_19: // 2A
                            case BBBPin.P8_45: // 2A
                            case BBBPin.P8_13: // 2B
                            case BBBPin.P8_46: // 2B
                                {
                                    PWMDev2.Add(Entry.Key, Entry.Value);
                                    continue;
                                }
                        }
                    }

                    int ListedNum = 0;
                    Output.Add("    exclusive-use =");
                    if (PWMDev0.Count != 0)
                    {
                        Output.Add("        \"epwmss0\",");
                        Output.Add("        \"ehrpwm0\",");
                        foreach (PinAssignment OnePin in PWMDev0.Values)
                        {
                            string PinName = OnePin.Pin.ToString("F");
                            PinName = PinName.Replace('_', '.');
                            char Terminator = (ListedNum == PWMMappings.Count - 1) ? ';' : ',';
                            Output.Add("        \"" + PinName + "\"" + Terminator);
                            ListedNum++;
                        }
                    }
                    if (PWMDev1.Count != 0)
                    {
                        Output.Add("        \"epwmss1\",");
                        Output.Add("        \"ehrpwm1\",");
                        foreach (PinAssignment OnePin in PWMDev1.Values)
                        {
                            string PinName = OnePin.Pin.ToString("F");
                            PinName = PinName.Replace('_', '.');
                            char Terminator = (ListedNum == PWMMappings.Count - 1) ? ';' : ',';
                            Output.Add("        \"" + PinName + "\"" + Terminator);
                            ListedNum++;
                        }
                    }
                    if (PWMDev2.Count != 0)
                    {
                        Output.Add("        \"epwmss2\",");
                        Output.Add("        \"ehrpwm2\",");
                        foreach (PinAssignment OnePin in PWMDev2.Values)
                        {
                            string PinName = OnePin.Pin.ToString("F");
                            PinName = PinName.Replace('_', '.');
                            char Terminator = (ListedNum == PWMMappings.Count - 1) ? ';' : ',';
                            Output.Add("        \"" + PinName + "\"" + Terminator);
                            ListedNum++;
                        }
                    }
                    Output.Add("    ");
                }
            }

            Output.Add("    fragment@0{");
            Output.Add("        target = <&am33xx_pinmux>;");
            Output.Add("        __overlay__ {");
            Output.Add("            scarlet_pins: scarlet_pin_set {");
            Output.Add("                pinctrl-single,pins = <");

            lock (GPIOMappings)
            {
                foreach (PinAssignment PinAss in GPIOMappings.Values)
                {
                    string Offset = String.Format("0x{0:X3}", (Pin.GetOffset(PinAss.Pin) - 0x800));
                    string Mode = String.Format("0x{0:X2}", PinAss.Mode);
                    Output.Add("                    " + Offset + " " + Mode);
                }
            }

            Output.Add("                >;");
            Output.Add("            };");
            Output.Add("        };");
            Output.Add("    };");
            Output.Add("    ");
            Output.Add("    fragment@1{");
            Output.Add("        target = <&ocp>;");
            Output.Add("        __overlay__ {");
            Output.Add("            test_helper: helper {");
            Output.Add("                compatible = \"bone-pinmux-helper\";");
            Output.Add("                pinctrl-names = \"default\";");
            Output.Add("                pinctrl-0 = <&scarlet_pins>;");
            Output.Add("                status = \"okay\";");
            Output.Add("            };");
            Output.Add("        };");
            Output.Add("    };");

            if(PWMMappings != null)
            {
                // TODO: Output PWM fragments.
            }

            Output.Add("};");

            return Output;
        }
    }
}
