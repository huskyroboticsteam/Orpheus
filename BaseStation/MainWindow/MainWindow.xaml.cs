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
using HuskyRobotics.Utilities;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace HuskyRobotics.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableDictionary<string, MeasuredValue<double>> Properties { get; } = new MockObservableMap();
        private SettingsFile _settingsFile = new SettingsFile(SETTINGS_PATH);

        private const string SETTINGS_PATH = "settings.xml";

        public Settings Settings { get => _settingsFile.Settings; }

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void ConnectPutty(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Settings.PuttyPath))
            {
                var process = new Process();
                process.StartInfo.FileName = Settings.PuttyPath;
                process.StartInfo.Arguments = "-ssh root@192.168.0.50";
                process.Start();
            }
            else
            {
                MessageBox.Show("Could not find PuTTY. You will need to install putty, or launch it manually\n" +
                        "Looking at: " + Settings.PuttyPath + "\n" + 
                        "Should be pointed to putty.exe");
            }
        }
    }
}
