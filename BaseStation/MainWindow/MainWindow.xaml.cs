using System.Windows;
using System.Diagnostics;
using System.IO;
using HuskyRobotics.Utilities;
using HuskyRobotics.Arm;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Controls;

namespace HuskyRobotics.UI {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		private const string SETTINGS_PATH = "settings.xml";
		private SettingsFile SettingsFile = new SettingsFile(SETTINGS_PATH);
		public Settings Settings { get => SettingsFile.Settings; }
		public ObservableDictionary<string, MeasuredValue<double>> Properties { get; }
        public Armature SetpointArm;
        public ObservableCollection<Waypoint> Waypoints { get; private set; } = new ObservableCollection<Waypoint>();
        public ObservableCollection<VideoStream> Streams { get; private set; } = new ObservableCollection<VideoStream>();

		public MainWindow()
        {
            Gst.Application.Init();
            Properties = new MockObservableMap();
            InitializeComponent();
            WindowState = WindowState.Maximized;
			DataContext = this;

            double degToRad = Math.PI / 180;
            ArmSideViewer.SetpointArmature =
                new Armature((0, 0, Math.PI / 2, Math.PI / 2, -4 * Math.PI, 4 * Math.PI, 6.8),
                        (0, 0, -76 * degToRad, 100 * degToRad, 0, 0, 28.0),
                        (0, 0, -168.51 * degToRad, -10 * degToRad, 0, 0, 28.0),
                        (0, 12.75, -Math.PI / 2, Math.PI / 2));

            ArmTopViewer.SetpointArmature = ArmSideViewer.SetpointArmature;

            Settings.PropertyChanged += SettingChanged;
            SettingPanel.Settings = Settings;
            if (!Directory.Exists(Settings.CurrentMapFile))
            {
                string[] mapFiles = Directory.GetFiles
                    (Directory.GetCurrentDirectory() + @"\Images", "*.map");
                if (mapFiles.Count() != 0)
                {
                    Settings.CurrentMapFile = Path.GetFileName(mapFiles[0]);
                }
            }
            Map.DisplayMap(Settings.CurrentMapFile);
            Map.Waypoints = Waypoints;
            ArmSideViewer.ViewLabel.Content = "Arm Side";
            ArmTopViewer.ViewLabel.Content = "Arm Top";
        }

        private void SettingChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("CurrentMapFile"))
            {
                Map.DisplayMap(Settings.CurrentMapFile);
            }
        }

        private void PuTTY_Button_Click(object sender, RoutedEventArgs e)
        {
			if (File.Exists(Settings.PuttyPath)) {
				var process = new Process();
				process.StartInfo.FileName = Settings.PuttyPath;
				process.StartInfo.Arguments = "-ssh root@192.168.0.50";
				process.Start();
			} else {
				//display error message
				MessageBox.Show("Could not find PuTTY. You will need to install putty, or launch it manually\n" +
						"Looking at: " + Settings.PuttyPath + "\n" +
						"Should be pointed to putty.exe");
			}
		}

        private void Add_Waypoint(object sender, RoutedEventArgs e)
        {
            double lat, long_;
            if (Double.TryParse(WaypointLatInput.Text, out lat) &&
                Double.TryParse(WaypointLongInput.Text, out long_)) {
                Waypoints.Add(new Waypoint(lat, long_, WaypointNameInput.Text));

                WaypointNameInput.Text = "";
                FocusManager.SetFocusedElement(this, WaypointLatInput);
            }
        }

        private void LaunchStream(object sender, RoutedEventArgs e)
        {
            NewWindowHandler(sender, e);
        }

        private void NewWindowHandler(object sender, RoutedEventArgs e)
        {
            Thread newWindowThread = new Thread(new ThreadStart(ThreadStartingPoint));
            newWindowThread.SetApartmentState(ApartmentState.STA);
            newWindowThread.IsBackground = true;
            newWindowThread.Start();
        }

        private void ThreadStartingPoint()
        {
            ComboBoxItem typeItem = (ComboBoxItem) StreamSelect.SelectedItem;
            string value = typeItem.Content.ToString();
            Console.WriteLine(value);
            int port = 0;

            List<VideoDevice> devices = Settings.VideoDevices.ToList();
            foreach (VideoDevice v in devices)
            {
                if (v.Name.Equals(value))
                {
                    port = Convert.ToInt32(v.Port);
                }
            }

            Console.WriteLine("Port: " + port);
            if (port != 0)
            {
                VideoStreamer.VideoWindow tempWindow = new VideoStreamer.VideoWindow(port);
                tempWindow.Show();
                tempWindow.StartStream();
                System.Windows.Threading.Dispatcher.Run();
            }    
        }
    }
}
