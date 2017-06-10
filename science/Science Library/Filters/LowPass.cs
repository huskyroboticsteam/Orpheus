using System;
using System.Collections.Generic;
using System.Text;
using RoboticsLibrary.Utilities;

namespace RoboticsLibrary.Filters
{
    public class LowPass<T> : IFilter<T> where T : IComparable
    {

        public T Output
        {
            private set { Output = value; }
            get { return Output; }
        }

        public double LPFk
        {
            private set { LPFk = value; }
            get { return LPFk; }
        }

        private T LastValue;

        public LowPass(double LPFk = 0.25)
        {
            if (!UtilMain.IsNumericType(typeof(T)))
            {
                throw new ArgumentException("Cannot create filter of non-numeric type: " + typeof(T).ToString());
            } // We can now assert that T is a numeric

            this.Output = default(T);
            this.LPFk = LPFk;
            this.Reset();
        }

        public void Feed(T Input)
        {
            dynamic _dLastValue = this.LastValue;
            dynamic _dInput = Input;
            dynamic _dOutput = this.LastValue + (_dLastValue - _dInput) * this.LPFk;
            this.Output = _dOutput;
            this.LastValue = Input;
        }

        public void Feed(T Input, T Rate)
        {
            this.Feed(Input); // Low Pass Filter independent of rate
        }

        public void Reset()
        {
            this.LastValue = default(T);
        }

    }
}
