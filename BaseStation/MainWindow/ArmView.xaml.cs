using HuskyRobotics.Arm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for ArmView.xaml
    /// </summary>
    public abstract partial class ArmView : UserControl, INotifyPropertyChanged
    {
        protected int CANVAS_WIDTH = 100;
        protected int CANVAS_HEIGHT = 100;

        public ArmView()
        {
            InitializeComponent();

            DataContext = this;
            SizeChanged += Resized;
            PreviewMouseMove += MouseMotion;
        }

        /// <summary>
        /// X positions of the setpoint armature in canvas space
        /// </summary>
        public ObservableCollection<double> SetpointDisplayX { get; private set; }
        
        /// <summary>
        /// Y positions of the setpoint armature in canvas space
        /// </summary>
        public ObservableCollection<double> SetpointDisplayY { get; private set; }
        
        /// <summary>
        /// Scaling factor in inches per canvas pixel
        /// </summary>
        public double ScaleInPerPx { get; } = 1;


        private (double, double) ScreenToModel(Tuple<double, double> input)
        {
            return ((input.Item1 - CANVAS_WIDTH / 2) * ScaleInPerPx, (CANVAS_HEIGHT / 2 - input.Item2) * ScaleInPerPx);
        }
        
        private Tuple<double, double> ModelToScreen((double, double) input)
        {
            return new Tuple<double, double>((CANVAS_WIDTH / 2 + input.Item1 / ScaleInPerPx), (CANVAS_HEIGHT / 2 - input.Item2 / ScaleInPerPx));
        }

        /// <summary>
        /// Projects a point in 3D arm space to some point in a 2D plane. This plane will then be scaled and 
        /// translated to be displayed on the canvas.
        /// 
        /// A simple implementation may simply map z -> x and y -> y.
        /// 
        /// It must be the case that for all in, ProjectCanvas(UnprojectCanvas(in)) == in
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected abstract (double, double) ProjectCanvas((double, double, double) input);

        /// <summary>
        /// Projects a from some 2D plane into the 3D plane in armature space.
        /// 
        /// A simple implementation may simply return (x, y, 0) or do something more complicated.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected abstract (double, double, double) UnprojectCanvas((double, double) input);

        private (double, double, double) _setpoint;
        public (double, double, double) Setpoint {
            get => _setpoint;
            set {
                _setpoint = value;
                SetpointArmature.MoveTo((float)_setpoint.Item1, (float)_setpoint.Item2, (float)_setpoint.Item3);
                
                SetpointDisplay = ModelToScreen(ProjectCanvas(_setpoint));

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Setpoint"));
            }
        }

        private Tuple<double, double> _setpointDisplay;
        public Tuple<double, double> SetpointDisplay {
            get => _setpointDisplay;
            private set {
                _setpointDisplay = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SetpointDisplay"));
            }
        }

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
                SetpointDisplayX = new ObservableCollection<double>(new List<double>(new double[_setpointArmature.Params.Length + 1]));
                SetpointDisplayY = new ObservableCollection<double>(new List<double>(new double[_setpointArmature.Params.Length + 1]));
                RefreshLocations();
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void RefreshLocations()
        {
            double x = 0;
            double y = 0;
            double z = 0;

            SetpointDisplayX[0] = CANVAS_WIDTH / 2;
            SetpointDisplayY[0] = CANVAS_HEIGHT / 2;

            double pitchSum = 0.0;
            double yawSum = 0.0;
            double rollSum = 0.0;
            for (int i = 0; i < SetpointArmature.Params.Length; i++)
            {
                pitchSum += SetpointArmature.CurrentPitches[i];
                yawSum += SetpointArmature.CurrentYaws[i];
                rollSum += SetpointArmature.CurrentRolls[i];

                double segmentLength = SetpointArmature.Params[i].Length;

                x += segmentLength * Math.Cos(pitchSum) * Math.Cos(yawSum);
                if (Math.Abs(rollSum) > 1e-8)
                {
                    y += segmentLength * Math.Sin(rollSum) * Math.Sin(pitchSum);
                    z += segmentLength * Math.Cos(rollSum) * Math.Sin(yawSum);
                }
                else
                {
                    y += segmentLength * Math.Sin(pitchSum);
                    z += segmentLength * Math.Sin(yawSum);
                }

                var projectedPoint = ModelToScreen(ProjectCanvas((x, y, z)));
                SetpointDisplayX[i + 1] = projectedPoint.Item1;
                SetpointDisplayY[i + 1] = projectedPoint.Item2;
            }
        }

        private void MouseMotion(object sender, MouseEventArgs e)
        {
            Point location = e.GetPosition(Canvas);

            Setpoint = UnprojectCanvas(ScreenToModel(new Tuple<double, double>(location.X, location.Y)));
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
