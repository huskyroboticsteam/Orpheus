using System.Windows;
using HuskyRobotics.Utilities;
using System.Diagnostics;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Input;

namespace HuskyRobotics.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public ObservableMap<float> Properties { get; } = new MockObservableMap();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            DisplayMap();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string puttyPath = @"C:\Program Files (x86)\PuTTY\putty.exe";

            if (File.Exists(puttyPath))
            {
                var process = new Process();
                process.StartInfo.FileName = puttyPath;
                process.StartInfo.Arguments = "-ssh root@192.168.0.50";
                process.Start();
            }
            else
            {
                MessageBox.Show("Could not find PuTTY. You will need to install putty, or launch it manually\n" +
                        "Looking at: " + puttyPath);
            }
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

