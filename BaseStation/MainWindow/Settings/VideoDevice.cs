using System;
using System.ComponentModel;

namespace HuskyRobotics.UI
{
    [Serializable]
    public class VideoDevice : INotifyPropertyChanged
    {
        public VideoDevice() : this("", "")
        {
        }

        public VideoDevice(string name, string port)
        {
            Name = name;
            Port = port;
        }

        private string _name;
        private string _port;

        // Boilerplate
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }

        public string Port
        {
            get => _port;
            set
            {
                _port = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Port"));
            }
        }
    }
}
