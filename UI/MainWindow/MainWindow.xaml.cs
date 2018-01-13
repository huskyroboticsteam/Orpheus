using System.Windows;
using HuskyRobotics.Utilities;
using System.Diagnostics;
using System.IO;

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
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            string puttyPath = @"C:\Program Files (x86)\PuTTY\putty.exe";

            if (File.Exists(puttyPath)) {
                var process = new Process();
                process.StartInfo.FileName = puttyPath;
                process.StartInfo.Arguments = "-ssh root@192.168.0.50";
                process.Start();
            } else {
                MessageBox.Show("Could not find PuTTY. You will need to install putty, or launch it manually\n" +
                        "Looking at: " + puttyPath);
            }
        }
    }
}
