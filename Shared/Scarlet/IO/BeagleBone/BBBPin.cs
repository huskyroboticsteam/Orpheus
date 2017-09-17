using BBBCSIO;
using System;
using System.Collections.Generic;

namespace Scarlet.IO.BeagleBone
{
    public enum BBBPin
    {
        P8_03, P8_04, P8_05, P8_06, P8_07, P8_08, P8_09, P8_10, P8_11, P8_12, P8_13, P8_14, P8_15, P8_16, P8_17, P8_18, P8_19, P8_20, P8_21, P8_22, P8_23, P8_24, P8_25, P8_26, P8_27, P8_28, P8_29, P8_30, P8_31, P8_32, P8_33, P8_34, P8_35, P8_36, P8_37, P8_38, P8_39, P8_40, P8_41, P8_42, P8_43, P8_44, P8_45, P8_46,
        P9_11, P9_12, P9_13, P9_14, P9_15, P9_16, P9_17, P9_18, P9_19, P9_20, P9_21, P9_22, P9_23, P9_24, P9_25, P9_26, P9_27, P9_28, P9_29, P9_30, P9_31, P9_33, P9_35, P9_36, P9_37, P9_38, P9_39, P9_40, P9_41, P9_42,
        NONE
    }

    public enum BBBPinMode
    {
        NONE, GPIO, PWM, ADC, I2C, SPI, UART
    }

