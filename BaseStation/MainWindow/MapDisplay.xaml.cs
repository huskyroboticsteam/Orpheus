using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using HuskyRobotics.Utilities;
using System.IO;
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
using System.Text.RegularExpressions;
using HuskyRobotics.BaseStation.Server;

namespace HuskyRobotics.UI
{
    /// <summary>
    /// Interaction logic for MapDisplay.xaml
    /// </summary>
    public partial class MapDisplay : UserControl
    {
        private const double RESET_PADDING = 10;
        private Point mousePosition;
        private List<Image> waypointIcons = new List<Image>();
        private int ImageWidth;
        private int ImageHeight;
        private int CenterPixelX = 0;
        private int CenterPixelY = 0;
        private int Zoom = 0;
        private Matrix _waypointTransformMatrix;
        private BitmapImage _waypointBitmap;
        private BitmapImage _roverIconBitmap;
        private Image RoverIcon;

        private ObservableCollection<Waypoint> _waypoints = new ObservableCollection<Waypoint>();
        public ObservableCollection<Waypoint> Waypoints
        {
            get => _waypoints;
            set
            {
                _waypoints.CollectionChanged -= WaypointsChanged;
                _waypoints = value;
                _waypoints.CollectionChanged += WaypointsChanged;
            }
        }

