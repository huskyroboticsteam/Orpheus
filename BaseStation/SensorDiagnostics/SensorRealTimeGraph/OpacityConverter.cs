﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HuskyRobotics.UI
{
    /// <summary>
    /// This class allows for plots to be toggled
    /// Retrieved from lvcharts.net
    /// </summary>
    public class OpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Visibility)value == Visibility.Visible
                ? 1d
                : .2d;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}