    static class Pin
    {
        private static Dictionary<BBBPin, PinData> PinInfo = new Dictionary<BBBPin, PinData>()
        {
            { BBBPin.P8_03, new PinData(0x818, 6, 38, GpioEnum.GPIO_38, 1) },
            { BBBPin.P8_04, new PinData(0x81C, 7, 39, GpioEnum.GPIO_39, 1) },
            { BBBPin.P8_05, new PinData(0x808, 2, 34, GpioEnum.GPIO_34, 1) },
            { BBBPin.P8_06, new PinData(0x80C, 3, 35, GpioEnum.GPIO_35, 1) },
            { BBBPin.P8_07, new PinData(0x890, 36, 66, GpioEnum.GPIO_66, 0) },
            { BBBPin.P8_08, new PinData(0x894, 37, 67, GpioEnum.GPIO_67, 0) },
            { BBBPin.P8_09, new PinData(0x89C, 39, 69, GpioEnum.GPIO_69, 0) },
            { BBBPin.P8_10, new PinData(0x898, 38, 68, GpioEnum.GPIO_68, 0) },
            { BBBPin.P8_11, new PinData(0x834, 13, 45, GpioEnum.GPIO_45, 0) },
            { BBBPin.P8_12, new PinData(0x830, 12, 44, GpioEnum.GPIO_44, 0) },
            { BBBPin.P8_13, new PinData(0x824, 9, 23, GpioEnum.GPIO_23, 0) },
            { BBBPin.P8_14, new PinData(0x828, 10, 26, GpioEnum.GPIO_26, 0) },
            { BBBPin.P8_15, new PinData(0x83C, 15, 47, GpioEnum.GPIO_47, 0) },
            { BBBPin.P8_16, new PinData(0x838, 14, 46, GpioEnum.GPIO_46, 0) },
            { BBBPin.P8_17, new PinData(0x82C, 11, 27, GpioEnum.GPIO_27, 0) },
            { BBBPin.P8_18, new PinData(0x88C, 35, 65, GpioEnum.GPIO_65, 0) },
            { BBBPin.P8_19, new PinData(0x820, 8, 22, GpioEnum.GPIO_22, 0) },
            { BBBPin.P8_20, new PinData(0x884, 33, 63, GpioEnum.GPIO_63, 1) },
            { BBBPin.P8_21, new PinData(0x880, 32, 62, GpioEnum.GPIO_62, 1) },
            { BBBPin.P8_22, new PinData(0x814, 5, 37, GpioEnum.GPIO_37, 1) },
            { BBBPin.P8_23, new PinData(0x810, 4, 36, GpioEnum.GPIO_36, 1) },
            { BBBPin.P8_24, new PinData(0x804, 1, 33, GpioEnum.GPIO_33, 1) },
            { BBBPin.P8_25, new PinData(0x800, 0, 32, GpioEnum.GPIO_32, 1) },
            { BBBPin.P8_26, new PinData(0x87C, 31, 61, GpioEnum.GPIO_61, 0) },
            { BBBPin.P8_27, new PinData(0x8E0, 56, 86, GpioEnum.GPIO_86, 2) },
            { BBBPin.P8_28, new PinData(0x8E8, 58, 88, GpioEnum.GPIO_88, 2) },
            { BBBPin.P8_29, new PinData(0x8E4, 57, 87, GpioEnum.GPIO_87, 2) },
            { BBBPin.P8_30, new PinData(0x8EC, 59, 89, GpioEnum.GPIO_89, 2) },
            { BBBPin.P8_31, new PinData(0x8D8, 54, 10, GpioEnum.GPIO_10, 2) },
            { BBBPin.P8_32, new PinData(0x8DC, 55, 11, GpioEnum.GPIO_11, 2) },
            { BBBPin.P8_33, new PinData(0x8D4, 53, 9, GpioEnum.GPIO_9, 2) },
            { BBBPin.P8_34, new PinData(0x8CC, 51, 81, GpioEnum.GPIO_81, 2) },
            { BBBPin.P8_35, new PinData(0x8D0, 52, 8, GpioEnum.GPIO_8, 2) },
            { BBBPin.P8_36, new PinData(0x8C8, 50, 80, GpioEnum.GPIO_80, 2) },
            { BBBPin.P8_37, new PinData(0x8C0, 48, 78, GpioEnum.GPIO_78, 2) },
            { BBBPin.P8_38, new PinData(0x8C4, 49, 79, GpioEnum.GPIO_79, 2) },
            { BBBPin.P8_39, new PinData(0x8B8, 46, 76, GpioEnum.GPIO_76, 2) },
            { BBBPin.P8_40, new PinData(0x8BC, 47, 77, GpioEnum.GPIO_77, 2) },
            { BBBPin.P8_41, new PinData(0x8B0, 44, 74, GpioEnum.GPIO_74, 2) },
            { BBBPin.P8_42, new PinData(0x8B4, 45, 75, GpioEnum.GPIO_75, 2) },
            { BBBPin.P8_43, new PinData(0x8A8, 42, 72, GpioEnum.GPIO_72, 2) },
            { BBBPin.P8_44, new PinData(0x8AC, 43, 73, GpioEnum.GPIO_73, 2) },
            { BBBPin.P8_45, new PinData(0x8A0, 40, 70, GpioEnum.GPIO_70, 2) },
            { BBBPin.P8_46, new PinData(0x8A4, 41, 71, GpioEnum.GPIO_71, 2) },

            { BBBPin.P9_11, new PinData(0x870, 28, 30, GpioEnum.GPIO_30, 0) },
            { BBBPin.P9_12, new PinData(0x878, 30, 60, GpioEnum.GPIO_60, 0) },
            { BBBPin.P9_13, new PinData(0x874, 29, 31, GpioEnum.GPIO_31, 0) },
            { BBBPin.P9_14, new PinData(0x848, 18, 50, GpioEnum.GPIO_50, 0) },
            { BBBPin.P9_15, new PinData(0x840, 16, 48, GpioEnum.GPIO_48, 0) },
            { BBBPin.P9_16, new PinData(0x84C, 19, 51, GpioEnum.GPIO_51, 0) },
            { BBBPin.P9_17, new PinData(0x95C, 87, 5, GpioEnum.GPIO_5, 0) },
            { BBBPin.P9_18, new PinData(0x958, 86, 4, GpioEnum.GPIO_4, 0) },
            { BBBPin.P9_19, new PinData(0x97C, 95, 13, GpioEnum.GPIO_13, 0) },
            { BBBPin.P9_20, new PinData(0x978, 94, 12, GpioEnum.GPIO_12, 0) },
            { BBBPin.P9_21, new PinData(0x954, 85, 3, GpioEnum.GPIO_3, 0) },
            { BBBPin.P9_22, new PinData(0x950, 84, 2, GpioEnum.GPIO_2, 0) },
            { BBBPin.P9_23, new PinData(0x844, 17, 49, GpioEnum.GPIO_49, 0) },
            { BBBPin.P9_24, new PinData(0x984, 97, 15, GpioEnum.GPIO_15, 0) },
            { BBBPin.P9_25, new PinData(0x9AC, 107, 117, GpioEnum.GPIO_117, 2) },
            { BBBPin.P9_26, new PinData(0x980, 96, 14, GpioEnum.GPIO_14, 0) },
            { BBBPin.P9_27, new PinData(0x9A4, 105, 115, GpioEnum.GPIO_115, 0) },
            { BBBPin.P9_28, new PinData(0x99C, 103, 113, GpioEnum.GPIO_113, 2) },
            { BBBPin.P9_29, new PinData(0x994, 101, 111, GpioEnum.GPIO_111, 2) },
            { BBBPin.P9_30, new PinData(0x998, 102, 112, GpioEnum.GPIO_112, 2) },
            { BBBPin.P9_31, new PinData(0x990, 100, 110, GpioEnum.GPIO_110, 0) },
            //{ BBBPin.P9_33, new PinData() }, // ADC // TODO: See if these have/need addresses.
            //{ BBBPin.P9_35, new PinData() }, // ADC
            //{ BBBPin.P9_36, new PinData() }, // ADC
            //{ BBBPin.P9_37, new PinData() }, // ADC
            //{ BBBPin.P9_38, new PinData() }, // ADC
            //{ BBBPin.P9_39, new PinData() }, // ADC
            //{ BBBPin.P9_40, new PinData() }, // ADC
            { BBBPin.P9_41, new PinData(0x984, 97, 20, GpioEnum.GPIO_20, 0) },
            { BBBPin.P9_42, new PinData(0x964, 89, 7, GpioEnum.GPIO_7, 0) }
        };

