using Scarlet.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Scarlet.IO.BeagleBone
{
    public static class BBBPinManager
    {
        private static Dictionary<BBBPin, PinAssignment> GPIOMappings, PWMMappings, I2CMappings, SPIMappings;
        private static Dictionary<BBBPin, int> ADCMappings;
        private static bool EnableI2C1, EnableI2C2, EnableSPI0, EnableSPI1, EnablePWM0, EnablePWM1, EnablePWM2;

        #region Adding Mappings
        public static void AddMappingGPIO(BBBPin SelectedPin, bool IsOutput, ResistorState Resistor, bool FastSlew = true)
        {
            byte Mode = Pin.GetModeID(SelectedPin, BBBPinMode.GPIO);
            if (Mode == 255) { throw new InvalidOperationException("This type of output is not supported on this pin."); }
            if (!Pin.CheckPin(SelectedPin, BeagleBone.Peripherals)) { throw new InvalidOperationException("This pin cannot be used without disabling some peripherals first."); }
            if (Pin.GetOffset(SelectedPin) == 0x000) { throw new InvalidOperationException("This pin is not valid for device tree registration."); }

            if (PWMMappings != null && PWMMappings.ContainsKey(SelectedPin)) { throw new InvalidOperationException("This pin is already registered as PWM, cannot also use for GPIO."); }
            if (I2CMappings != null && I2CMappings.ContainsKey(SelectedPin)) { throw new InvalidOperationException("This pin is already registered as I2C, cannot also use for GPIO."); }
            if (SPIMappings != null && SPIMappings.ContainsKey(SelectedPin)) { throw new InvalidOperationException("This pin is already registered as SPI, cannot also use for GPIO."); }

            if (GPIOMappings == null) { GPIOMappings = new Dictionary<BBBPin, PinAssignment>(); }
            PinAssignment NewMap = new PinAssignment(SelectedPin, Pin.GetPinMode(FastSlew, !IsOutput, Resistor, Mode));
            lock(GPIOMappings)
            {
                if (GPIOMappings.ContainsKey(SelectedPin))
                {
                    Log.Output(Log.Severity.WARNING, Log.Source.HARDWAREIO, "Overriding GPIO pin setting. This may mean that you have a pin usage conflict.");
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
            if (I2CMappings != null && I2CMappings.ContainsKey(SelectedPin)) { throw new InvalidOperationException("This pin is already registered as I2C, cannot also use for PWM."); }
            if (SPIMappings != null && SPIMappings.ContainsKey(SelectedPin)) { throw new InvalidOperationException("This pin is already registered as SPI, cannot also use for PWM."); }

            switch(PWMBBB.PinToPWMID(SelectedPin))
            {
                case BBBCSIO.PWMPortEnum.PWM0_A: case BBBCSIO.PWMPortEnum.PWM0_B: EnablePWM0 = true; break;
                case BBBCSIO.PWMPortEnum.PWM1_A: case BBBCSIO.PWMPortEnum.PWM1_B: EnablePWM1 = true; break;
                case BBBCSIO.PWMPortEnum.PWM2_A: case BBBCSIO.PWMPortEnum.PWM2_B: EnablePWM2 = true; break;
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
            else { throw new InvalidOperationException("Given pin is not a possible I2C SCL pin. Check the documentation for pinouts."); }

            if (GPIOMappings != null && GPIOMappings.ContainsKey(ClockPin)) { throw new InvalidOperationException("This pin is already registered as GPIO, cannot also use for I2C Clock."); }
            if (GPIOMappings != null && GPIOMappings.ContainsKey(DataPin)) { throw new InvalidOperationException("This pin is already registered as GPIO, cannot also use for I2C Data."); }
            if (PWMMappings != null && PWMMappings.ContainsKey(ClockPin)) { throw new InvalidOperationException("This pin is already registered as PWM, cannot also use for I2C Clock."); }
            if (PWMMappings != null && PWMMappings.ContainsKey(DataPin)) { throw new InvalidOperationException("This pin is already registered as PWM, cannot also use for I2C Data."); }
            if (SPIMappings != null && SPIMappings.ContainsKey(ClockPin)) { throw new InvalidOperationException("This pin is already registered as SPI, cannot also use for I2C Clock."); }
            if (SPIMappings != null && SPIMappings.ContainsKey(DataPin)) { throw new InvalidOperationException("This pin is already registered as SPI, cannot also use for I2C Data."); }

            if (I2CMappings == null) { I2CMappings = new Dictionary<BBBPin, PinAssignment>(); }
            PinAssignment ClockMap = new PinAssignment(ClockPin, Pin.GetPinMode(false, true, ResistorState.PULL_UP, ClockMode));
            PinAssignment DataMap = new PinAssignment(DataPin, Pin.GetPinMode(false, true, ResistorState.PULL_UP, DataMode));
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

        // Either MISO or MOSI can be BBBPin.NONE if you only need 1-way communication.
        // To add chip select pins, call AddMappingSPI_CS().
        public static void AddMappingsSPI(BBBPin MISO, BBBPin MOSI, BBBPin Clock)
        {
            byte ClockMode = Pin.GetModeID(Clock, BBBPinMode.SPI);
            byte MISOMode = 255;
            byte MOSIMode = 255;
            if (ClockMode == 255) { throw new InvalidOperationException("This pin cannot be used for SPI clock."); }
            if (!Pin.CheckPin(Clock, BeagleBone.Peripherals)) { throw new InvalidOperationException("SPI pin cannot be used without disabling some peripherals first."); }
            if (Pin.GetOffset(Clock) == 0x000) { throw new InvalidOperationException("SPI pin is not valid for device tree registration."); }

            if (MISO == BBBPin.NONE && MOSI == BBBPin.NONE) { throw new InvalidOperationException("You must set either MOSI or MISO, or both."); }

            if (MISO != BBBPin.NONE)
            {
                MISOMode = Pin.GetModeID(MISO, BBBPinMode.SPI);
                if (MISOMode == 255) { throw new InvalidOperationException("This pin cannot be used for SPI MISO."); }
                if (!Pin.CheckPin(MISO, BeagleBone.Peripherals)) { throw new InvalidOperationException("SPI pin cannot be used without disabling some peripherals first."); }
                if (Pin.GetOffset(MISO) == 0x000) { throw new InvalidOperationException("SPI pin is not valid for device tree registration."); }
            }
            if (MOSI != BBBPin.NONE)
            {
                MOSIMode = Pin.GetModeID(MOSI, BBBPinMode.SPI);
                if (MOSIMode == 255) { throw new InvalidOperationException("This pin cannot be used for SPI MOSI."); }
                if (!Pin.CheckPin(MOSI, BeagleBone.Peripherals)) { throw new InvalidOperationException("SPI pin cannot be used without disabling some peripherals first."); }
                if (Pin.GetOffset(MOSI) == 0x000) { throw new InvalidOperationException("SPI pin is not valid for device tree registration."); }
            }

            if (GPIOMappings != null && MISO != BBBPin.NONE && GPIOMappings.ContainsKey(MISO)) { throw new InvalidOperationException("This pin is already registered as GPIO, cannot also use for SPI MISO."); }
            if (GPIOMappings != null && MOSI != BBBPin.NONE && GPIOMappings.ContainsKey(MOSI)) { throw new InvalidOperationException("This pin is already registered as GPIO, cannot also use for SPI MOSI."); }
            if (GPIOMappings != null && GPIOMappings.ContainsKey(Clock)) { throw new InvalidOperationException("This pin is already registered as GPIO, cannot also use for SPI Clock."); }
            if (PWMMappings != null && MISO != BBBPin.NONE && PWMMappings.ContainsKey(MISO)) { throw new InvalidOperationException("This pin is already registered as PWM, cannot also use for SPI MISO."); }
            if (PWMMappings != null && MOSI != BBBPin.NONE && PWMMappings.ContainsKey(MOSI)) { throw new InvalidOperationException("This pin is already registered as PWM, cannot also use for SPI MOSI."); }
            if (PWMMappings != null && PWMMappings.ContainsKey(Clock)) { throw new InvalidOperationException("This pin is already registered as PWM, cannot also use for SPI Clock."); }
            if (I2CMappings != null && MISO != BBBPin.NONE && I2CMappings.ContainsKey(MISO)) { throw new InvalidOperationException("This pin is already registered as I2C, cannot also use for SPI MISO."); }
            if (I2CMappings != null && MOSI != BBBPin.NONE && I2CMappings.ContainsKey(MOSI)) { throw new InvalidOperationException("This pin is already registered as I2C, cannot also use for SPI MOSI."); }
            if (I2CMappings != null && I2CMappings.ContainsKey(Clock)) { throw new InvalidOperationException("This pin is already registered as I2C, cannot also use for SPI Clock."); }

            if (Clock == BBBPin.P9_22) // Port 0
            {
                if (MISO != BBBPin.NONE && MISO != BBBPin.P9_18 && MISO == BBBPin.P9_30) { throw new InvalidOperationException("MISO pin selected is invalid with the selected clock pin. Make sure that they are part of the same SPI port."); }
                if (MOSI != BBBPin.NONE && MOSI != BBBPin.P9_21 && MOSI != BBBPin.P9_29) { throw new InvalidOperationException("MOSI pin selected is invalid with the selected clock pin. Make sure that they are part of the same SPI port."); }
                EnableSPI0 = true;
            }
            else if (Clock == BBBPin.P9_31 || Clock == BBBPin.P9_42) // Port 1
            {
                if (MISO != BBBPin.NONE && MISO != BBBPin.P9_30) { throw new InvalidOperationException("MISO pin selected is invalid with the selected clock pin. Make sure that they are part of the same SPI port."); }
                if (MOSI != BBBPin.NONE && MOSI != BBBPin.P9_29) { throw new InvalidOperationException("MOSI pin selected is invalid with the selected clock pin. Make sure that they are part of the same SPI port."); }
                EnableSPI1 = true;
            }
            else { throw new InvalidOperationException("SPI Clock pin selected is invalid. Make sure MISO, MOSI, and clock are valid selections and part of the same port."); }

            if (SPIMappings == null) { SPIMappings = new Dictionary<BBBPin, PinAssignment>(); }
            PinAssignment ClockMap = new PinAssignment(Clock, Pin.GetPinMode(true, true, ResistorState.PULL_UP, ClockMode));
            PinAssignment MOSIMap = null;
            PinAssignment MISOMap = null;
            if (MOSI != BBBPin.NONE) { MOSIMap = new PinAssignment(MOSI, Pin.GetPinMode(true, false, ResistorState.PULL_UP, MOSIMode)); }
            if (MISO != BBBPin.NONE) { MISOMap = new PinAssignment(MISO, Pin.GetPinMode(true, true, ResistorState.PULL_UP, MISOMode)); }

            lock (SPIMappings)
            {
                if (SPIMappings.ContainsKey(Clock))
                {
                    Log.Output(Log.Severity.WARNING, Log.Source.HARDWAREIO, "Overriding SPI clock pin setting. This may mean that you have a pin usage conflict.");
                    SPIMappings[Clock] = ClockMap;
                }
                else { SPIMappings.Add(Clock, ClockMap); }

                if (MISO != BBBPin.NONE)
                {
                    if (SPIMappings.ContainsKey(MISO))
                    {
                        Log.Output(Log.Severity.WARNING, Log.Source.HARDWAREIO, "Overriding SPI MISO pin setting. This may mean that you have a pin usage conflict.");
                        SPIMappings[MISO] = MISOMap;
                    }
                    else { SPIMappings.Add(MISO, MISOMap); }
                }

                if (MOSI != BBBPin.NONE)
                {
                    if (SPIMappings.ContainsKey(MOSI))
                    {
                        Log.Output(Log.Severity.WARNING, Log.Source.HARDWAREIO, "Overriding SPI MOSI pin setting. This may mean that you have a pin usage conflict.");
                        SPIMappings[MOSI] = MOSIMap;
                    }
                    else { SPIMappings.Add(MOSI, MOSIMap); }
                }
            }
        }

        public static void AddMappingSPI_CS(BBBPin ChipSelect)
        {
            AddMappingGPIO(ChipSelect, true, ResistorState.PULL_UP); // TODO: Switch this to be in SPI overlay section instead of GPIO to make it clear.
        }

        public static void AddMappingADC(BBBPin SelectedPin)
        {
            int ADCNum = -1;
            switch(SelectedPin)
            {
                case BBBPin.P9_39: ADCNum = 0; break;
                case BBBPin.P9_40: ADCNum = 1; break;
                case BBBPin.P9_37: ADCNum = 2; break;
                case BBBPin.P9_38: ADCNum = 3; break;
                case BBBPin.P9_33: ADCNum = 4; break;
                case BBBPin.P9_36: ADCNum = 5; break;
                case BBBPin.P9_35: ADCNum = 6; break;
                default: throw new InvalidOperationException("This pin is not an ADC pin. Cannot be registered for ADC use.");
            }
            if (ADCMappings == null) { ADCMappings = new Dictionary<BBBPin, int>(); }
            lock (ADCMappings)
            {
                if (ADCMappings.ContainsKey(SelectedPin))
                {
                    Log.Output(Log.Severity.WARNING, Log.Source.HARDWAREIO, "Overriding ADC pin setting. This may mean you have a pin usage conflict.");
                    ADCMappings[SelectedPin] = ADCNum;
                }
                else { ADCMappings.Add(SelectedPin, ADCNum); }
            }
        }
        #endregion

        public enum ApplicationMode { NO_CHANGES, APPLY_IF_NONE, REMOVE_AND_APPLY, APPLY_REGARDLESS }

        /// <summary>
        /// Generates the device tree file, compiles it, and instructs the kernel to load the overlay though the cape manager. May take a while.
        /// Currently this can only be done once, as Scarlet does not have a way of removing the existing mappings.
        /// </summary>
        public static void ApplyPinSettings(ApplicationMode Mode)
        {
            // Generate the device tree
            if((GPIOMappings == null || GPIOMappings.Count == 0) &&
               (PWMMappings == null || PWMMappings.Count == 0) &&
               (I2CMappings == null || I2CMappings.Count == 0) &&
               (SPIMappings == null || SPIMappings.Count == 0) &&
               (ADCMappings == null || ADCMappings.Count == 0))
                { Log.Output(Log.Severity.INFO, Log.Source.HARDWAREIO, "No pins defined, skipping device tree application."); return; }
            if (!StateStore.Started) { throw new Exception("Please start the StateStore system first."); }
            string FileName = "Scarlet-DT";
            string PrevNum = StateStore.GetOrCreate("Scarlet-DevTreeNum", "0");
            string PrevHash = StateStore.GetOrCreate("Scarlet-DevTreeHash", "NONE");

            List<string> DeviceTree = GenerateDeviceTree();
            bool New = PrevHash != DeviceTree.GetHashCode().ToString();
            if (New) { StateStore.Set("Scarlet-DevTreeNum", (int.Parse(PrevNum) + 1).ToString()); }
            FileName += StateStore.Get("Scarlet-DevTreeNum");
            StateStore.Set("Scarlet-DevTreeHash", DeviceTree.GetHashCode().ToString());
            StateStore.Save();
            string OutputDTFile = FileName + ".dts";

            // Save the device tree to file
            File.WriteAllLines(OutputDTFile, DeviceTree);

            bool AttemptOverlayChanges = false;
            bool WarnAboutApplication = false;
            switch(Mode)
            {
                case ApplicationMode.APPLY_IF_NONE:
                    AttemptOverlayChanges = FindScarletOverlays().Count == 0;
                    break;
                case ApplicationMode.APPLY_REGARDLESS:
                    AttemptOverlayChanges = true;
                    New = false;
                    break;
                case ApplicationMode.REMOVE_AND_APPLY:
                    RemovePinSettings();
                    AttemptOverlayChanges = true;
                    break;
                case ApplicationMode.NO_CHANGES:
                    if (FindScarletOverlays().Count > 0) { WarnAboutApplication = true; }
                    break;
            }

            if (AttemptOverlayChanges)
            {
                if (New)
                {
                    // Compile the device tree file
                    // Command: dtc -O dtb -o Scarlet-DT-00A0.dtbo -b 0 -@ Scarlet-DT.dts
                    string CompiledDTFile = FileName + "-00A0.dtbo";
                    Process Compile = new Process();
                    Compile.StartInfo.FileName = "dtc";
                    Compile.StartInfo.Arguments = "-O dtb -o \"" + CompiledDTFile + "\" -b 0 -@ \"" + OutputDTFile + "\"";
                    Log.Output(Log.Severity.INFO, Log.Source.HARDWAREIO, "Compiling device tree...");
                    Compile.Start();
                    Compile.WaitForExit();

                    // Remove previous device tree
                    RemovePinSettings();

                    // Copy the compiled file to the firmware folder, removing the existing one
                    // Command: cp Scarlet-DT-00A0.dtbo /lib/firmware
                    if (!File.Exists(CompiledDTFile)) { throw new FileNotFoundException("Failed to get compiled device tree!"); }
                    if (File.Exists("/lib/firmware/" + CompiledDTFile)) { File.Delete("/lib/firmware/" + CompiledDTFile); }
                    File.Copy(CompiledDTFile, "/lib/firmware/" + CompiledDTFile, true);

                    // Delete the compiled tree file in execution folder
                    File.Delete(CompiledDTFile);
                }
                // Apply the device tree
                // Command: echo Scarlet-DT > /sys/devices/platform/bone_capemgr/slots
                try
                {
                    using (StreamWriter SlotWriter = File.AppendText("/sys/devices/platform/bone_capemgr/slots"))
                    {
                        SlotWriter.WriteLine(FileName);
                        SlotWriter.Flush();
                    }
                }
                catch(IOException Exc)
                {
                    Log.Output(Log.Severity.ERROR, Log.Source.HARDWAREIO, "Failed to apply device tree overlay. This is likely caused by a conflict. Please read 'Common Issues' in the documentation.");
                    throw;
                }

                Thread.Sleep(100);
            }
            if(WarnAboutApplication) { Log.Output(Log.Severity.WARNING, Log.Source.HARDWAREIO, "Scarlet device tree overlays have not been applied. Ensure that this is what you intended, otherwise I/O pins may not work as expected."); }

            // Start relevant components.
            I2CBBB.Initialize(EnableI2C1, EnableI2C2);
            SPIBBB.Initialize(EnableSPI0, EnableSPI1);
            PWMBBB.Initialize(EnablePWM0, EnablePWM1, EnablePWM2);
        }

        /// <summary>
        /// Unloads all Scarlet device tree overlays.
        /// NOTE: This has a high probability of causing instability, issues, and possibly even a kernel panic. Only do this if it is really necessary.
        /// </summary>
        private static void RemovePinSettings()
        {
            List<int> ToRemove = FindScarletOverlays();
            StreamWriter SlotManager = File.AppendText("/sys/devices/platform/bone_capemgr/slots");
            // Command: echo -[NUM] > /sys/devices/platform/bone_capemgr/slots
            ToRemove.ForEach(Num => SlotManager.Write('-' + Num + Environment.NewLine));
            SlotManager.Flush();
            SlotManager.Close();
        }

        private static List<int> FindScarletOverlays()
        {
            // Command: cat /sys/devices/platform/bone_capemgr/slots
            string[] Overlays = File.ReadAllLines("/sys/devices/platform/bone_capemgr/slots");
            List<int> ToRemove = new List<int>();
            foreach (string Overlay in Overlays)
            {
                if (Overlay.Contains("Scarlet-DT"))
                {
                    ToRemove.Add(int.Parse(Overlay.Substring(0, Overlay.IndexOf(":"))));
                }
            }
            return ToRemove;
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

        // WARNING: Treacherous territory ahead. This is an extremely complex function that does all the work of actually generating a device tree overlay.
        // Don't read through this unless you really need to :P
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

            Dictionary<BBBPin, PinAssignment> SPIDev0 = new Dictionary<BBBPin, PinAssignment>();
            Dictionary<BBBPin, PinAssignment> SPIDev1 = new Dictionary<BBBPin, PinAssignment>();

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

            if (SPIMappings != null)
            {
                lock (SPIMappings)
                {
                    // Sort SPI pins into devices
                    foreach (KeyValuePair<BBBPin, PinAssignment> Entry in SPIMappings)
                    {
                        switch (Entry.Key)
                        {
                            case BBBPin.P9_22: // 0_CLK
                            case BBBPin.P9_21: // 0_D0
                            case BBBPin.P9_18: // 0_D1
                            case BBBPin.P9_17: // 0_CS0
                                SPIDev0.Add(Entry.Key, Entry.Value); continue;
                            case BBBPin.P9_31: case BBBPin.P9_42: // 1_CLK
                            case BBBPin.P9_29: // 1_D0
                            case BBBPin.P9_30: // 1_D1
                            case BBBPin.P9_20: case BBBPin.P9_28: // 1_CS0
                            case BBBPin.P9_19: // 1_CS1
                                SPIDev1.Add(Entry.Key, Entry.Value); continue;
                        }
                    }

                    // Add SPI pins to the exclusive-use list
                    // TODO: See if this is needed.
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
            if (GPIOMappings != null)
            {
                lock (GPIOMappings)
                {
                    Output.Add("    fragment@0 {");
                    Output.Add("        target = <&am33xx_pinmux>;");
                    Output.Add("        __overlay__ {");
                    Output.Add("            scarlet_pins: scarlet_pin_set {");
                    Output.Add("                pinctrl-single,pins = <");

                    foreach (PinAssignment PinAss in GPIOMappings.Values)
                    {
                        string Offset = String.Format("0x{0:X3}", (Pin.GetOffset(PinAss.Pin) - 0x800));
                        string Mode = String.Format("0x{0:X2}", PinAss.Mode);
                        Output.Add("                    " + Offset + " " + Mode);
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
                }
            }

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
                        Output.Add("            scarlet_pwm0_pins: pinmux_scarlet_pwm0_pins {");
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
                        Output.Add("            scarlet_pwm1_pins: pinmux_scarlet_pwm1_pins {");
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
                        Output.Add("            scarlet_pwm2_pins: pinmux_scarlet_pwm2_pins {");
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
                        Output.Add("            pinctrl-0 = <&scarlet_pwm0_pins>;");
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
                        Output.Add("            pinctrl-0 = <&scarlet_pwm1_pins>;");
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
                        Output.Add("            pinctrl-0 = <&scarlet_pwm2_pins>;");
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

            // Output SPI device fragments
            if (SPIMappings != null)
            {
                lock (SPIMappings)
                {
                    Output.Add("    fragment@4 {");
                    Output.Add("        target = <&am33xx_pinmux>;");
                    Output.Add("        __overlay__ {");
                    if (SPIDev0.Count > 0)
                    {
                        Output.Add("            scarlet_spi0_pins: pinmux_scarlet_spi0_pins {");
                        Output.Add("                pinctrl-single,pins = <");
                        foreach (PinAssignment PinAss in SPIDev0.Values)
                        {
                            string Offset = String.Format("0x{0:X3}", (Pin.GetOffset(PinAss.Pin) - 0x800));
                            string Mode = String.Format("0x{0:X2}", PinAss.Mode);
                            Output.Add("                    " + Offset + " " + Mode);
                        }
                        Output.Add("                >;");
                        Output.Add("            };");
                    }
                    if (SPIDev1.Count > 0)
                    {
                        Output.Add("            scarlet_spi1_pins: pinmux_scarlet_spi1_pins {");
                        Output.Add("                pinctrl-single,pins = <");
                        foreach (PinAssignment PinAss in SPIDev1.Values)
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

                    if (SPIDev0.Count > 0)
                    {
                        Output.Add("    fragment@30 {");
                        Output.Add("        target = <&spi0>;");
                        Output.Add("        __overlay__ {");
                        Output.Add("            status = \"okay\";");
                        Output.Add("            pinctrl-names = \"default\";");
                        Output.Add("            pinctrl-0 = <&scarlet_spi0_pins>;");
                        Output.Add("            ti,pio-mode;");
                        Output.Add("            #address-cells = <1>;");
                        Output.Add("            #size-cells = <0>;");
                        Output.Add("            spidev@0 {");
                        Output.Add("                spi-max-frequency = <24000000>;");
                        Output.Add("                reg = <0>;");
                        Output.Add("                compatible = \"spidev\";");
                        Output.Add("                spi-cpha;");
                        Output.Add("            };");
                        Output.Add("            spidev@1 {");
                        Output.Add("                spi-max-frequency = <24000000>;");
                        Output.Add("                reg = <1>;");
                        Output.Add("                compatible = \"spidev\";");
                        Output.Add("            };");
                        Output.Add("        };");
                        Output.Add("    };");
                        Output.Add("    ");
                    }
                    if (SPIDev1.Count > 0)
                    {
                        Output.Add("    fragment@31 {");
                        Output.Add("        target = <&spi1>;");
                        Output.Add("        __overlay__ {");
                        Output.Add("            status = \"okay\";");
                        Output.Add("            pinctrl-names = \"default\";");
                        Output.Add("            pinctrl-0 = <&scarlet_spi1_pins>;");
                        Output.Add("            ti,pio-mode;");
                        Output.Add("            #address-cells = <1>;");
                        Output.Add("            #size-cells = <0>;");
                        Output.Add("            spidev@2 {");
                        Output.Add("                spi-max-frequency = <24000000>;");
                        Output.Add("                reg = <0>;");
                        Output.Add("                compatible = \"spidev\";");
                        Output.Add("                spi-cpha;");
                        Output.Add("            };");
                        Output.Add("            spidev@3 {");
                        Output.Add("                spi-max-frequency = <24000000>;");
                        Output.Add("                reg = <1>;");
                        Output.Add("                compatible = \"spidev\";");
                        Output.Add("            };");
                        Output.Add("        };");
                        Output.Add("    };");
                        Output.Add("    ");
                    }
                }
            }

            // Output ADC device fragment
            if(ADCMappings != null)
            {
                lock(ADCMappings)
                {
                    SortedList Channels = new SortedList(7);
                    ADCMappings.Values.ToList().ForEach(x => Channels.Add(x, x));

                    string ChannelsOut = "<";
                    for(int i = 0; i < Channels.Count; i++)
                    {
                        ChannelsOut += Channels.GetByIndex(i);
                        if (i + 1 < Channels.Count) { ChannelsOut += " "; }
                    }
                    ChannelsOut += ">";

                    Output.Add("    fragment@5 {");
                    Output.Add("        target = <&tscadc>;");
                    Output.Add("        __overlay__ {");
                    Output.Add("            status = \"okay\";");
                    Output.Add("            adc {");
                    Output.Add("                ti,adc-channels = " + ChannelsOut + ";");
                    Output.Add("                ti,chan-step-avg = <" + UtilMain.RepeatWithSeperator("0x16", " ", Channels.Count) + ">;");
                    Output.Add("                ti,chan-step-opendelay = <" + UtilMain.RepeatWithSeperator("0x98", " ", Channels.Count) + ">;");
                    Output.Add("                ti,chan-step-sampledelay = <" + UtilMain.RepeatWithSeperator("0x0", " ", Channels.Count) + ">;");
                    Output.Add("            };");
                    Output.Add("        };");
                    Output.Add("    };");
                    Output.Add("    ");
                }
            }

            Output.Add("};");
            return Output;
        }
    }
}
