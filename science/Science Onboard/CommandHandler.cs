using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoboticsLibrary.Commands;

namespace Science
{
    public class CommandHandler
    {
        // Must be public and static for direct parsing access
        public static EmergencyStopListener EMERGENCY_STOP = new EmergencyStopListener();

        public CommandHandler()
        {
            EMERGENCY_STOP.SetEmergencyStopMethod(OnEmergencyStop); // Sets the method to call before Emergency Stop is invoked.
        }

        /// <summary>
        /// * * * IMPORTANT * * *
        /// Emergency stop
        /// delegate implementation.
        /// Stop all motors here,
        /// or any other important
        /// procedure.
        /// </summary>
        /// <returns>
        /// Returns whether or not
        /// to actually commit
        /// an emergenct stop.</returns>
        public static bool OnEmergencyStop()
        {
            return true;
        }

    }

}
