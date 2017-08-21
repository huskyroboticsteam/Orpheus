using BBBCSIO;
using System;

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
        static byte GetModeID(BBBPin Pin, BBBPinMode Mode)
        {
            // Definitely not the prettiest code in the world.
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
    }

}
