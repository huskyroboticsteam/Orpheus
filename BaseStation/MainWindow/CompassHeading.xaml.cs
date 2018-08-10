using HuskyRobotics.BaseStation.Server;
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
    /// Interaction logic for CompassHeading.xaml
    /// </summary>
    public partial class CompassHeading : UserControl
    {
        public CompassHeading()
        {
            InitializeComponent();
            BaseServer.MagnetometerUpdate += UpdateHeading;
        }

        public void UpdateHeading(Object sender, (float, float, float) data)
        {
            double angle = Math.Atan2(data.Item2, data.Item1);
            Transform transform = new RotateTransform(angle, 0, 35);
            Heading.RenderTransform = transform;
        }
    }
}
