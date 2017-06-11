using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboticsLibrary.Controllers
{
    /// <summary>
    /// Class built to create a PID control loop in C#
    /// Based off of the PID control algorithm:
    /// https://en.wikipedia.org/wiki/PID_controller 
    /// * * Built for Real-Time Operation * * 
    /// Basic Implementation as follows:
    /// 1) Choose P, I, and D constants in constructor
    /// 2) Set a target value using <c>SetTarget()</c>
    ///     (Units do not matter as long as they are used uniformly in the loop)
    /// 3) Use <c>Feed(...)</c> to run the PID loop given a sensor reading
    ///     (Should be called iteratively, since it depends on system time)
    /// 4) Use <c>YourPIDInstance.Output</c> to return the desired output 
    ///     (Output restrained to +/-1, tune PID coefficients accordingly)
    /// * NOTE: This output will be relative to the units given in the Feed() method.
    /// Map this output to a given range if necessary, for example, in using motors, consider a map to -1 to 1
    /// * NOTE: Nothing has to be done to change the target value during operation, it is all handled
    /// in this class.
    /// * * IMPORTANT * *
    /// A PID Control loop needs to be tuned.
    /// That is, to be used correctly, the P, I, and D
    /// coefficients need to be precisely chosen.
    /// An algorithm for determining coefficients:
    /// https://en.wikipedia.org/wiki/PID_controller#Ziegler.E2.80.93Nichols_method
    /// </summary>
    public class PID
    {

        private Stopwatch StopWatch;                           // Time handled in units of seconds.
        private double Kp, Ki, Kd;                             // PID Coefficients.
        private double Target, LastInput, LastTime, LastError; // Necessary PID Information.
        private double P, I, D;                                // Stored values for last P, I, and D terms.
        public double Output { get; private set; }             // Ouptut of the PID Controller.
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Kp">Proportional Coefficient</param>
        /// <param name="Ki">Integral Coefficient</param>
        /// <param name="Kd">Derivative Coefficient</param>
        public PID(double Kp = 0.0, double Ki = 0.0, double Kd = 0.0)
        {
            this.SetCoefficients(Kp, Ki, Kd);
            this.StopWatch = new Stopwatch();
            this.P = 0.0;
            this.I = 0.0;
            this.D = 0.0;
        }

        /// <summary>
        /// Feeds the PID controller with given input
        /// value. The input is the desired controlled value,
        /// the output is the relative control value to the
        /// control mechanism (e.g. a motor). Tune PID coeficients
        /// accordingly and limit output to desired limits.
        /// </summary>
        /// <param name="Input">
        /// Input to feed the PID controller.</param>
        public void Feed(double Input)
        {
            if (!this.StopWatch.IsRunning) // Start stopwatch first if not already running.
            {
                this.StopWatch.Start();
                // One the first iteration, the dT will be very small, if not, zero.
                this.LastTime = this.StopWatch.ElapsedMilliseconds / 1000.0;
            }
            double Time = this.StopWatch.ElapsedMilliseconds / 1000.0;
            double dT = Time - this.LastTime;
            double Error = this.Target - Input;
            // Compute PID terms
            this.P = this.Kp * Error;
            this.I += this.Ki * Error * dT;
            this.D = this.Kd * ((Error - this.LastError) / dT);
            // Set Last Variables
            this.LastInput = Input;
            this.LastTime = Time;
            this.LastError = Error;
            // Add PID terms
            this.Output = this.P + this.I + this.D;
        }

        /// <summary>
        /// Sets the PID target to a new target.
        /// </summary>
        /// <param name="Target">
        /// New PID target.</param>
        public void SetTarget(double Target)
        {
            this.Target = Target;
        }

        /// <summary>
        /// Sets the PID coefficients.
        /// </summary>
        /// <param name="Kp">Proportional Coefficient</param>
        /// <param name="Ki">Integral Coefficient</param>
        /// <param name="Kd">Derivative Coefficient</param>
        public void SetCoefficients(double Kp, double Ki, double Kd)
        {
            this.Kp = Kp;
            this.Ki = Ki;
            this.Kd = Kd;
        }

        /// <summary>
        /// Resets PID Controller.
        /// </summary>
        public void Reset()
        {
            this.P = 0.0;
            this.I = 0.0;
            this.D = 0.0;
            this.LastInput = 0.0;
            this.LastError = 0.0;
            if (this.StopWatch.IsRunning)
            {
                this.LastTime = this.StopWatch.ElapsedMilliseconds;
            }
        }

    }
}
