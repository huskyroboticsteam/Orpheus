using System;
using System.Collections.Generic;
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

namespace HuskyRobotics.UI
{
    /// <summary>
    /// Interaction logic for MapDisplay.xaml
    /// </summary>
    public partial class MapDisplay : UserControl
    {
        public MapDisplay()
        {
            InitializeComponent();
        }

        public void DisplayMap(string mapSetFile)
        {
            ClearCanvas();
            // load in individual images
            // TODO get the path from the settings
            if (File.Exists(Directory.GetCurrentDirectory() + @"\Images\" + mapSetFile))
            {
                using (StreamReader file = new StreamReader(Directory.GetCurrentDirectory() + @"\Images\" + mapSetFile))
                {
                    string line = file.ReadLine();
                    string[] config = line.Split('|');
                    string[] imgDim = config[0].Split('x');
                    int imgWidth = 1;
                    int imgHeight = 1;
                    Int32.TryParse(imgDim[0], out imgWidth);
                    Int32.TryParse(imgDim[1], out imgHeight);
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] parts = line.Split('|');
                        string[] location = parts[0].Split(',');
                        int x = 0;
                        int y = 0;
                        Int32.TryParse(location[0], out x);
                        Int32.TryParse(location[1], out y);
                        addImage(Directory.GetCurrentDirectory() + @"\Images\" + parts[1] + ".png", x, y, imgWidth, imgHeight);
                    }
                }
            }
        }

        // adds an image to the canvas with the given file location and the coords of where
        // on the canvas it goes
        private void addImage(String location, int x, int y, int width, int height)
        {
            var uri = new Uri(location, UriKind.Absolute);
            var bitmap = new BitmapImage(uri);
            var image = new Image { Source = bitmap, Width = width, Height = height };
            Canvas.SetLeft(image, x * (width - 1));
            Canvas.SetTop(image, y * (height - 1));
            MapCanvas.Children.Add(image);
            allImages.Add(image);
        }

        // removes all images from the map display canvas
        private void ClearCanvas()
        {
            MapCanvas.Children.Clear();
            allImages.Clear();
        }

        private Point mousePosition;
        private List<Image> allImages = new List<Image>();
        private bool dragging = false;

        private void CanvasMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            dragging = true;
            mousePosition = e.GetPosition(MapCanvas);
            e.MouseDevice.Capture(MapCanvas);
        }

        private void CanvasMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            dragging = false;
            e.MouseDevice.Capture(null); // Release capture
        }

        private void CanvasMouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                var position = e.GetPosition(MapCanvas);
                var offset = position - mousePosition;
                mousePosition = position;
                for (int i = 0; i < allImages.Count; i++)
                {
                    Canvas.SetLeft(allImages[i], Canvas.GetLeft(allImages[i]) + offset.X);
                    Canvas.SetTop(allImages[i], Canvas.GetTop(allImages[i]) + offset.Y);
                }

            }
        }

        private void CanvasMouseWheel(object sender, MouseWheelEventArgs e)
        {
            double scale = Math.Pow(1.5, -e.Delta/20.0);
            var position = e.GetPosition(MapCanvas);
            var matrix = MapCanvas.RenderTransform.Value;
            matrix.ScaleAtPrepend(scale, scale, position.X, position.Y);
            MapCanvas.RenderTransform = new MatrixTransform(matrix);
        }
    }
}
