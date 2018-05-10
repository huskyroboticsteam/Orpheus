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

        public abstract string ViewName{ get; set; }

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
        public ObservableCollection<float> SetpointDisplayX { get; private set; }
        
        /// <summary>
        /// Y positions of the setpoint armature in canvas space
        /// </summary>
        public ObservableCollection<float> SetpointDisplayY { get; private set; }
        
        /// <summary>
        /// Scaling factor in inches per canvas pixel
        /// </summary>
        public float ScaleInPerPx { get; } = 1;


        private (float, float) ScreenToModel(Tuple<float, float> input)
        {
            return ((input.Item1 - CANVAS_WIDTH / 2) * ScaleInPerPx, (CANVAS_HEIGHT / 2 - input.Item2) * ScaleInPerPx);
        }
        
        private Tuple<float, float> ModelToScreen((float, float) input)
        {
            return new Tuple<float, float>((CANVAS_WIDTH / 2 + input.Item1 / ScaleInPerPx), (CANVAS_HEIGHT / 2 - input.Item2 / ScaleInPerPx));
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
        protected abstract (float, float) ProjectCanvas((float, float, float) input);

        /// <summary>
        /// Projects a from some 2D plane into the 3D plane in armature space.
        /// 
        /// A simple implementation may simply return (x, y, 0) or do something more complicated.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected abstract (float, float, float) UnprojectCanvas((float, float) input, (float, float, float) setpoint);
        
        private Tuple<float, float> _setpointDisplay;
        public Tuple<float, float> SetpointDisplay {
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
                SetpointDisplayX = new ObservableCollection<float>(new List<float>(new float[_setpointArmature.Params.Length + 1]));
                SetpointDisplayY = new ObservableCollection<float>(new List<float>(new float[_setpointArmature.Params.Length + 1]));
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

                var projectedPoint = ModelToScreen(ProjectCanvas(((float)x, (float)y, (float)z)));
                SetpointDisplayX[i + 1] = projectedPoint.Item1;
                SetpointDisplayY[i + 1] = projectedPoint.Item2;
            }

            SetpointDisplay = ModelToScreen(ProjectCanvas(SetpointArmature.Setpoint));
        }

        private void MouseMotion(object sender, MouseEventArgs e)
        {
            Point location = e.GetPosition(Canvas);

            if (e.LeftButton == MouseButtonState.Pressed && Math.Pow(location.X - SetpointDisplay.Item1, 2) + Math.Pow(location.Y - SetpointDisplay.Item2, 2) < 36)
            {
                SetpointArmature.Setpoint = UnprojectCanvas(ScreenToModel(new Tuple<float, float>((float)location.X, (float)location.Y)), SetpointArmature.Setpoint);
            }
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
