using System;
using System.Collections.Generic;
using System.Text;
using RoboticsLibrary.Errors;
using RoboticsLibrary.Utilities;

namespace RoboticsLibrary.Communications
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
        private static Dictionary<int, Delegate> ParsingHandlers = new Dictionary<int, Delegate>();

        /// <summary>
        /// Sets the handler for parsing of the appropriate
        /// MessageId.
        /// </summary>
        /// <param name="MessageId">
        /// Message ID for parsing.</param>
        /// <param name="ParseMethod">Method used when incoming packet
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
        public static void ParseMessage(Message NewMessage)
        {
            try
            {
                ParsingHandlers[NewMessage.ID].DynamicInvoke(NewMessage);
            }
            catch (Exception Except)
            {
                Log.Output(Log.Severity.ERROR, Log.Source.NETWORK, "Failed to invoke handler for incoming message.");
                Log.Exception(Log.Source.NETWORK, Except);
            }
        }

    }
}
