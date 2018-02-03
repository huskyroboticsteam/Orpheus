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
    /// Interaction logic for PingMonitor.xaml
    /// </summary>
    public partial class PingMonitor : UserControl
    {
        private List<String> _addresses = new List<string>();
        private NetworkStatus _netStatus = new NetworkStatus();
        
        public PingMonitor()
        {
            DataContext = _netStatus; // Tell WPF where to search for bindings

            InitializeComponent();
            AddAddresses();

        }

        public void AddAddresses()
        {

            _addresses.Add("www.google.com");
            _addresses.Add("www.isitraininginseattle.com");
            for(int i = 0; i < _addresses.Count; i++)
            {
                _netStatus.PingResults.Add(new PingResult(_addresses[i]));
            }
        }
 
    }
}
