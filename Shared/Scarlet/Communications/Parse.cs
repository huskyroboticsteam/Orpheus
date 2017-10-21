using System;
using System.Collections.Generic;
using Scarlet.Utilities;

namespace Scarlet.Communications
{
    /// <summary>
    /// Handles packet parsing,
    /// using handlers of incoming message IDs.
    /// </summary>
    public static class Parse
    {

        // Delegate method type for parsing specific packet ids
        public delegate void ParseMethod(Packet Packet);
        // Stored parsing Handlers for all possible message ids
        static readonly Dictionary<byte, Delegate> ParsingHandlers = new Dictionary<byte, Delegate>();

        /// <summary>
        /// Sets the handler for parsing of the appropriate
        /// MessageId.
        /// </summary>
        /// <param name="MessageId">
        /// Message ID for parsing.</param>
        /// <param name="ParseMethod">Method used when incoming packet.
        /// of <c>MessageId</c> is received.</param>
        public static void SetParseHandler(byte MessageId, ParseMethod ParseMethod)
        {
            if (ParsingHandlers.ContainsKey(MessageId))
            {
                Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "Parse Method for Packet ID 0x" + MessageId.ToString("X4") + " overridden.");
            }
            ParsingHandlers[MessageId] = ParseMethod;
        }

        /// <summary>
        /// Appropriately parses incoming message.
        /// </summary>
        /// <param name="NewMessage">Message to parse.</param>
        /// <returns>Whether or not parsing was successful.</returns>
        public static bool ParseMessage(Packet Packet)
        {
            try
            {
                if(Packet.Data.ID != Constants.WATCHDOG_PING || Server.OutputWatchdogDebug) { Log.Output(Log.Severity.DEBUG, Log.Source.NETWORK, "Parsing packet: " + Packet.Data.ToString()); }
                if (!ParsingHandlers.ContainsKey(Packet.Data.ID))
                {
                    Log.Output(Log.Severity.ERROR, Log.Source.NETWORK, "No handler is registered for parsing packet ID " + Packet.Data.ID + "!");
                    return false;
                }
                ParsingHandlers[Packet.Data.ID].DynamicInvoke(Packet);
                return true;
            }
            catch (Exception Except)
            {
                Log.Output(Log.Severity.ERROR, Log.Source.NETWORK, "Failed to invoke handler for incoming message.");
                Log.Exception(Log.Source.NETWORK, Except);
                return false;
            }
        }

    }
}
