using System;
using System.ComponentModel;
using System.Windows.Threading;

namespace HuskyRobotics.UI
{
    public class VideoStream : INotifyPropertyChanged
    {

        private DateTime StartTime;
        private DispatcherTimer RecordingTime;

        public VideoStream() : this("", "")
        {
        }

        public VideoStream(string name, string time)
        {
            Name = name;
            Time = time;
            StartTime = DateTime.Now;

            RecordingTime = new DispatcherTimer();
            RecordingTime.Interval = new TimeSpan(0, 0, 1);
            RecordingTime.Tick += UpdateTime;
            RecordingTime.Start();
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

        private void UpdateTime(object sender, EventArgs e)
        {
            DateTime duration = new DateTime(DateTime.Now.Subtract(StartTime).Ticks);
            Time = duration.ToString("HH:mm:ss");
        }
    }
}
