using System;
using System.ComponentModel;

namespace HuskyRobotics.UI
{
    [Serializable]
    public class RemoteDevice : INotifyPropertyChanged
    {
        public RemoteDevice() : this("", "")
        {
        }

        public RemoteDevice(string name, string address)
        {
            Name = name;
            Address = address;
        }

        private string _name;
        private string _address;

        // Boilerplate
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name {
            get => _name;
            set {
                _name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }

        public string Address {
            get => _address;
            set {
                _address = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Address"));
            }
        }
    }
}
