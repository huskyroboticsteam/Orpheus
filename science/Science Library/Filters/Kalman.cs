using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using RoboticsLibrary.Utilities;


namespace RoboticsLibrary.Filters
{
    /// <summary>
    /// This Kalman Filter class is intended to be used for the 
    /// implementation of a single-measurement Kalman Filter.
    /// See https://en.m.wikipedia.org/wiki/Kalman_filter for
    /// details of the construction and operation of a Kalman
    /// filter.</summary>
    /// <typeparam name="T">
    /// A type, which must be a numeric.</typeparam>
    /// 
    /// Basic implementation as follows:
    /// 
    /// For basic prototyping, it is probably best to leave the 
    /// coefficients as their defaults. For tuning, you can change
    /// the coefficients later.
    /// 
    /// Coefficients:
    /// <c>
    /// Q,</c><c>
    /// Qbias, </c><c>
    /// Rmeasure</c>.
    /// 
    /// - Before iterations call <c>Initialize()</c>
    /// - Iteratively call <c> Feed(T input, T rate)</c>
    ///      (This is important because the filter is time-dependent)
    /// - To get the value returned from the filter, call:
    ///      <c> YourKalmanFilterInstance.Ouput </c>
    /// ***** IMPORTANT *****
    ///
    /// " Kalman filters produce the optimal estimate for a linear system. 
    /// As such, a sensor or system must have (or be close to) a linear 
    /// response in order to apply a Kalman filter. "
    /// Quote from: http://robotsforroboticists.com/kalman-filtering/
    public class Kalman<T> : IFilter<T> where T : IComparable
    { 
        public double Rmeasure // Measurement noise variance.
        {
            set { this.Rmeasure = value; }
            get { return this.Rmeasure; }
        } 

        public T Output // Ouput of the filter system.
        {
            private set { this.Output = value; }
            get { return this.Output; }
        } 

        public bool Initialized // Whether or not the system is initialized
        {
            private set { this.Initialized = value; }
            get { return this.Initialized; }
        }

        private double CalcMeasure;  // The output calculated by the Kalman filter.
        private double Bias;         // The rate bias calculated by the Kalman filter.
        private double NewRate;      // New Rate to the Kalman Filter
        private double Y;            // Angle difference
        private double S;            // Estimate error
        private double[,] P;         // Error covariance matrix.
        private double[] K;          // Kalman gain 2x1 vector.
        private T LastRate;          // Last rate recorded by the system
        private Stopwatch StopWatch; // Stopwatch for time-dependency
        private double LastTime;     // Last timestamp (in ms since Initialize()) recorded by the filter
        public double Q;             // Process noise variance for the signal.
        public double Qbias;         // Process noise variance for the signal rate bias.
                
        /// Kalman Filter Constructor
        /// <summary>
        /// Handles construction of the
        /// Kalman filter with default
        /// filter coefficients.
        /// See class comment for more
        /// information regarding 
        /// implementation.</summary>
        public Kalman()
        {
            if (!UtilMain.IsNumericType(typeof(T))) // Can now assert that T is a numeric
            {
                throw new ArgumentException("Cannot create filter of non-numeric type: " + typeof(T).ToString());
            } 

            this.Output = default(T);
            this.LastRate = default(T);
            this.StopWatch = new Stopwatch();
            this.Initialized = false;
            this.Q = 0.001;       // Default Q value (arbitrary), set Q to necessary value
            this.Qbias = 0.003;   // Default Qbias value (arbitrary), set Qbias to necessary value
            this.Rmeasure = 0.03; // Default Rmeasure value (arbitrary), set Rmeasure to necessary value
            this.CalcMeasure = 0.0;
            this.Bias = 0.0;
            this.P = new double[2, 2];
            this.K = new double[2];
            this.P[0, 0] = 0.0; // Since we assume that the bias is 0 and we know the starting position,
            this.P[0, 1] = 0.0; // the error covariance matrix is set like so.
            this.P[1, 0] = 0.0;
            this.P[1, 1] = 0.0;
        }

        /// <summary>
        /// Feeds the Kalman filter with an
        /// input, but only uses the last
        /// supplied rate.</summary>
        /// <param name="Input">
        /// is the new input 
        /// to the Kalman filter.</param>
        public void Feed(T Input)
        {
            // Kalman Filter is rate-dependent
            // Therefore must be given a rate.
            // By default, the last rate
            // fed into the filter will be
            // given or, by default 0.0.
            this.Feed(Input, this.LastRate); 
        }

        /// <summary>
        /// Initializes the Kalman filter.
        /// Call before running iteratively.</summary>
        public void Initialize()
        {
            this.StopWatch.Start();
            this.LastTime = this.StopWatch.Elapsed.TotalSeconds;
            this.Initialized = true;
        }

        /// <summary>
        /// Feeds the Kalman filter with an
        /// input and rate and
        /// calculates the feedback
        /// from the filter.</summary>
        /// <param name="Input">
        /// is the new input to 
        /// the Kalman filter.</param> 
        /// <param name="Rate"> is the
        /// new rate to the Kalman filter.
        /// Also computes filter feedback.
        public void Feed(T Input, T Rate)
        {
            if (!this.Initialized)
            {
                throw new InvalidOperationException("Kalman Filter must be initialized before running, call .Initialize()...");
            }

            double Time = this.StopWatch.Elapsed.TotalSeconds;
            double dt = Time - this.LastTime;
            this.LastTime = Time;
            dynamic RateD = Rate;             // Set parameters to dynamic
            dynamic InputD = Input;           // for generic type manipulation.
            this.NewRate = RateD - this.Bias; // Predictive output measurements
            this.CalcMeasure += dt * RateD;   // for measurement and rate.

            // Update the estimation error covariance.
            this.P[0, 0] += dt * (dt * this.P[1, 1] - this.P[0, 1] - this.P[1, 0] + this.Q);
            this.P[0, 1] -= dt * this.P[1, 1];
            this.P[1, 0] -= dt * this.P[1, 1];
            this.P[1, 1] += dt * this.Qbias;

            // Compute the Kalman gain.
            this.S = this.P[0, 0] + this.Rmeasure;
            this.K[0] = this.P[0, 0] / this.S;
            this.K[1] = this.P[1, 0] / this.S;

            // Calculate angle and bias - update estimate with measurement.
            this.Y = InputD - this.CalcMeasure;
            this.CalcMeasure += this.K[0] * this.Y;
            this.Bias += this.K[1] * this.Y;

            // Update the error covariance.
            this.P[0, 0] -= this.K[0] * this.P[0, 0];
            this.P[0, 1] -= this.K[0] * this.P[0, 1];
            this.P[1, 0] -= this.K[1] * this.P[0, 0];
            this.P[1, 1] -= this.K[1] * this.P[0, 1];
            
            this.LastRate = Rate;
            dynamic SetAngle = this.CalcMeasure;
            this.Output = SetAngle;
        }
    }
}
