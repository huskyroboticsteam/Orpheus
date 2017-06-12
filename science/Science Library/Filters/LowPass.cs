using System;
using System.Collections.Generic;
using System.Text;
using RoboticsLibrary.Utilities;

namespace RoboticsLibrary.Filters
{
    /// <summary>
    /// The Low Pass filter is intended
    /// for use as an average-gathering
    /// system, using a low pass filter
    /// with time constant <c>LPFk</c>.</summary>
    /// 
    /// Implementation Details:
    /// 
    /// *Construct Low Pass filter given a time constant
    ///  <c>LPFk</c>
    /// 
    /// *Iteratively add values into the filter
    ///  using <c>Feed(T Input)</c>
    /// 
    /// *Get the filter output by calling
    ///  <c>YourFilterInstance.Output</c>
    /// 
    /// <typeparam name="T">
    /// A type, which must be a numeric.</typeparam>
    public class LowPass<T> : IFilter<T> where T : IComparable
    {

        public T Output { get; private set; } // Output for the Low Pass Filter
        public double LPFk { get; private set; } // Time constant for the Low Pass Filter

        private T LastValue; // Last filter value (internal use)

        /// <summary>
        /// Constructs a low pass filter with 
        /// time constant <c>LPFk</c>.</summary>
        /// <param name="LPFk">
        /// Low Pass Filter Time Constant.</param>
        public LowPass(double LPFk = 0.25)
        {
            if (!UtilMain.IsNumericType(typeof(T)))
            {
                Log.Output(Log.Severity.ERROR, Log.Source.OTHER, "Low-pass filter cannot be instantiated with non-numeric type.");
                throw new ArgumentException("Cannot create filter of non-numeric type: " + typeof(T).ToString());
            } // We can now assert that T is a numeric

            this.Output = default(T);
            this.LPFk = LPFk;
            this.Reset();
        }

        /// <summary>
        /// Feeds a value into the filter.
        /// </summary>
        /// <param name="Input">
        /// Value to feed into the filter.</param>
        public void Feed(T Input)
        {
            // Store values as dynamic for manipulation
            dynamic _dLastValue = this.LastValue;
            dynamic _dInput = Input;
            // Find output by adding the difference of the output and
            // input and multiplying by the time constant.
            dynamic _dOutput = this.LastValue + (_dLastValue - _dInput) * this.LPFk;
            // Set iterative variables
            this.Output = _dOutput;
            this.LastValue = Input;
        }
        /// <summary>
        /// Feeds filter with specified rate.
        /// Not used for average filter.
        /// </summary>
        /// <param name="Input">
        /// Value to feed into the filer.</param>
        /// <param name="Rate">
        /// Current rate to feed into the filter.</param>
        public void Feed(T Input, T Rate)
        {
            this.Feed(Input); // Low Pass Filter independent of rate.
        }

        /// <summary>
        /// Resets the low pass filter to the
        /// default value of <c>T</c>
        /// </summary>
        public void Reset()
        {
            this.LastValue = default(T); // Set to default T value.
        }

    }
}
