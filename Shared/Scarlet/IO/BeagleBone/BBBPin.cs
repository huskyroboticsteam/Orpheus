using BBBCSIO;
using System;
using System.Collections.Generic;

namespace Scarlet.IO.BeagleBone
{
    public enum BBBPin
    {
        P8_03, P8_04, P8_05, P8_06, P8_07, P8_08, P8_09, P8_10, P8_11, P8_12, P8_13, P8_14, P8_15, P8_16, P8_17, P8_18, P8_19, P8_20, P8_21, P8_22, P8_23, P8_24, P8_25, P8_26, P8_27, P8_28, P8_29, P8_30, P8_31, P8_32, P8_33, P8_34, P8_35, P8_36, P8_37, P8_38, P8_39, P8_40, P8_41, P8_42, P8_43, P8_44, P8_45, P8_46,
        P9_11, P9_12, P9_13, P9_14, P9_15, P9_16, P9_17, P9_18, P9_19, P9_20, P9_21, P9_22, P9_23, P9_24, P9_25, P9_26, P9_27, P9_28, P9_29, P9_30, P9_31, P9_32, P9_33, P9_34, P9_35, P9_36, P9_37, P9_38, P9_39, P9_40, P9_41, P9_42
    }

    public enum BBBPinMode
    {
        NONE, GPIO, PWM, ADC, I2C, SPI, UART
    }

    static class Pin
    {
        /// <summary>Converts a Scarlet BBBPin to a BBBCSIO GpioEnum.</summary>
        public static GpioEnum PinToGPIO(BBBPin Pin)
        {
            switch (Pin)
            {
                case BBBPin.P8_03: return GpioEnum.GPIO_38;
                case BBBPin.P8_04: return GpioEnum.GPIO_39;
                case BBBPin.P8_05: return GpioEnum.GPIO_34;
                case BBBPin.P8_06: return GpioEnum.GPIO_35;
                case BBBPin.P8_07: return GpioEnum.GPIO_66;
                case BBBPin.P8_08: return GpioEnum.GPIO_67;
                case BBBPin.P8_09: return GpioEnum.GPIO_69;
                case BBBPin.P8_10: return GpioEnum.GPIO_68;
                case BBBPin.P8_11: return GpioEnum.GPIO_45;
                case BBBPin.P8_12: return GpioEnum.GPIO_44;
                case BBBPin.P8_13: return GpioEnum.GPIO_23;
                case BBBPin.P8_14: return GpioEnum.GPIO_26;
                case BBBPin.P8_15: return GpioEnum.GPIO_47;
                case BBBPin.P8_16: return GpioEnum.GPIO_46;
                case BBBPin.P8_17: return GpioEnum.GPIO_27;
                case BBBPin.P8_18: return GpioEnum.GPIO_65;
                case BBBPin.P8_19: return GpioEnum.GPIO_22;
                case BBBPin.P8_20: return GpioEnum.GPIO_63;
                case BBBPin.P8_21: return GpioEnum.GPIO_62;
                case BBBPin.P8_22: return GpioEnum.GPIO_37;
                case BBBPin.P8_23: return GpioEnum.GPIO_36;
                case BBBPin.P8_24: return GpioEnum.GPIO_33;
                case BBBPin.P8_25: return GpioEnum.GPIO_32;
                case BBBPin.P8_26: return GpioEnum.GPIO_61;
                case BBBPin.P8_27: return GpioEnum.GPIO_86;
                case BBBPin.P8_28: return GpioEnum.GPIO_88;
                case BBBPin.P8_29: return GpioEnum.GPIO_87;
                case BBBPin.P8_30: return GpioEnum.GPIO_89;
                case BBBPin.P8_31: return GpioEnum.GPIO_10;
                case BBBPin.P8_32: return GpioEnum.GPIO_11;
                case BBBPin.P8_33: return GpioEnum.GPIO_9;
                case BBBPin.P8_34: return GpioEnum.GPIO_81;
                case BBBPin.P8_35: return GpioEnum.GPIO_8;
                case BBBPin.P8_36: return GpioEnum.GPIO_80;
                case BBBPin.P8_37: return GpioEnum.GPIO_78;
                case BBBPin.P8_38: return GpioEnum.GPIO_79;
                case BBBPin.P8_39: return GpioEnum.GPIO_76;
                case BBBPin.P8_40: return GpioEnum.GPIO_77;
                case BBBPin.P8_41: return GpioEnum.GPIO_74;
                case BBBPin.P8_42: return GpioEnum.GPIO_75;
                case BBBPin.P8_43: return GpioEnum.GPIO_72;
                case BBBPin.P8_44: return GpioEnum.GPIO_73;
                case BBBPin.P8_45: return GpioEnum.GPIO_70;
                case BBBPin.P8_46: return GpioEnum.GPIO_71;

                case BBBPin.P9_11: return GpioEnum.GPIO_30;
                case BBBPin.P9_12: return GpioEnum.GPIO_60;
                case BBBPin.P9_13: return GpioEnum.GPIO_31;
                case BBBPin.P9_14: return GpioEnum.GPIO_50;
                case BBBPin.P9_15: return GpioEnum.GPIO_48;
                case BBBPin.P9_16: return GpioEnum.GPIO_51;
                case BBBPin.P9_17: return GpioEnum.GPIO_5;
                case BBBPin.P9_18: return GpioEnum.GPIO_4;
                case BBBPin.P9_19: return GpioEnum.GPIO_13;
                case BBBPin.P9_20: return GpioEnum.GPIO_12;
                case BBBPin.P9_21: return GpioEnum.GPIO_3;
                case BBBPin.P9_22: return GpioEnum.GPIO_2;
                case BBBPin.P9_23: return GpioEnum.GPIO_49;
                case BBBPin.P9_24: return GpioEnum.GPIO_15;
                case BBBPin.P9_25: return GpioEnum.GPIO_117;
                case BBBPin.P9_26: return GpioEnum.GPIO_14;
                case BBBPin.P9_27: return GpioEnum.GPIO_115;
                case BBBPin.P9_28: return GpioEnum.GPIO_113;
                case BBBPin.P9_29: return GpioEnum.GPIO_111;
                case BBBPin.P9_30: return GpioEnum.GPIO_112;
                case BBBPin.P9_31: return GpioEnum.GPIO_110;
                case BBBPin.P9_41: return GpioEnum.GPIO_20;
                case BBBPin.P9_42: return GpioEnum.GPIO_7;

                default: return GpioEnum.GPIO_NONE;
            }
        }

