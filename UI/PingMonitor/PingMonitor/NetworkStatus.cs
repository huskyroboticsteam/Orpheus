using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuskyRobotics.UI
{
    public class NetworkStatus : INotifyPropertyChanged
    {
        private DateTime _lastUpdated = DateTime.Now;
        private ObservableCollection<PingResult> _pingResults = new ObservableCollection<PingResult>();

        // Boilerplate
        public event PropertyChangedEventHandler PropertyChanged;

        public DateTime LastUpdated
        {
            get
            {
                return _lastUpdated;
            }
            set
            {
                _lastUpdated = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LastUpdated"));
            }
        }

        public ObservableCollection<PingResult> PingResults
        {
            get
            {
                return _pingResults;
            }
            set
            {
                _pingResults = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PingResults"));
            }
        }
    }
}