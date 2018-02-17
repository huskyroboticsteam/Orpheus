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
            var uri = new System.Uri("F:/GitHub/2017-18/BaseStation/MainWindow/Images/map.jpg");
            var bitmap = new BitmapImage(uri);
            var hi = new BitmapImage(uri);
            var image = new Image { Source = bitmap, Width = 300, Height = 300 };
            var image2 = new Image { Source = bitmap, Width = 300, Height = 300 };
            // put images to together 
            Canvas.SetLeft(image, 0);
            Canvas.SetTop(image, 0);
            Canvas.SetLeft(image2, 300);
            Canvas.SetTop(image2, 300);
            canvas.Children.Add(image);
            canvas.Children.Add(image2);
            allImages[0] = image;
            allImages[1] = image2;
        }

        private Point mousePosition;
        private Image[] allImages = new Image[2];
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
