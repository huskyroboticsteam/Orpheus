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

        DataGPS = 0xC0,
        DataMagnetometer = 0xC1,

        EmergencyStop = 0x80,

    }
}
