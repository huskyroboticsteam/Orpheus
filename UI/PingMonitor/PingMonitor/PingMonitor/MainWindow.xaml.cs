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

namespace PingMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// 
    /// This class represents the VeiwModel
    /// </summary>
    public partial class MainWindow : Window
    {
        private NetworkStatus _netStatus = new NetworkStatus();

        public MainWindow()
        {
            InitializeComponent();

            DataContext = _netStatus; // Tell WPF where to search for bindings
        }

        public void AddAddress(object sender, RoutedEventArgs e)
        {
            _netStatus.PingResults.Add(new PingResult(AddressInput.Text));
        }

        private void AddressInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddAddress(sender, null);
                AddressInput.Text = "";
            }
        }
    }
}