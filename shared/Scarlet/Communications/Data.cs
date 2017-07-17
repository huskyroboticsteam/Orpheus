using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scarlet.Communications
{
    public static class Data
    {
        #region Communication Defaults
        public const int WATCHDOG_DELAY = 400;  // ms
        #endregion

        #region Reserved Packet IDs
        public const int WATCHDOG_PING = 0xF0;
        #endregion
    }
}
