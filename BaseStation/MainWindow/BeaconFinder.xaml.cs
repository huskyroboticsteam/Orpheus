using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HuskyRobotics.BaseStation.Server;

namespace HuskyRobotics.UI
{
    /// <summary>
    /// Interaction logic for SignalStrengthNavigation.xaml
    /// </summary>
    public partial class BeaconFinder : UserControl
    {
        private double MaxVoltage;

        public BeaconFinder()
        {
            BaseServer.RFUpdate += UpdateHeading;
            InitializeComponent();
        }

        private void ResetHeading(object sender, RoutedEventArgs e)
        {
            MaxVoltage = 0;
            RotateTransform transform = new RotateTransform(0, 5, 55);
            Heading.RenderTransform = transform;        }

        private void UpdateHeading(object sender, (double, double) vals)
        {
            if (vals.Item2 > MaxVoltage)
            {
                MaxVoltage = vals.Item2;

                RotateTransform transform = new RotateTransform(vals.Item1, 5, 55);
                Heading.RenderTransform = transform;
            }
        }
    }
}