        /// <summary>Converts a Scarlet BBBPin to a BBBCSIO GpioEnum.</summary>
        public static GpioEnum PinToGPIO(BBBPin Pin)
        {
            if (PinInfo.ContainsKey(Pin)) { return PinInfo[Pin].GPIO; }
            else { return GpioEnum.GPIO_NONE; }
        }

        /// <summary>Converts a Scarlet BBBPin to a BBBCSIO A2DPinEnum.</summary>
        public static A2DPortEnum PinToA2D(BBBPin Pin)
        {
            switch(Pin)
            {
                case BBBPin.P9_39: return A2DPortEnum.AIN_0;
                case BBBPin.P9_40: return A2DPortEnum.AIN_1;
                case BBBPin.P9_37: return A2DPortEnum.AIN_2;
                case BBBPin.P9_38: return A2DPortEnum.AIN_3;
                case BBBPin.P9_33: return A2DPortEnum.AIN_4;
                case BBBPin.P9_36: return A2DPortEnum.AIN_5;
                case BBBPin.P9_35: return A2DPortEnum.AIN_6;
                default: return A2DPortEnum.AIN_NONE;
            }
        }

        /// <summary>Determines if the given pin can be used in the system mode, or if it used by another device.</summary>
        public static bool CheckPin(BBBPin Pin, SystemMode Mode)
        {
            if (!PinInfo.ContainsKey(Pin)) { return false; }

            if(Mode == SystemMode.DEFAULT) { return PinInfo[Pin].Devices == 0; } // No devices must be present.
            else if(Mode == SystemMode.NO_STORAGE) { return (PinInfo[Pin].Devices & 0b1111_1110) == 0; } // No devices except for eMMC must be present.
            else if(Mode == SystemMode.NO_HDMI) { return (PinInfo[Pin].Devices & 0b1111_1101) == 0; } // No devices except for HDMI must be present.
            return false; // Invalid system mode.
        }

        /// <summary>Returns the memory address offset for each pin. Returns 0x000 if not found or invalid input.</summary>
        internal static int GetOffset(BBBPin Pin)
        {
            if (PinInfo.ContainsKey(Pin)) { return PinInfo[Pin].Offset; }
            else { return 0x000; }
        }

