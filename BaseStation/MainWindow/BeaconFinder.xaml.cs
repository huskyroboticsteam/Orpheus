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
            InitializeComponent();
        }

        private void ResetMax(object sender, RoutedEventArgs e)
        {
            MaxVoltage = 0;
        }

        private void UpdateMax(double voltage, double angle)
        {
            if (voltage > MaxVoltage)
            {
                MaxVoltage = voltage;

                RotateTransform transform = new RotateTransform(angle, 5, 55);
                Heading.RenderTransform = transform;
            }
        }
    }
}
