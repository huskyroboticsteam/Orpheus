using System;
using Scarlet.IO.BeagleBone;
namespace MainRover
{
    public static class Pins
    {
        public const BBBPin BNO055_SCL = BBBPin.P9_19;
        public const BBBPin BNO055_SDA = BBBPin.P9_20;

        public const BBBPin MTK3339_RX = BBBPin.P9_11;
        public const BBBPin MTK3339_TX = BBBPin.P9_13;

        public const BBBPin SteeringLimitSwitch = BBBPin.P9_29;
        public const BBBPin SteeringMotor = BBBPin.P9_31;
    }
}
