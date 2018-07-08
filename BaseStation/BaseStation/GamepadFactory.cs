using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuskyRobotics.BaseStation
{
    public static class GamepadFactory
    {
        private static Controller DriveGamepad;
        private static Controller ArmGamepad;

        public static Controller GetDriveGamePad()
        {
            if (DriveGamepad == null)
            {
                DriveGamepad = new Controller(UserIndex.One);
                return DriveGamepad;
            }

            return DriveGamepad;
        }

        public static Controller GetArmGamepad()
        {
            if (ArmGamepad == null)
            {
                ArmGamepad = new Controller(UserIndex.Two);
                return ArmGamepad;
            }

            return ArmGamepad;
        }
    }
}
