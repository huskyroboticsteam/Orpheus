using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Xml.Serialization;

namespace HuskyRobotics.UI
{
    /// <summary>
    /// Interaction logic for SettingsPanel.xaml
    /// </summary>
    public partial class SettingsPanel : UserControl
    {
        public SettingsPanel()
        {
            InitializeComponent();
        }

        private void Button_GetMap(object sender, RoutedEventArgs e)
        {
            Settings settings = (Settings)DataContext;
            MapTileDownloadManager.DownloadNewTileSet(settings.Config);
            settings.initMapFiles();
            settings.CurrentMap = settings.Config.MapSetName + ".map";
        }
    }
}
