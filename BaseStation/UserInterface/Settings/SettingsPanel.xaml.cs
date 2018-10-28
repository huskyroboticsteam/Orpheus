using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace HuskyRobotics.UI {
	/// <summary>
	/// Interaction logic for SettingsPanel.xaml
	/// </summary>
	public partial class SettingsPanel : UserControl, INotifyPropertyChanged
    {
        private MapConfiguration _mapConfig;
        private Settings _settings;
        public Settings Settings {
            get { return _settings; }
            set {
                _settings = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Settings"));
            }
        }
        private ObservableCollection<string> _mapSets = new ObservableCollection<string>();

        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<string> MapSets
        {
            get => _mapSets;
        }
        public MapConfiguration MapConfig {
            get { return _mapConfig; }
            set {
                _mapConfig = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MapConfig"));
            }
        }

        public SettingsPanel()
        {
            DataContext = this;
            MapConfig = new MapConfiguration();
            InitMapFiles();
            InitializeComponent();
        }


        private void InitMapFiles()
        {
            _mapSets.Clear();
            string imageFolderPath = Directory.GetCurrentDirectory() + @"\Images";
            if (!Directory.Exists(imageFolderPath))
            {
                Directory.CreateDirectory(imageFolderPath);
            }
            string[] files = Directory.GetFiles
                (imageFolderPath, "*.map");
            foreach (string file in files)
            {
                _mapSets.Add(System.IO.Path.GetFileName(file));
            }
        }

        private void Button_GetMap(object sender, RoutedEventArgs e)
        {
            MapStatus.Content = "Downloading map...";
            MapDownloadButton.IsEnabled = false;

			foreach(RadioButton radio in MapTypeContainer.Children) {
				if (radio.IsChecked.HasValue && radio.IsChecked.Value) {
					MapConfig.MapType = radio.Content.ToString().ToLower();
				}
			}

            var bgw = new BackgroundWorker();
            bgw.WorkerReportsProgress = true;
            bgw.DoWork += (worker, _) =>
            {
                MapTileDownloadManager.DownloadNewTileSet(MapConfig, worker as BackgroundWorker);
            };
            bgw.ProgressChanged += (_, progress) =>
            {
                MapStatus.Content = "Downloading " + progress.ProgressPercentage + " of " + MapConfig.TilingHeight * MapConfig.TilingWidth;
            };
            bgw.RunWorkerCompleted += (_, __) =>
            {
                MapDownloadButton.IsEnabled = true;
                InitMapFiles();
                Settings.CurrentMapFile = MapConfig.MapSetName + ".map";
                MapStatus.Content = Settings.CurrentMapFile + " downloaded!";
            };
            bgw.RunWorkerAsync();
        }
    }
}