        /// <summary>Determines if the given pin can be used in the system mode, or if it used by another device.</summary>
        public static bool CheckPin(BBBPin Pin, SystemMode Mode)
        {
            if(Mode == SystemMode.DEFAULT) // Everything enabled
            {
                switch (Pin)
                {
                    case BBBPin.P8_03:
                    case BBBPin.P8_04:
                    case BBBPin.P8_05:
                    case BBBPin.P8_06:
                    case BBBPin.P8_20:
                    case BBBPin.P8_21:
                    case BBBPin.P8_22:
                    case BBBPin.P8_23:
                    case BBBPin.P8_24:
                    case BBBPin.P8_25:
                    case BBBPin.P8_27:
                    case BBBPin.P8_28:
                    case BBBPin.P8_29:
                    case BBBPin.P8_30:
                    case BBBPin.P8_31:
                    case BBBPin.P8_32:
                    case BBBPin.P8_33:
                    case BBBPin.P8_34:
                    case BBBPin.P8_35:
                    case BBBPin.P8_36:
                    case BBBPin.P8_37:
                    case BBBPin.P8_38:
                    case BBBPin.P8_39:
                    case BBBPin.P8_40:
                    case BBBPin.P8_41:
                    case BBBPin.P8_42:
                    case BBBPin.P8_43:
                    case BBBPin.P8_44:
                    case BBBPin.P8_45:
                    case BBBPin.P8_46:

                    case BBBPin.P9_25:
                    case BBBPin.P9_28:
                    case BBBPin.P9_29:
                    case BBBPin.P9_31:
                        return false;
                }
            }
            else if(Mode == SystemMode.NO_STORAGE) // HDMI Enabled
            {
                switch (Pin)
                {
                    case BBBPin.P8_27:
                    case BBBPin.P8_28:
                    case BBBPin.P8_29:
                    case BBBPin.P8_30:
                    case BBBPin.P8_31:
                    case BBBPin.P8_32:
                    case BBBPin.P8_33:
                    case BBBPin.P8_34:
                    case BBBPin.P8_35:
                    case BBBPin.P8_36:
                    case BBBPin.P8_37:
                    case BBBPin.P8_38:
                    case BBBPin.P8_39:
                    case BBBPin.P8_40:
                    case BBBPin.P8_41:
                    case BBBPin.P8_42:
                    case BBBPin.P8_43:
                    case BBBPin.P8_44:
                    case BBBPin.P8_45:
                    case BBBPin.P8_46:

                    case BBBPin.P9_25:
                    case BBBPin.P9_28:
                    case BBBPin.P9_29:
                    case BBBPin.P9_31:
                        return false;
                }
            }
            else if(Mode == SystemMode.NO_HDMI) // Storage Enabled
            {
                switch (Pin)
                {
                    case BBBPin.P8_03:
                    case BBBPin.P8_04:
                    case BBBPin.P8_05:
                    case BBBPin.P8_06:
                    case BBBPin.P8_20:
                    case BBBPin.P8_21:
                    case BBBPin.P8_22:
                    case BBBPin.P8_23:
                    case BBBPin.P8_24:
                    case BBBPin.P8_25:
                        return false;
                }
            }
            return true; // Nothing Enabled, or pin is not in use.
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
                case BBBPin.P8_45:
                case BBBPin.P8_46:
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
                    if(Mode == BBBPinMode.I2C) { return 3; }
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

                case BBBPin.P9_28:
                case BBBPin.P9_29:
                case BBBPin.P9_31:
                    if (Mode == BBBPinMode.SPI) { return 3; }
                    else if (Mode == BBBPinMode.PWM) { return 1; } // TODO: Not sure if this is correct for P9_28.
                    else if (Mode == BBBPinMode.GPIO) { return 7; }
                    else { return 255; }

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
                    if(Mode == BBBPinMode.ADC) { return 0; }
                    else { return 255; }

                case BBBPin.P9_41:
                    if (Mode == BBBPinMode.GPIO) { return 7; }
                    else { return 255; }

                case BBBPin.P9_42:
                    if (Mode == BBBPinMode.SPI) { return 4; }
                    else if (Mode == BBBPinMode.UART) { return 1; } // TODO: Not sure if this works.
                    else if (Mode == BBBPinMode.PWM) { return 0; } // TODO: Not sure if this works.
                    else if(Mode == BBBPinMode.GPIO) { return 7; }
                    else { return 255; }
            }
            return 255;
        }

