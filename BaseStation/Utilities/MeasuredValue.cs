using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuskyRobotics.Utilities
{
    /// <summary>
    /// A value which keeps track of when it was last updated.
    /// </summary>

    public class MeasuredValue<T> : INotifyPropertyChanged
    {
        public bool IsPresent {
            get => _value != null;
        }

        public MeasuredValue()
        {
        }

        public MeasuredValue(T value)
        {
            _value = value;
            _lastUpdated = DateTime.Now;
        }

        private T _value;
        public T Value {
            get => _value;
            set {
                if (value == null)
                {
                    throw new ArgumentException("Can't set the value to be null. Old value: " + _value.ToString());
                }

                _value = value;
                LastUpdated = DateTime.Now;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
            }
        }

        public DateTime _lastUpdated;
        public DateTime LastUpdated {
            get => _lastUpdated;
            private set {
                _lastUpdated = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LastUpdated"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
