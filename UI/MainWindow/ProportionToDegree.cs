using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HuskyRobotics.UI
{
    /// <summary>
    /// Converts a 0 to 1 value into a 0 to 360 value.
    /// </summary>
    public class ProportionToDegree : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is float)
            {
                return (float)value * 360.0f;
            }
            return 0;
        }
     
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is float)
            {
                return (float)value / 360.0f;
            }
            return 0;
        }
    }
}
