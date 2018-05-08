using System;
using System.ComponentModel;

namespace HuskyRobotics.UI
{
    public class VideoStream : INotifyPropertyChanged
    {
        public VideoStream() : this("", "")
        {
        }

        public VideoStream(string name, string time)
        {
            Name = name;
            Time = time;
        }

        private string _name;
        private string _time;

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

        public string Time
        {
            get => _time;
            set
            {
                _time = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Time"));
            }
        }
    }
}