        public MapDisplay()
        {
            InitializeComponent();
            BaseServer.GPSUpdate += UpdateRoverPosition;
            _roverIconBitmap = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"/Icons/RoverIcon.png", UriKind.Absolute));
            _waypointBitmap = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"/Icons/waypoint.png", UriKind.Absolute));
            RoverIcon = new Image { Source = _roverIconBitmap };
        }

        public void DisplayMap(string mapSetFile)
        {
            ClearCanvas();
            // load in individual images
            // TODO get the path from the settings
            if (File.Exists(Directory.GetCurrentDirectory() + @"\Images\" + mapSetFile))
            {
                String waypointsFile = (mapSetFile.Replace(".map", ".waypoints"));
                using (StreamReader file = new StreamReader(Directory.GetCurrentDirectory() + @"\Images\" + mapSetFile))
                {
                    string line = file.ReadLine();
                    string[] config = line.Split('|');
                    string[] imgDim = config[0].Split('x');
                    string[] centerCoords = config[1].Split(',');

                    // reasonable defaults in case the parsing does fail
                    ImageWidth = 300;
                    ImageHeight = 300;
                    double MapCenterLat = 47.653799;
                    double MapCenterLong = -122.307808;
                    Zoom = 17;

                    if (!Int32.TryParse(config[2], out Zoom))
                        throw new ArithmeticException("Could not parse zoom from file");
                    if (!Int32.TryParse(imgDim[0], out ImageWidth))
                        throw new ArithmeticException("Could not parse image width from file");
                    if (!Int32.TryParse(imgDim[1], out ImageHeight))
                        throw new ArithmeticException("Could not parse image height from file");
                    if (!Double.TryParse(centerCoords[0], out MapCenterLat))
                        throw new ArithmeticException("Could not parse center latitude from file");
                    if (!Double.TryParse(centerCoords[1], out MapCenterLong))
                        throw new ArithmeticException("Could not parse center longitude from file");

                    Tuple<int, int> PixelCoords = MapConversion.LatLongToPixelXY(MapCenterLat, MapCenterLong, Zoom);
                    CenterPixelX = PixelCoords.Item1;
                    CenterPixelY = PixelCoords.Item2;

                    while ((line = file.ReadLine()) != null)
                    {
                        string[] parts = line.Split('|');
                        string[] location = parts[0].Split(',');
                        int x = 0;
                        int y = 0;
                        Int32.TryParse(location[0], out x);
                        Int32.TryParse(location[1], out y);
                        AddImage(Directory.GetCurrentDirectory() + @"\Images\" + parts[1] + ".jpg", x, y, ImageWidth, ImageHeight);
                    }
                }

                WaypointsChanged(null, null); // this loads the waypoints on startup
            }

            UpdateRoverPosition(this, (0, 0));
        }

        // adds an image to the canvas with the given file location and the coords of where
        // on the canvas it goes
        private void AddImage(String location, int x, int y, int width, int height)
        {
            var uri = new Uri(location, UriKind.Absolute);
            var bitmap = new BitmapImage(uri);
            var image = new Image { Source = bitmap, Width = width, Height = height };
            Canvas.SetLeft(image, x * width - (width / 2));
            Canvas.SetTop(image, y * height - (height / 2));
            MapCanvas.Children.Add(image);
        }

        // removes all images from the map display canvas
        private void ClearCanvas()
        {
            MapCanvas.Children.Clear();
        }

        private void UpdateRoverPosition(object sender, (float, float) e)
        {
            Dispatcher.Invoke(() =>
            {
                MapCanvas.Children.Remove(RoverIcon);
                Tuple<int, int> pixelCoords = MapConversion.LatLongToPixelXY(e.Item1, e.Item2, Zoom);
                var transform = new MatrixTransform(_waypointTransformMatrix);
                RoverIcon.RenderTransform = transform;
                Canvas.SetLeft(RoverIcon, pixelCoords.Item1 - CenterPixelX - (_roverIconBitmap.Width / 2));
                Canvas.SetTop(RoverIcon, pixelCoords.Item2 - CenterPixelY - (_roverIconBitmap.Height / 2));
                MapCanvas.Children.Add(RoverIcon);
            });
        }

        private void WaypointsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var oldIcon in waypointIcons)
            {
                MapCanvas.Children.Remove(oldIcon);
            }

            var transform = new MatrixTransform(_waypointTransformMatrix);
            foreach (var waypoint in Waypoints)
            {
                Tuple<int, int> pixelCoords = MapConversion.LatLongToPixelXY(waypoint.Lat, waypoint.Long, Zoom);
                var waypointIcon = new Image { Source = _waypointBitmap };
                waypointIcon.RenderTransform = transform;
                Canvas.SetLeft(waypointIcon, pixelCoords.Item1 - CenterPixelX - (_waypointBitmap.Width / 2));
                Canvas.SetTop(waypointIcon, pixelCoords.Item2 - CenterPixelY - (_waypointBitmap.Height / 2));
                MapCanvas.Children.Add(waypointIcon);
                waypointIcons.Add(waypointIcon);
            }
        }

        private void CanvasMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            mousePosition = e.GetPosition(OuterCanvas);
            e.MouseDevice.Capture(MapCanvas);
        }

        private void CanvasMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.MouseDevice.Capture(null); // Release capture
        }

        private void CanvasMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var position = e.GetPosition(OuterCanvas);
                var offset = position - mousePosition;
                mousePosition = position;

                var matrix = MapCanvas.RenderTransform.Value;
                matrix.Translate(offset.X, offset.Y);
                MapCanvas.RenderTransform = new MatrixTransform(matrix);
            }
        }

        private void CanvasMouseWheel(object sender, MouseWheelEventArgs e)
        {
            double scale = Math.Pow(1.1, -e.Delta / 20.0);
            var position = e.GetPosition(MapCanvas);
            var matrix = MapCanvas.RenderTransform.Value;
            matrix.ScaleAtPrepend(scale, scale, position.X, position.Y);
            MapCanvas.RenderTransform = new MatrixTransform(matrix);

            _waypointTransformMatrix.ScaleAt(1 / scale, 1 / scale, _waypointBitmap.Width / 2, _waypointBitmap.Height / 2);
            var transform = new MatrixTransform(_waypointTransformMatrix);
            foreach (var waypoint in waypointIcons)
            {
                waypoint.RenderTransform = transform;
            }
            RoverIcon.RenderTransform = transform;
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            MapCanvas.RenderTransform = new MatrixTransform();
        }
    }
}
