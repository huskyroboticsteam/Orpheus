using System.Windows;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using HuskyRobotics.Utilities;

namespace HuskyRobotics.UI {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		private const string SETTINGS_PATH = "settings.xml";
		private SettingsFile SettingsFile = new SettingsFile(SETTINGS_PATH);
		public Settings Settings { get => SettingsFile.Settings; }
		public ObservableDictionary<string, MeasuredValue<double>> Properties { get; }


		public MainWindow() {
			Properties = new MockObservableMap();
			InitializeComponent();
			WindowState = WindowState.Maximized;
			DataContext = this;
            Settings.PropertyChanged += SettingChanged;
		}

        private void SettingChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("CurrentMap"))
            {
                Map.DisplayMap(Settings.CurrentMap);
            } 
        }

        private void PuTTY_Button_Click(object sender, RoutedEventArgs e) {
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
	}
}
