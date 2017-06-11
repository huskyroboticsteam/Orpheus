using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoboticsLibrary.Communications;

namespace RoboticsLibrary.Errors
{
    /// <summary>
    /// Handles:
    ///     - On board error throwing
    ///     - Throwing errors over comms
    ///         - Whether or not error is
    ///           sent over comms is determined
    ///           by the boolean <c>Send</c>.
    /// </summary>
    public static class ErrorHandler
    {

        /// <summary>
        /// Throws and error given an exception.
        /// </summary>
        /// <param name="e">Exception to throw as an error.</param>
        /// <param name="Send">true if Error to be sent over comms.</param>
        public static void Throw(Exception e, bool Send = true)
        {
        }

        /// <summary>
        /// Throws an error given an integer value for the exception.
        /// </summary>
        /// <param name="Error">The integer value of the error to throw</param>
        /// <param name="Send">true if Error to be sent over comms.</param>
        public static void Throw(int Error, bool Send = true)
        {
        }

    }
}
