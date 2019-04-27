using SharpDX.XInput;

namespace HuskyRobotics.BaseStation
{
    public static class GamepadFactory
    {
		private static volatile Controller drive, arm;

        public static Controller DriveGamepad {
			get {
				return drive ?? (drive = new Controller(UserIndex.One));
			}
		}

        public static Controller ArmGamepad {
			get {
				return arm ?? (arm = new Controller(UserIndex.Two));
			}
		}
    }
}
