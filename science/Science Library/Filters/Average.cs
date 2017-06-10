using System;
using System.Collections.Generic;
using System.Text;
using RoboticsLibrary.Utilities;

namespace RoboticsLibrary.Filters
{
    /// <summary>
    /// The Average filter is intended
    /// for use as an average-gathering
    /// system, using a rolling average
    /// with "roll-length" <param>FilterCount</param>.</summary>
    /// 
    /// 
    /// 
    /// <typeparam name="T">
    /// A type, which must be a numeric.</typeparam>
    public class Average<T> : IFilter<T> where T : IComparable
    {

        public T Output
        {
            private set { this.Output = value; }
            get { return this.Output; }
        } // Filter output

        private dynamic[] AverageArray; // Stored average array
        private dynamic CurSum;         // Current sum of the average array 
        private int FilterCount,        // Size of the average array
                    Index,              // Current index of the average array
                    Iterations;         // Number of iterations in the filter

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FilterCount"></param>
        public Average(int FilterCount = 10)
        {
            if (!UtilMain.IsNumericType(typeof(T)))
            {
                throw new ArgumentException("Cannot create filter of non-numeric type: " + typeof(T).ToString());
            } // We can now assert that T is a numeric type

            this.Output = default(T);
            this.CurSum = 0;
            this.Index = 0;
            this.Iterations = 0;
            this.FilterCount = FilterCount;
            this.AverageArray = new dynamic[this.FilterCount];
            this.InitializeArray(); // Initialize average array to defaults
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Input"></param>
        public void Feed(T Input)
        {
            // Increase number of iterations by 1
            this.Iterations++; 
            // Store input as a dynamic type since we know T is a numeric
            dynamic dynamicInput = Input; 
            // Subtract current array index value from sum
            this.CurSum -= this.AverageArray[this.Index]; 
            // Add current value to sum
            this.CurSum += dynamicInput; 
            // Store curent value in old spot
            this.AverageArray[this.Index] = dynamicInput; 
            // Increment index. Go back to zero if index + 1 == filterCount
            this.Index = (this.Index + 1) % this.FilterCount; // Increment index. Go back to zero if index + 1 == filterCount
            // Divide output by either number of iterations or filter length
            this.Output = this.CurSum / (Math.Min(this.Iterations, this.FilterCount)); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="Rate"></param>
        public void Feed(T Input, T Rate)
        {
            this.Feed(Input); // Average filter is independent of rate
        }

        /// <summary>
        /// Initializes dynamic number array to  
        /// all zeros.</summary>
        private void InitializeArray()
        {
            for (int i = 0; i < this.AverageArray.Length; i++)
            {
                this.AverageArray[i] = 0;
            }
        }
    }
}
