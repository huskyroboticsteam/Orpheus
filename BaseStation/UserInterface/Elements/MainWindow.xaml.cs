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
using System.ComponentModel;
using HuskyRobotics.UI.VideoStreamer;
using System.Windows.Media;
using HuskyRobotics.UI.Elements;
using Scarlet.Utilities;

namespace HuskyRobotics.UI {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		private const string SETTINGS_PATH = "settings.xml";
		private SettingsFile SettingsFile = new SettingsFile(SETTINGS_PATH);
        private WaypointsFile WaypointsFile { get; set; }
        private List<VideoWindow> VideoWindows = new List<VideoWindow>();
        public Settings Settings { get => SettingsFile.Settings; }
        public ObservableDictionary<string, MeasuredValue<double>> Properties { get; }
        public Armature SetpointArm;
        public ObservableCollection<Waypoint> Waypoints {
            get => WaypointsFile?.Waypoints != null ? WaypointsFile.Waypoints : new ObservableCollection<Waypoint>();
        }
        public ObservableCollection<VideoStream> Streams { get; private set; } = new ObservableCollection<VideoStream>();
        public bool manualMode = true;
        public double gLat = 0.0;
        public double gLon = 0.0;

        public MainWindow()
        {
            Environment.SetEnvironmentVariable("GST_PLUGIN_SYSTEM_PATH", Directory.GetCurrentDirectory() + "\\lib");
            try
            {
                Gst.Application.Init();
            }
            catch (DllNotFoundException)
            {
                Log.Output(Log.Severity.ERROR, Log.Source.OTHER, "GStreamer libs not found, please include them in lib directory");
            }
            catch (TypeInitializationException)
            {
                Log.Output(Log.Severity.ERROR, Log.Source.OTHER, "GStreamer libs not found, please include them in lib directory");
            }
            Properties = new MockObservableMap();
            InitializeComponent();
			((ConsoleView)FindName("console")).Writer.WriteLine("ConsoleView disabled due to performance issues, use Command Prompt view instead.");
			//Console.SetOut(((ConsoleView)FindName("console")).Writer);
			WindowState = WindowState.Maximized;
            this.Closing += OnCloseEvent;
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

            if (!File.Exists(Directory.GetCurrentDirectory() + @"\Images\" + Settings.CurrentMapFile))
            {
                string[] mapFiles = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "Images"), "*.map");
                if (mapFiles.Count() > 0)
                {
                    Settings.CurrentMapFile = Path.GetFileName(mapFiles[0]);
                }
            }
            updateMapWaypoints();
            HuskyRobotics.BaseStation.Server.PacketSender.NotificationUpdate += arrivalPacketScan;
        }

        private void arrivalPacketScan(object sender, int data)
        {
            Dispatcher.Invoke(() =>
            {
                Notification popup = new Notification(gLat, gLon);
                popup.ShowDialog();
            });
        }

        private void updateMapWaypoints()
        {
            if (Settings.CurrentMapFile != null)
            {
                WaypointsFile = new WaypointsFile(Settings.CurrentMapFile.Replace(".map", ".waypoints"));
            }
            Map.Waypoints = Waypoints;
            Map.DisplayMap(Settings.CurrentMapFile);
        }

        private void SettingChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("CurrentMapFile"))
            {
                updateMapWaypoints();
                WaypointList.ItemsSource = Waypoints;
            }
        }

        private void PuTTY_Button_Click(object sender, RoutedEventArgs e)
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
                Double.TryParse(WaypointLongInput.Text, out long_))
            {
                Waypoints.Add(new Waypoint(lat, long_, WaypointNameInput.Text));

                WaypointNameInput.Text = "";
                FocusManager.SetFocusedElement(this, WaypointLatInput);

                
                Tuple<double, double> temp = Tuple.Create( lat, long_ );
                //HuskyRobotics.BaseStation.Server.PacketSender.coords.Add(temp);
                //HuskyRobotics.BaseStation.Server.PacketSender.target = temp;
            }
        }

        private void LaunchStream(object sender, RoutedEventArgs e)
        {
            if (StreamSelect.HasItems && StreamSelect.SelectedItem is VideoDevice)
            {
                VideoDevice selection = (VideoDevice)StreamSelect.SelectedItem;
                Streams.Add(new VideoStream(selection.Name, "00:00:00"));

                Thread newWindowThread = new Thread(() => ThreadStartingPoint(selection.Name, selection.IP, Convert.ToInt32(selection.Port), selection.URI, Convert.ToInt32(selection.BufferingMs)));
                newWindowThread.SetApartmentState(ApartmentState.STA);
                newWindowThread.IsBackground = true;
                newWindowThread.Start();
            }
        }

        private void ThreadStartingPoint(string Name, string IP, int Port, string URI, int BufferingMs)
        {
            VideoWindow tempWindow = new RTSPVideoWindow(Name, IP, Port, URI, Settings.RecordingPath, BufferingMs);

            VideoWindows.Add(tempWindow);

            Window tempWindowCastAsWindow = tempWindow as Window;
            tempWindowCastAsWindow.Closed += VideoWindowClosedEvent;
            tempWindowCastAsWindow.Show();
            tempWindow.StartStream();
            System.Windows.Threading.Dispatcher.Run();
        }

        private void VideoWindowClosedEvent(object sender, EventArgs e)
        {
            VideoWindow window = (VideoWindow)sender;

            Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
            {
                for (int i = 0; i < this.Streams.Count; i++)
                {
                    if (this.Streams[i].Name.Equals(window.StreamName))
                    {
                        this.Streams.RemoveAt(i);
                        this.VideoWindows.RemoveAt(i);
                        break;
                    }
                }
            }));
        }

        private void OnCloseEvent(object sender, CancelEventArgs e)
        {
            for (int i = VideoWindows.Count - 1; i >= 0; i--)
            {
                VideoWindow window = VideoWindows[i];
                if(window is Window videoWindowAsWindow)
                {
                    videoWindowAsWindow.Dispatcher.InvokeAsync(() => {
                        videoWindowAsWindow.Hide(); //Closing takes awhile so hide the window
                        videoWindowAsWindow.Close();
                    }, System.Windows.Threading.DispatcherPriority.Normal);
                }
                VideoWindows.RemoveAt(i);
            }
        }

        private void SwitchModes(object sender, EventArgs e)
        {
            if (manualMode)
            {
                manualMode = false;
                ModeLabel.Content = "Autonomous";
                ModeLabel.Foreground = Brushes.Green;
            }
            else
            {
                manualMode = true;
                ModeLabel.Content = "Manual";
                ModeLabel.Foreground = Brushes.Red;
            }
            HuskyRobotics.BaseStation.Server.PacketSender.SwitchMode(manualMode);
            

        }

        private void UpdateSliderValue(object sender, EventArgs e)
        {
            double scale = Math.Round(Arm_Sensitivity.Value / 10, 2);
            HuskyRobotics.BaseStation.Server.PacketSender.SwitchScaler(scale);
            Sensitivty_percentages.Content = System.Convert.ToString(scale);
        }

        private void Start_Navigation(object sender, EventArgs e)
        {
            double lat = Waypoints.ElementAt(0).Lat;
            double lon = Waypoints.ElementAt(0).Long;
            gLat = lat;
            gLon = lon;
            HuskyRobotics.BaseStation.Server.PacketSender.target =Tuple.Create(lat, lon);
        }

        private void Stop_Navigation(object sender, EventArgs e)
        {
            //for testing purposes
            
        }
    }
}
