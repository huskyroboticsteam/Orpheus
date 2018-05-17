using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace HuskyRobotics.UI
{
    [XmlType("remote_device")]
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

        [XmlElement("name")]
        public string Name {
            get => _name;
            set {
                _name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }

        [XmlElement("address")]
        public string Address {
            get => _address;
            set {
                _address = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Address"));
            }
        }
    }
}