        /// <summary>Returns the memory address offset for each pin. Returns 0x000 if not found or invalid input.</summary>
        internal static int GetOffset(BBBPin Pin)
        {
            switch (Pin)
            {
                case BBBPin.P8_03: return 0x818;
                case BBBPin.P8_04: return 0x81C;
                case BBBPin.P8_05: return 0x808;
                case BBBPin.P8_06: return 0x80C;
                case BBBPin.P8_07: return 0x890;
                case BBBPin.P8_08: return 0x894;
                case BBBPin.P8_09: return 0x89C;
                case BBBPin.P8_10: return 0x898;
                case BBBPin.P8_11: return 0x834;
                case BBBPin.P8_12: return 0x830;
                case BBBPin.P8_13: return 0x824;
                case BBBPin.P8_14: return 0x828;
                case BBBPin.P8_15: return 0x83C;
                case BBBPin.P8_16: return 0x838;
                case BBBPin.P8_17: return 0x82C;
                case BBBPin.P8_18: return 0x88C;
                case BBBPin.P8_19: return 0x820;
                case BBBPin.P8_20: return 0x884;
                case BBBPin.P8_21: return 0x880;
                case BBBPin.P8_22: return 0x814;
                case BBBPin.P8_23: return 0x810;
                case BBBPin.P8_24: return 0x804;
                case BBBPin.P8_25: return 0x800;
                case BBBPin.P8_26: return 0x87C;
                case BBBPin.P8_27: return 0x8E0;
                case BBBPin.P8_28: return 0x8E8;
                case BBBPin.P8_29: return 0x8E4;
                case BBBPin.P8_30: return 0x8EC;
                case BBBPin.P8_31: return 0x8D8;
                case BBBPin.P8_32: return 0x8DC;
                case BBBPin.P8_33: return 0x8D4;
                case BBBPin.P8_34: return 0x8CC;
                case BBBPin.P8_35: return 0x8D0;
                case BBBPin.P8_36: return 0x8C8;
                case BBBPin.P8_37: return 0x8C0;
                case BBBPin.P8_38: return 0x8C4;
                case BBBPin.P8_39: return 0x8B8;
                case BBBPin.P8_40: return 0x8BC;
                case BBBPin.P8_41: return 0x8B0;
                case BBBPin.P8_42: return 0x8B4;
                case BBBPin.P8_43: return 0x8A8;
                case BBBPin.P8_44: return 0x8AC;
                case BBBPin.P8_45: return 0x8A0;
                case BBBPin.P8_46: return 0x8A4;

                case BBBPin.P9_11: return 0x870;
                case BBBPin.P9_12: return 0x878;
                case BBBPin.P9_13: return 0x874;
                case BBBPin.P9_14: return 0x848;
                case BBBPin.P9_15: return 0x840;
                case BBBPin.P9_16: return 0x84C;
                case BBBPin.P9_17: return 0x95C;
                case BBBPin.P9_18: return 0x958;
                case BBBPin.P9_19: return 0x97C;
                case BBBPin.P9_20: return 0x978;
                case BBBPin.P9_21: return 0x954;
                case BBBPin.P9_22: return 0x950;
                case BBBPin.P9_23: return 0x844;
                case BBBPin.P9_24: return 0x984;
                case BBBPin.P9_25: return 0x9AC;
                case BBBPin.P9_26: return 0x980;
                case BBBPin.P9_27: return 0x9A4;
                case BBBPin.P9_28: return 0x99C;
                case BBBPin.P9_29: return 0x994;
                case BBBPin.P9_30: return 0x998;
                case BBBPin.P9_31: return 0x990;

                case BBBPin.P9_33:
                case BBBPin.P9_35:
                case BBBPin.P9_36:
                case BBBPin.P9_37:
                case BBBPin.P9_38:
                case BBBPin.P9_39:
                case BBBPin.P9_40:
                    return 0x000; // TODO: Replace this with an actual value?

                case BBBPin.P9_41: return 0x9B4;
                case BBBPin.P9_42: return 0x964;

                default: return 0x000;
            }
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

    }

}
