using System;
namespace MainRover
{
	public enum PacketID : byte
	{
		AllDriveMotorsRPM = 0x8E,
		SteeringMotorRPM = 0x8F,
		FrontRightRPM = 0x90,
		FrontLeftRPM = 0x91,
		BackRightRPM = 0x92,
		BackLeftRPM = 0x93,
		SteerPosition = 0x94,

		GpsCoordinates = 0xC0,
		MagnetometerOrientation = 0xC1,

		EmergencyStop = 0x80,
	}
}
