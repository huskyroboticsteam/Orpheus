using System;
using Scarlet.IO.BeagleBone;
namespace ArmMaster
{
    class Pins
    {
        public const BBBPin BaseRotation = BBBPin.P8_19;
        public const BBBPin BaseRotationDir = BBBPin.P8_08;
        public const BBBPin Shoulder = BBBPin.P9_14;
        public const BBBPin Elbow = BBBPin.P9_16;

        public const BBBPin ShoulderPot = BBBPin.P9_36;
        public const BBBPin ElbowLimitSwitch = BBBPin.P9_41;

        public const BBBPin SlaveTX = BBBPin.P9_24;
        public const BBBPin SlaveRX = BBBPin.P9_26;
    }
}
