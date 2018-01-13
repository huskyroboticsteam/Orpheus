using System;
using System.Globalization;
using System.Windows.Data;

namespace HuskyRobotics.UI
{
    /// <summary>
    /// Converts a 0 to 1 value into a 0 to 360 value.
    /// </summary>
    public class ProportionToDegree : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
            => value is float ? (float)value * 360.0f : 0;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
            => value is float ? (float)value / 360.0f : 0;
    }
}
