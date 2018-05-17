using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace HuskyRobotics.UI
{
    [XmlType("video_devices")]
    public class VideoDevice : INotifyPropertyChanged
    {
        public VideoDevice() : this("", "", "")
        {
        }

        public VideoDevice(string name, string port, string bufferingMs)
        {
            Name = name;
            Port = port;
            BufferingMs = bufferingMs;
        }

        private string _name;
        private string _port;
        private string _bufferingMs;

        // Boilerplate
        public event PropertyChangedEventHandler PropertyChanged;

        [XmlElement("name")]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }

        [XmlElement("port")]
        public string Port
        {
            get => _port;
            set
            {
                _port = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Port"));
            }
        }

        [XmlElement("buffering_ms")]
        public string BufferingMs
        {
            get => _bufferingMs;
            set
            {
                _bufferingMs = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BufferingMs"));
            }
        }
    }
}
