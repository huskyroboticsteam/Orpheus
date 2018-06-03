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
        private static Controller gamepad;

        public static Controller GetDriveGamePad()
        {
            if (gamepad == null)
            {
                gamepad = new Controller(UserIndex.One);
                return gamepad;
            }

            return gamepad;
        }
    }
}
