using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboticsLibrary.Science
{
    public static class PacketType
    {
        #region Bidirectional (0x00 through 0x7F)
        public const byte WATCHDOG_PING = 0x00;
        public const byte ERROR = 0x01;
        #endregion

        #region Base to Rover (0x80 through 0xBF)
        public const byte EMERGENCY_STOP = 0x80;
        public const byte IMAGE_REQUEST = 0x81;
        #endregion

        #region Rover to Base (0xC0 through 0xFF)
        public const byte SENSOR_READINGS = 0xC0;
        #endregion
    }
}
