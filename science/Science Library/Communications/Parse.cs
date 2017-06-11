using System;
using System.Collections.Generic;
using System.Text;
using RoboticsLibrary.Errors;

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
        private static ParseMethod[] ParsingHandlers = new ParseMethod[256];

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
            Parse.ParsingHandlers[MessageId] = ParseMethod;
        }

        /// <summary>
        /// Appropriately parses incoming message.
        /// </summary>
        /// <param name="NewMessage">Message to parse.</param>
        public static void ParseMessage(Message NewMessage)
        {
            try
            {
                Parse.ParsingHandlers[NewMessage.Id](NewMessage);
            }
            catch (Exception e)
            {
                ErrorHandler.Throw(e);
            }
        }

    }
}
