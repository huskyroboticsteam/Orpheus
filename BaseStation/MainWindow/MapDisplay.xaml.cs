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
            DisplayMap();
        }

        private void DisplayMap()
        {
            // load in individual images
            // var path = Path.Combine(System.Environment.CurrentDirectory, "Images", "map.jpg");
            // TODO get the path from the settings
            MapTileDownloadManager.Configuration config = new MapTileDownloadManager.Configuration
                (new Tuple<double, double>(47.653683, -122.304836), new Tuple<int, int>(300, 300), 19);
            //MapTileDownloadManager.DownloadNewTileSet(new Tuple<int, int>(20,20), config, "test");
            // map tile set is already downloaded. But this is a good example line
            using (StreamReader file = new StreamReader(Directory.GetCurrentDirectory() + @"\Images\test.evm"))
            {
                int index = 0;
                //first line specifies some config of map(not needed for now)
                string line = file.ReadLine();
                while ((line = file.ReadLine()) != null)
                {
                    string[] parts = line.Split('|');
                    string[] location = parts[0].Split(',');
                    var uri = new Uri(Directory.GetCurrentDirectory() + @"\Images\" + parts[1] + ".jpg", UriKind.Absolute);
                    var bitmap = new BitmapImage(uri);
                    var image = new Image { Source = bitmap, Width = 300, Height = 300 };
                    int x = 0;
                    int y = 0;
                    Int32.TryParse(location[0], out x);
                    Int32.TryParse(location[1], out y);
                    Canvas.SetLeft(image, x * 300);
                    Canvas.SetTop(image, y * 300);
                    canvas.Children.Add(image);
                    allImages[index] = image;
                    index++;
                }
            }
        }

        private Point mousePosition;
        private Image[] allImages = new Image[400];
        private bool dragging = false;

        private void CanvasMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            dragging = true;
            mousePosition = e.GetPosition(canvas);
        }

        private void CanvasMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            dragging = false;
        }

        private void CanvasMouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                var position = e.GetPosition(canvas);
                var offset = position - mousePosition;
                mousePosition = position;
                for (int i = 0; i < allImages.Length; i++)
                {
                    Canvas.SetLeft(allImages[i], Canvas.GetLeft(allImages[i]) + offset.X);
                    Canvas.SetTop(allImages[i], Canvas.GetTop(allImages[i]) + offset.Y);
                }

            }
        }
    }
}
