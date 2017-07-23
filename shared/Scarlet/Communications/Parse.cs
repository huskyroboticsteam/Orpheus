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
        public delegate void ParseMethod(Message NewMessage);
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
        public static bool ParseMessage(Message NewMessage)
        {
            try
            {
                Log.Output(Log.Severity.DEBUG, Log.Source.NETWORK, "Parsing packet: " + NewMessage.ToString());
                if (!ParsingHandlers.ContainsKey(NewMessage.ID))
                {
                    Log.Output(Log.Severity.ERROR, Log.Source.NETWORK, "No handler is registered for parsing packet ID " + NewMessage.ID + "!");
                    return false;
                }
                ParsingHandlers[NewMessage.ID].DynamicInvoke(NewMessage);
                return true;
            }
            catch (Exception Except)
            {
                Log.Output(Log.Severity.ERROR, Log.Source.NETWORK, "Failed to invoke handler for incoming message.");
                Log.Exception(Log.Source.NETWORK, Except);
                return false;
            }
        }

        /// <summary>
        /// Appropriately parses incoming packet.
        /// Does the same thing as ParseMessage(Message NewMessage)
        /// </summary>
        /// <param name="NewMessage">Packet to parse.</param>
        public static bool ParseMessage(Packet NewMessage)
        {
            return ParseMessage(NewMessage.Data);
        }

    }
}
