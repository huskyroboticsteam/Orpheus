using System;
using System.Collections.Generic;
using System.Text;
using RoboticsLibrary.Errors;

namespace RoboticsLibrary.Communications
{
    public static class Parse
    {

        // Delegate method type for parsing specific packet ids
        public delegate void ParseMethod(Message NewMessage);
        // Stored parsing Handlers for all possible message ids
        private static ParseMethod[] ParsingHandlers = new ParseMethod[256];

        public static void SetParseMethod(byte MessageId, ParseMethod ParseMethod)
        {
            int Id = (int)MessageId;
            Parse.ParsingHandlers[Id] = ParseMethod;
        }

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
