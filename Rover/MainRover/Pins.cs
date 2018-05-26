using System;
using Scarlet.IO.BeagleBone;
namespace MainRover
{
    public static class Pins
    {
        public const BBBPin BNO055_SCL = BBBPin.P9_17;
        public const BBBPin BNO055_SDA = BBBPin.P9_18;

        public const BBBPin MTK3339_RX = BBBPin.P9_26;
        public const BBBPin MTK3339_TX = BBBPin.P9_24;

        public const BBBPin SteeringLimitSwitchPin = BBBPin.P9_29;
    }
}
