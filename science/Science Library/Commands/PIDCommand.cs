using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoboticsLibrary.Controllers;

namespace RoboticsLibrary.Commands
{
    /// <summary>
    /// 
    /// </summary>
    class PIDCommand : Command
    {

        private PID PIDController;

        public PIDCommand(double Kp, double Ki, double Kd)
        {
            this.PIDController = new PID(Kp, Ki, Kd);
        }

        public virtual void Run(double Measurement) { }

        public void SetSetpoint(double Setpoint)
        {
            this.PIDController.SetTarget(Setpoint);
        }
    }
}
