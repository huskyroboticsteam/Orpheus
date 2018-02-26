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
    public partial class SideArmView : UserControl
    {
        private ObservableCollection<double> SetpointLocationX { get; set; }
        private ObservableCollection<double> SetpointLocationY { get; set; }

        public double Scale { get; } = 5;

        private Armature _setpointArmature;
        public Armature SetpointArmature {
            get => _setpointArmature;
            set {
                if (_setpointArmature != null)
                {
                    _setpointArmature.PropertyChanged -= UpdateSetpoint;
                }
                _setpointArmature = value;
                _setpointArmature.PropertyChanged += UpdateSetpoint;
                SetpointLocationX = new ObservableCollection<double>(new List<double>(new double[_setpointArmature.Params.Length + 1]));
                SetpointLocationY = new ObservableCollection<double>(new List<double>(new double[_setpointArmature.Params.Length + 1]));
                RefreshLocations();
            }
        }

        public SideArmView()
        {
            InitializeComponent();
        }

        private void RefreshLocations()
        {
            SetpointLocationX[0] = Width / 2;
            SetpointLocationY[0] = Height / 2;

            double angleSum = 0.0;
            for(int i = 0; i < SetpointArmature.Params.Length; i++)
            {
                angleSum += SetpointArmature.CurrentPitches[i];
                SetpointLocationX[1 + i] = SetpointLocationX[i] + SetpointArmature.Params[i].Length * Scale * Math.Cos(angleSum);
                SetpointLocationY[1 + i] = SetpointLocationY[i] + SetpointArmature.Params[i].Length * Scale * Math.Sin(angleSum);
            }
        }
        
        private void UpdateSetpoint(object sender, PropertyChangedEventArgs e)
        {
            RefreshLocations();
        }
    }
}
