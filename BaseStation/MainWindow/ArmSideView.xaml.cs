using HuskyRobotics.Arm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.ComponentModel;

namespace HuskyRobotics.UI
{
    /// <summary>
    /// Interaction logic for SideArmView.xaml
    /// </summary>
    public partial class ArmSideView : UserControl
    {
        public ArmSideView()
        {
            InitializeComponent();

            DataContext = this;
            SizeChanged += Resized;
            PreviewMouseMove += MouseMotion; ;
        }
        
        public ObservableCollection<double> SetpointLocationX { get; private set; }
        public ObservableCollection<double> SetpointLocationY { get; private set; }

        public double Scale { get; } = 1;
        
        private Armature _setpointArmature;
        public Armature SetpointArmature {
            get => _setpointArmature;
            set {
                if (_setpointArmature != null)
                {
                    _setpointArmature.PropertyChanged -= UpdateSetpointArmature;
                }
                _setpointArmature = value;
                _setpointArmature.PropertyChanged += UpdateSetpointArmature;
                SetpointLocationX = new ObservableCollection<double>(new List<double>(new double[_setpointArmature.Params.Length + 1]));
                SetpointLocationY = new ObservableCollection<double>(new List<double>(new double[_setpointArmature.Params.Length + 1]));
                RefreshLocations();
            }
        }
        
        private void RefreshLocations()
        {
            // Canvas is 100x100
            SetpointLocationX[0] = 100 / 2;
            SetpointLocationY[0] = 100 / 2;

            double angleSum = 0.0;
            for (int i = 0; i < SetpointArmature.Params.Length; i++)
            {
                angleSum += SetpointArmature.CurrentPitches[i];
                SetpointLocationX[1 + i] = SetpointLocationX[i] + SetpointArmature.Params[i].Length * Scale * Math.Cos(angleSum);
                SetpointLocationY[1 + i] = SetpointLocationY[i] - SetpointArmature.Params[i].Length * Scale * Math.Sin(angleSum);
            }
        }


        private void MouseMotion(object sender, MouseEventArgs e)
        {
            Point location = e.GetPosition(Canvas);
            location.X = (location.X - 50) / Scale;
            location.Y = (location.Y - 50) / -Scale;

            SetpointArmature.MoveTo((float)location.X, (float)location.Y, 0);
        }

        private void UpdateSetpointArmature(object sender, PropertyChangedEventArgs e)
        {
            RefreshLocations();
        }
        
        private void Resized(object sender, SizeChangedEventArgs e)
        {
            RefreshLocations();
        }
    }
}
