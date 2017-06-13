using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboticsLibrary.Commands
{
    public class EmergencyStopListener : Command
    {

        public static bool Stop;

        protected override void Terminate()
        {
            Environment.Exit(0x0005); // Exit the system with EMERGENCY STOP error code
        }

        protected override bool IsFinished()
        {
            return Stop;
        }

    }
}