        /// <summary>Creates a 8-bit pin mode from the specified parameters.</summary>
        internal static byte GetPinMode(bool FastSlew, bool EnableReceiver, ResistorState Resistor, byte ModeID)
        {
            // Bit | Function           | Modes
            // ====|====================|=====================
            // 0   | Reserved           | 0
            // 0   | Slew Rate          | 0=Fast, 1=Slow
            // 0   | Receiver Enable    | 0=Disable, 1=Enable
            // 0   | Resistor Selection | 0=Pulldown, 1=Pullup
            // ----|--------------------|---------------------
            // 0   | Resistor Enable    | 0=Enable, 1=Disable
            // 000 | Mux Select         | Value, 0 through 7
            // -----------------------------------------------
            // Source: AM3359 Technical Reference Manual: http://www.ti.com/product/AM3359/technicaldocuments
            // Version P, Page 1512, Section 9.3.1.50, Table 9-60
            // Find by searching "conf_<module>" in other versions.
            byte Output = 0b0000_0000;
            if (!FastSlew) { Output |= 0b0100_0000; }
            if (EnableReceiver) { Output |= 0b0010_0000; }
            if (Resistor == ResistorState.PULL_UP) { Output |= 0b0001_0000; }
            else if(Resistor == ResistorState.NONE) { Output |= 0b0000_1000; }
            if (ModeID >= 0b000 && ModeID <= 0b111) { Output |= ModeID; }
            return Output;
        }

        internal struct PinData
        {
            public int Offset;
            public byte NumLinux;
            public byte NumGPIO;
            public GpioEnum GPIO;
            public byte Devices; // x x x x _ x x <HDMI> <eMMC>

            public PinData(int Offset, byte NumLinux, byte NumGPIO, GpioEnum GPIO, byte Devices)
            {
                this.Offset = Offset;
                this.NumLinux = NumLinux;
                this.NumGPIO = NumGPIO;
                this.GPIO = GPIO;
                this.Devices = Devices;
            }
        }

