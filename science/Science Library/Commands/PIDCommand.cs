using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using RoboticsLibrary.Controllers;

namespace RoboticsLibrary.Commands
{
    /// <summary>
    /// A Command class with an 
    /// integrated PID controller,
    /// <c>PIDController</c>.
    /// Override Run() and make sure to
    /// run the PIDController as documented
    /// in Controllers/PID.cs within the Run()
    /// method.
    /// See Command.cs for more details for 
    /// command architecture.
    /// </summary>
    class PIDCommand : Command
    {

        private PID PIDController;

        /// <summary>
        /// Sets up the PID command with 
        /// PID coefficients Kp, Ki, and Kd
        /// </summary>
        /// <param name="Kp">Proportional coefficient for PID</param>
        /// <param name="Ki">Integral coefficient for PID</param>
        /// <param name="Kd">Derivative coefficient for PID</param>
        public PIDCommand(double Kp, double Ki, double Kd)
        {
            this.PIDController = new PID(Kp, Ki, Kd);
        }

        /// <summary>
        /// Sets command setpoint. PID will cause the
        /// internal PID controller to be set to target
        /// the given setpoint. Take into consideration
        /// when overriding and utilizing Run().
        /// </summary>
        /// <param name="Setpoint">New internal PID target.</param>
        public void SetSetpoint(double Setpoint)
        {
            this.PIDController.SetTarget(Setpoint);
        }
    }
}
