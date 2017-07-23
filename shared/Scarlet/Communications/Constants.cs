namespace Scarlet.Communications
{
    static class Constants
    {
        #region Communication Defaults
        public const int WATCHDOG_WAIT = 500;  // ms
        public const int WATCHDOG_INTERVAL = 100; // ms
        #endregion

        #region Reserved Packet IDs
        public const int WATCHDOG_PING = 0xF0;
        #endregion

    }

}
