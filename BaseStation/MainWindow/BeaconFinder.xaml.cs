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
            BaseServer.RFUpdate += UpdateVoltage;
            InitializeComponent();
        }

        private void UpdateVoltage(object sender, (double, double) data)
        {
            if (data.Item2 > MaxVoltage) MaxVoltage = data.Item2;
        }

        private void ResetHeading(object sender, RoutedEventArgs e)
        {
            MaxVoltage = 0;
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(this);
            Heading.X2 = position.X;
            Heading.Y2 = position.Y;
        }
    }
}
