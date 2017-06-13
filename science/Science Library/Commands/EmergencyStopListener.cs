using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboticsLibrary.Commands
{
    public class EmergencyStopListener : Command
    {

        public static bool STOP;
        public delegate void OnEmergencyStop();
        public Func<bool> EmergenyStopMethod = new Func<bool>(DefaultEmergencyStopMethod);

        protected override void Terminate()
        {
            if(this.EmergenyStopMethod())
            { // Checks if system wants to continue with emergency stop
                Environment.Exit(0x0005); // Exit the system with EMERGENCY STOP error code
            }
        }

        protected override bool IsFinished()
        {
            return STOP;
        }

        public void SetEmergencyStopMethod(Func<bool> NewMethod)
        {
            this.EmergenyStopMethod = NewMethod;
        }
        
        private static bool DefaultEmergencyStopMethod()
        {
            return true;
        }

    }
}
