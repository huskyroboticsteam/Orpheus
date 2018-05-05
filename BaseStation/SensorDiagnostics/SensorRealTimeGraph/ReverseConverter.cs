using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using LiveCharts;

namespace HuskyRobotics.UI
{
    /// <summary>
    /// This class allows for plots to be toggled
    /// Retrieved from lvcharts.net
    /// </summary>
    public class ReverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((SeriesCollection)value).Reverse();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}