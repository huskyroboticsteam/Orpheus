using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuskyRobotics.UI
{
    /// <summary>
    /// This class is just used to store the data in the form of a datetime and value to be plotted.
    /// </summary>
    class MeasureModel
    {
        public DateTime DateTime { get; set; } //DateTime tied to data
        public double Value { get; set; } //Value of data
    }
}