        /// <summary>Gets the mux mode that needs to be used for the given pin usage. Returns 255 if invalid usage provided.</summary>
        internal static byte GetModeID(BBBPin Pin, BBBPinMode Mode)
        {
            // Definitely not the prettiest code in the world.
            // Source: http://www.ofitselfso.com/BeagleNotes/BeagleboneBlackPinMuxModes.php
            switch (Pin)
            {
                // P8 Header
                case BBBPin.P8_03:
                case BBBPin.P8_04:
                case BBBPin.P8_05:
                case BBBPin.P8_06:
                case BBBPin.P8_07:
                case BBBPin.P8_08:
                case BBBPin.P8_09:
                case BBBPin.P8_10:
                case BBBPin.P8_11:
                case BBBPin.P8_12://
                case BBBPin.P8_14:
                case BBBPin.P8_15:
                case BBBPin.P8_16:
                case BBBPin.P8_17:
                case BBBPin.P8_18://
                case BBBPin.P8_20:
                case BBBPin.P8_21:
                case BBBPin.P8_22:
                case BBBPin.P8_23:
                case BBBPin.P8_24:
                case BBBPin.P8_25:
                case BBBPin.P8_26:
                case BBBPin.P8_27:
                case BBBPin.P8_28:
                case BBBPin.P8_29:
                case BBBPin.P8_30:
                case BBBPin.P8_31:
                case BBBPin.P8_32:
                case BBBPin.P8_33://
                case BBBPin.P8_35://
                case BBBPin.P8_39:
                case BBBPin.P8_40:
                case BBBPin.P8_41:
                case BBBPin.P8_42:
                case BBBPin.P8_43:
                case BBBPin.P8_44:
                    if (Mode == BBBPinMode.GPIO) { return 7; }
                    else { return 255; }

                case BBBPin.P8_13:
                case BBBPin.P8_19:
                    if (Mode == BBBPinMode.PWM) { return 4; }
                    else if (Mode == BBBPinMode.GPIO) { return 7; }
                    else { return 255; }

                case BBBPin.P8_34:
                case BBBPin.P8_36:
                    if (Mode == BBBPinMode.PWM) { return 2; }
                    else if (Mode == BBBPinMode.GPIO) { return 7; }
                    else { return 255; }

                case BBBPin.P8_37:
                case BBBPin.P8_38:
                    if (Mode == BBBPinMode.UART) { return 4; }
                    else if (Mode == BBBPinMode.GPIO) { return 7; }
                    else { return 255; }

                case BBBPin.P8_45:
                case BBBPin.P8_46:
                    if (Mode == BBBPinMode.GPIO) { return 7; }
                    else if (Mode == BBBPinMode.PWM) { return 3; }
                    else { return 255; }


                // P9 Header
                case BBBPin.P9_11:
                case BBBPin.P9_13:
                    if (Mode == BBBPinMode.GPIO) { return 7; }
                    else if (Mode == BBBPinMode.UART) { return 6; }
                    else { return 255; }

                case BBBPin.P9_12:
                case BBBPin.P9_15:
                case BBBPin.P9_23:
                    if (Mode == BBBPinMode.GPIO) { return 7; }
                    else { return 255; }

                case BBBPin.P9_14:
                case BBBPin.P9_16:
                    if (Mode == BBBPinMode.GPIO) { return 7; }
                    else if (Mode == BBBPinMode.PWM) { return 6; }
                    else { return 255; }

                case BBBPin.P9_17:
                case BBBPin.P9_18:
                    if (Mode == BBBPinMode.SPI) { return 0; }
                    else if (Mode == BBBPinMode.I2C) { return 2; }
                    else if (Mode == BBBPinMode.GPIO) { return 7; }
                    else { return 255; }

                case BBBPin.P9_19:
                case BBBPin.P9_20:
                    if (Mode == BBBPinMode.I2C) { return 3; }
                    if (Mode == BBBPinMode.SPI) { return 4; }
                    if (Mode == BBBPinMode.GPIO) { return 7; }
                    else { return 255; }

                case BBBPin.P9_21:
                case BBBPin.P9_22:
                    if (Mode == BBBPinMode.SPI) { return 0; }
                    else if (Mode == BBBPinMode.UART) { return 1; }
                    else if (Mode == BBBPinMode.I2C) { return 2; }
                    else if (Mode == BBBPinMode.PWM) { return 3; }
                    else if (Mode == BBBPinMode.GPIO) { return 7; }
                    else { return 255; }

                case BBBPin.P9_24:
                case BBBPin.P9_26:
                    if (Mode == BBBPinMode.UART) { return 0; }
                    else if (Mode == BBBPinMode.I2C) { return 3; }
                    else if (Mode == BBBPinMode.GPIO) { return 7; }
                    else { return 255; }

                case BBBPin.P9_25:
                case BBBPin.P9_27:
                    if (Mode == BBBPinMode.GPIO) { return 7; }
                    else { return 255; }

                case BBBPin.P9_29:
                case BBBPin.P9_31:
                    if (Mode == BBBPinMode.SPI) { return 3; }
                    else if (Mode == BBBPinMode.PWM) { return 1; }
                    else if (Mode == BBBPinMode.GPIO) { return 7; }
                    else { return 255; }

                case BBBPin.P9_28:
                case BBBPin.P9_30:
                    if (Mode == BBBPinMode.SPI) { return 3; }
                    else if (Mode == BBBPinMode.GPIO) { return 7; }
                    else { return 255; }

                case BBBPin.P9_33:
                case BBBPin.P9_35:
                case BBBPin.P9_36:
                case BBBPin.P9_37:
                case BBBPin.P9_38:
                case BBBPin.P9_39:
                case BBBPin.P9_40:
                    if (Mode == BBBPinMode.ADC) { return 0; }
                    else { return 255; }

                case BBBPin.P9_41:
                    if (Mode == BBBPinMode.GPIO) { return 7; }
                    else { return 255; }

                case BBBPin.P9_42:
                    if (Mode == BBBPinMode.SPI) { return 4; }
                    else if (Mode == BBBPinMode.UART) { return 1; } // TODO: Not sure if this works.
                    else if (Mode == BBBPinMode.GPIO) { return 7; }
                    else { return 255; }
            }
            return 255;
        }

    }

}
