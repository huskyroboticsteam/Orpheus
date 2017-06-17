using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboticsLibrary.Science
{
    public static class PacketType
    {
        public const byte WATCHDOG_PING = 0x00;
        public const byte ERROR = 0x01;
        public const byte EMERGENCY_STOP = 0x02;
    }
}
