using System;
namespace MainRover
{
    public enum PacketID : byte
    {
        RPMAllDriveMotors = 0x8E,
        RPMSteeringMotor = 0x8F,
        RPMFrontRight = 0x90,
        RPMFrontLeft = 0x91,
        RPMBackRight = 0x92,
        RPMBackLeft = 0x93,
        SteerPosition = 0x94,
        SpeedAllDriveMotors = 0x95,

        PathingSpeed = 0x96,
        PathingTurnAngle = 0x97,

        BaseSpeed = 0x9A,
        ShoulderSpeed = 0x9B,
        ElbowSpeed = 0x9C,
        WristSpeed = 0x9D,
        DifferentialVert = 0x9E,
        DifferentialRotate = 0x9F,
        HandGrip = 0xA0,
            
        DataGPS = 0xC0,
        DataMagnetometer = 0xC1,
        HeadingFromGPS = 0xC2,

        EmergencyStop = 0x80,

    }
}
