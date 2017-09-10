using Scarlet.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Scarlet.IO.BeagleBone
{
    public static class BBBPinManager
    {
        private static Dictionary<BBBPin, PinAssignment> GPIOMappings, PWMMappings, I2CMappings;
        private static bool EnableI2C1, EnableI2C2;

        public static void AddMappingGPIO(BBBPin SelectedPin, bool IsOutput, ResistorState Resistor, bool FastSlew = true)
        {
            byte Mode = Pin.GetModeID(SelectedPin, BBBPinMode.GPIO);
            if (Mode == 255) { throw new InvalidOperationException("This type of output is not supported on this pin."); }
            if (!Pin.CheckPin(SelectedPin, BeagleBone.Peripherals)) { throw new InvalidOperationException("This pin cannot be used without disabling some peripherals first."); }
            if (Pin.GetOffset(SelectedPin) == 0x000) { throw new InvalidOperationException("This pin is not valid for device tree registration. ADC pins do not need to be registered."); }
            if (PWMMappings != null && PWMMappings.ContainsKey(SelectedPin)) { throw new InvalidOperationException("This pin is already registered as PWM, cannot also use for GPIO."); }

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
            if (GPIOMappings != null && GPIOMappings.ContainsKey(SelectedPin)) { throw new InvalidOperationException("This pin is already registered as GPIO, cannot also use for PWM."); }

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

        public static void AddMappingsI2C(BBBPin ClockPin, BBBPin DataPin)
        {
            byte ClockMode = Pin.GetModeID(ClockPin, BBBPinMode.I2C);
            byte DataMode = Pin.GetModeID(DataPin, BBBPinMode.I2C);
            if (ClockMode == 255) { throw new InvalidOperationException("This pin cannot be used for I2C clock line."); }
            if (DataMode == 255) { throw new InvalidOperationException("This pin cannot be used for I2C data line."); }
            if (!Pin.CheckPin(ClockPin, BeagleBone.Peripherals)) { throw new InvalidOperationException("I2C clock pin cannot be used without disabling some peripherals first."); }
            if (!Pin.CheckPin(DataPin, BeagleBone.Peripherals)) { throw new InvalidOperationException("I2C data pin cannot be used without disabling some peripherals first."); }
            if (Pin.GetOffset(ClockPin) == 0x000) { throw new InvalidOperationException("I2C clock pin is not valid for device tree registration."); }
            if (Pin.GetOffset(DataPin) == 0x000) { throw new InvalidOperationException("I2C data pin is not valid for device tree registration."); }

            if (ClockPin == BBBPin.P9_17 || ClockPin == BBBPin.P9_24) // Port 1 SCL
            {
                if(DataPin != BBBPin.P9_18 && DataPin != BBBPin.P9_26) // Not Port 1 SDA
                {
                    throw new InvalidOperationException("I2C SDA pin selected is invalid with the selected SCL pin. Make sure that it is part of the same I2C port.");
                }
            }
            else if (ClockPin == BBBPin.P9_19 || ClockPin == BBBPin.P9_21) // Port 2 SCL
            {
                if(DataPin != BBBPin.P9_20 && DataPin != BBBPin.P9_22) // Not Port 2 SDA
                {
                    throw new InvalidOperationException("I2C SDA pin selected is invalid with the selected SCL pin. Make sure that it is part of the same I2C port.");
                }
            }
            else { throw new InvalidOperationException("Given pin is not a possible I2C SDA pin. Check the documentation for pinouts."); }


            if (GPIOMappings != null && GPIOMappings.ContainsKey(ClockPin)) { throw new InvalidOperationException("I2C clock pin is already registered as GPIO, cannot also use for PWM."); }
            if (GPIOMappings != null && GPIOMappings.ContainsKey(DataPin)) { throw new InvalidOperationException("I2C data pin is already registered as GPIO, cannot also use for PWM."); }
            // TODO check PWM

            if (I2CMappings == null) { I2CMappings = new Dictionary<BBBPin, PinAssignment>(); }
            PinAssignment ClockMap = new PinAssignment(ClockPin, ClockMode);
            PinAssignment DataMap = new PinAssignment(DataPin, DataMode);
            lock (I2CMappings)
            {
                if(I2CMappings.ContainsKey(ClockPin))
                {
                    Log.Output(Log.Severity.WARNING, Log.Source.HARDWAREIO, "Overriding I2C SCL pin setting. This may mean that you have a pin usage conflict.");
                    I2CMappings[ClockPin] = ClockMap;
                }
                else { I2CMappings.Add(ClockPin, ClockMap); }

                if (I2CMappings.ContainsKey(DataPin))
                {
                    Log.Output(Log.Severity.WARNING, Log.Source.HARDWAREIO, "Overriding I2C SDA pin setting. This may mean that you have a pin usage conflict.");
                    I2CMappings[DataPin] = DataMap;
                }
                else { I2CMappings.Add(DataPin, DataMap); }
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
            string FileName = "Scarlet-DT9";
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
            File.WriteAllText("/sys/devices/platform/bone_capemgr/slots", FileName);

            Thread.Sleep(100);

            // Start relevant components.
            I2CBBB.Initialize(EnableI2C1, EnableI2C2);
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
            Output.Add("    version = \"00A0\";");
            Output.Add("    ");

            List<string> ExclusiveUseList = new List<string>();

            Dictionary<BBBPin, PinAssignment> PWMDev0 = new Dictionary<BBBPin, PinAssignment>();
            Dictionary<BBBPin, PinAssignment> PWMDev1 = new Dictionary<BBBPin, PinAssignment>();
            Dictionary<BBBPin, PinAssignment> PWMDev2 = new Dictionary<BBBPin, PinAssignment>();

            Dictionary<BBBPin, PinAssignment> I2CDev1 = new Dictionary<BBBPin, PinAssignment>();
            Dictionary<BBBPin, PinAssignment> I2CDev2 = new Dictionary<BBBPin, PinAssignment>();

            // Build exclusive-use list
            if (PWMMappings != null)
            {
                lock (PWMMappings)
                {
                    // Sort PWM pins into devices
                    foreach (KeyValuePair<BBBPin, PinAssignment> Entry in PWMMappings)
                    {
                        switch (Entry.Key)
                        {
                            case BBBPin.P9_22: case BBBPin.P9_31: // 0_A
                            case BBBPin.P9_21: case BBBPin.P9_29: // 0_B
                                PWMDev0.Add(Entry.Key, Entry.Value); continue;

                            case BBBPin.P9_14: case BBBPin.P8_36: // 1_A
                            case BBBPin.P9_16: case BBBPin.P8_34: // 1_B
                                PWMDev1.Add(Entry.Key, Entry.Value); continue;

                            case BBBPin.P8_19: case BBBPin.P8_45: // 2_A
                            case BBBPin.P8_13: case BBBPin.P8_46: // 2_B
                                PWMDev2.Add(Entry.Key, Entry.Value); continue;
                        }
                    }
                    // Add PWM pins to exclusive-use list
                    if (PWMDev0.Count > 0)
                    {
                        ExclusiveUseList.Add("epwmss0");
                        ExclusiveUseList.Add("ehrpwm0");
                        foreach (PinAssignment OnePin in PWMDev0.Values)
                        {
                            string PinName = OnePin.Pin.ToString("F").Replace('_', '.');
                            ExclusiveUseList.Add(PinName);
                        }
                    }
                    if (PWMDev1.Count > 0)
                    {
                        ExclusiveUseList.Add("epwmss1");
                        ExclusiveUseList.Add("ehrpwm1");
                        foreach (PinAssignment OnePin in PWMDev1.Values)
                        {
                            string PinName = OnePin.Pin.ToString("F").Replace('_', '.');
                            ExclusiveUseList.Add(PinName);
                        }
                    }
                    if (PWMDev2.Count > 0)
                    {
                        ExclusiveUseList.Add("epwmss2");
                        ExclusiveUseList.Add("ehrpwm2");
                        foreach (PinAssignment OnePin in PWMDev2.Values)
                        {
                            string PinName = OnePin.Pin.ToString("F").Replace('_', '.');
                            ExclusiveUseList.Add(PinName);
                        }
                    }
                }
            }

            if (I2CMappings != null)
            {
                lock(I2CMappings)
                {
                    // Sort I2C pins into devices
                    foreach(KeyValuePair<BBBPin, PinAssignment> Entry in I2CMappings)
                    {
                        switch(Entry.Key)
                        {
                            case BBBPin.P9_17: case BBBPin.P9_24: // 1_SCL
                            case BBBPin.P9_18: case BBBPin.P9_26: // 1_SDA
                                I2CDev1.Add(Entry.Key, Entry.Value); continue;

                            case BBBPin.P9_19: case BBBPin.P9_21: // 2_SCL
                            case BBBPin.P9_20: case BBBPin.P9_22: // 2_SDA
                                I2CDev2.Add(Entry.Key, Entry.Value); continue;
                        }
                    }
                    // Add I2C pins to exclusive-use list
                    if(I2CDev1.Count > 0)
                    {
                        ExclusiveUseList.Add("i2c1");
                        foreach (PinAssignment OnePin in I2CDev1.Values)
                        {
                            string PinName = OnePin.Pin.ToString("F").Replace('_', '.');
                            ExclusiveUseList.Add(PinName);
                        }
                        EnableI2C1 = true;
                    }
                    if (I2CDev2.Count > 0)
                    {
                        ExclusiveUseList.Add("i2c2");
                        foreach (PinAssignment OnePin in I2CDev2.Values)
                        {
                            string PinName = OnePin.Pin.ToString("F").Replace('_', '.');
                            ExclusiveUseList.Add(PinName);
                        }
                        EnableI2C2 = true;
                    }
                }
            }

            // Output exclusive-use list
            if(ExclusiveUseList.Count > 0)
            {
                Output.Add("    exclusive-use =");
                for (int i = 0; i < ExclusiveUseList.Count; i++)
                {
                    Output.Add("        \"" + ExclusiveUseList[i] + "\"" + ((i == ExclusiveUseList.Count - 1) ? ';' : ','));
                }
                Output.Add("    ");
            }

            // Output GPIO mappings
            Output.Add("    fragment@0 {");
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
            Output.Add("    fragment@1 {");
            Output.Add("        target = <&ocp>;");
            Output.Add("        __overlay__ {");
            Output.Add("            scarlet_pinmux: scarlet {");
            Output.Add("                compatible = \"bone-pinmux-helper\";");
            Output.Add("                pinctrl-names = \"default\";");
            Output.Add("                pinctrl-0 = <&scarlet_pins>;");
            Output.Add("                status = \"okay\";");
            Output.Add("            };");
            Output.Add("        };");
            Output.Add("    };");
            Output.Add("    ");

            // Output PWM device fragments
            if (PWMMappings != null)
            {
                lock (PWMMappings)
                {
                    Output.Add("    fragment@2 {");
                    Output.Add("        target = <&am33xx_pinmux>;");
                    Output.Add("        __overlay__ {");
                    if (PWMDev0.Count > 0)
                    {
                        Output.Add("            bbb_ehrpwm0_pins: pinmux_bbb_ehrpwm0_pins {");
                        Output.Add("                pinctrl-single,pins = <");
                        foreach (PinAssignment PinAss in PWMDev0.Values)
                        {
                            string Offset = String.Format("0x{0:X3}", (Pin.GetOffset(PinAss.Pin) - 0x800));
                            string Mode = String.Format("0x{0:X2}", PinAss.Mode);
                            Output.Add("                    " + Offset + " " + Mode);
                        }
                        Output.Add("                >;");
                        Output.Add("            };");
                    }
                    if (PWMDev1.Count > 0)
                    {
                        Output.Add("            bbb_ehrpwm1_pins: pinmux_bbb_ehrpwm1_pins {");
                        Output.Add("                pinctrl-single,pins = <");
                        foreach (PinAssignment PinAss in PWMDev1.Values)
                        {
                            string Offset = String.Format("0x{0:X3}", (Pin.GetOffset(PinAss.Pin) - 0x800));
                            string Mode = String.Format("0x{0:X2}", PinAss.Mode);
                            Output.Add("                    " + Offset + " " + Mode);
                        }
                        Output.Add("                >;");
                        Output.Add("            };");
                    }
                    if (PWMDev2.Count > 0)
                    {
                        Output.Add("            bbb_ehrpwm2_pins: pinmux_bbb_ehrpwm2_pins {");
                        Output.Add("                pinctrl-single,pins = <");
                        foreach (PinAssignment PinAss in PWMDev2.Values)
                        {
                            string Offset = String.Format("0x{0:X3}", (Pin.GetOffset(PinAss.Pin) - 0x800));
                            string Mode = String.Format("0x{0:X2}", PinAss.Mode);
                            Output.Add("                    " + Offset + " " + Mode);
                        }
                        Output.Add("                >;");
                        Output.Add("            };");
                    }
                    Output.Add("        };");
                    Output.Add("    };");
                    Output.Add("    ");

                    if (PWMDev0.Count > 0)
                    {
                        Output.Add("    fragment@10 {");
                        Output.Add("        target = <&epwmss0>;");
                        Output.Add("        __overlay__ {");
                        Output.Add("            status = \"okay\";");
                        Output.Add("            pinctrl-names = \"default\";");
                        Output.Add("        };");
                        Output.Add("    };");
                        Output.Add("    fragment@11 {");
                        Output.Add("        target = <&ehrpwm0>;");
                        Output.Add("        __overlay__ {");
                        Output.Add("            status = \"okay\";");
                        Output.Add("            pinctrl-names = \"default\";");
                        Output.Add("            pinctrl-0 = <&bbb_ehrpwm0_pins>;");
                        Output.Add("        };");
                        Output.Add("    };");
                        Output.Add("    ");
                    }
                    if (PWMDev1.Count > 0)
                    {
                        Output.Add("    fragment@12 {");
                        Output.Add("        target = <&epwmss1>;");
                        Output.Add("        __overlay__ {");
                        Output.Add("            status = \"okay\";");
                        Output.Add("            pinctrl-names = \"default\";");
                        Output.Add("        };");
                        Output.Add("    };");
                        Output.Add("    fragment@13 {");
                        Output.Add("        target = <&ehrpwm1>;");
                        Output.Add("        __overlay__ {");
                        Output.Add("            status = \"okay\";");
                        Output.Add("            pinctrl-names = \"default\";");
                        Output.Add("            pinctrl-0 = <&bbb_ehrpwm1_pins>;");
                        Output.Add("        };");
                        Output.Add("    };");
                        Output.Add("    ");
                    }
                    if (PWMDev2.Count > 0)
                    {
                        Output.Add("    fragment@14 {");
                        Output.Add("        target = <&epwmss2>;");
                        Output.Add("        __overlay__ {");
                        Output.Add("            status = \"okay\";");
                        Output.Add("            pinctrl-names = \"default\";");
                        Output.Add("        };");
                        Output.Add("    };");
                        Output.Add("    fragment@15 {");
                        Output.Add("        target = <&ehrpwm2>;");
                        Output.Add("        __overlay__ {");
                        Output.Add("            status = \"okay\";");
                        Output.Add("            pinctrl-names = \"default\";");
                        Output.Add("            pinctrl-0 = <&bbb_ehrpwm2_pins>;");
                        Output.Add("        };");
                        Output.Add("    };");
                        Output.Add("    ");
                    }
                }
            }

            // Output I2C device fragments
            if(I2CMappings != null)
            {
                lock(I2CMappings)
                {
                    Output.Add("    fragment@3 {");
                    Output.Add("        target = <&am33xx_pinmux>;");
                    Output.Add("        __overlay__ {");
                    if (I2CDev1.Count > 0)
                    {
                        Output.Add("            bbb_i2c1_pins: pinmux_bbb_i2c1_pins {");
                        Output.Add("                pinctrl-single,pins = <");
                        foreach (PinAssignment PinAss in I2CDev1.Values)
                        {
                            string Offset = String.Format("0x{0:X3}", (Pin.GetOffset(PinAss.Pin) - 0x800));
                            string Mode = String.Format("0x{0:X2}", PinAss.Mode);
                            Output.Add("                    " + Offset + " " + Mode);
                        }
                        Output.Add("                >;");
                        Output.Add("            };");
                    }
                    if (I2CDev2.Count > 0)
                    {
                        Output.Add("            bbb_i2c2_pins: pinmux_bbb_i2c2_pins {");
                        Output.Add("                pinctrl-single,pins = <");
                        foreach (PinAssignment PinAss in I2CDev2.Values)
                        {
                            string Offset = String.Format("0x{0:X3}", (Pin.GetOffset(PinAss.Pin) - 0x800));
                            string Mode = String.Format("0x{0:X2}", PinAss.Mode);
                            Output.Add("                    " + Offset + " " + Mode);
                        }
                        Output.Add("                >;");
                        Output.Add("            };");
                    }
                    Output.Add("        };");
                    Output.Add("    };");
                    Output.Add("    ");

                    if (I2CDev1.Count > 0)
                    {
                        Output.Add("    fragment@20 {");
                        Output.Add("        target = <&i2c1>;");
                        Output.Add("        __overlay__ {");
                        Output.Add("            status = \"okay\";");
                        Output.Add("            pinctrl-names = \"default\";");
                        Output.Add("            pinctrl-0 = <&bbb_i2c1_pins>;");
                        Output.Add("            clock-frequency = <100000>;"); // TODO: Make this adjustable?
                        Output.Add("            #address-cells = <1>;");
                        Output.Add("            #size-cells = <0>;");
                        Output.Add("        };");
                        Output.Add("    };");
                        Output.Add("    ");
                    }
                    if (I2CDev2.Count > 0)
                    {
                        Output.Add("    fragment@21 {");
                        Output.Add("        target = <&i2c2>;");
                        Output.Add("        __overlay__ {");
                        Output.Add("            status = \"okay\";");
                        Output.Add("            pinctrl-names = \"default\";");
                        Output.Add("            pinctrl-0 = <&bbb_i2c2_pins>;");
                        Output.Add("            clock-frequency = <100000>;"); // TODO: Make this adjustable?
                        Output.Add("            #address-cells = <1>;");
                        Output.Add("            #size-cells = <0>;");
                        Output.Add("        };");
                        Output.Add("    };");
                        Output.Add("    ");
                    }
                }
            }

            Output.Add("};");

            return Output;
        }
    }
}
