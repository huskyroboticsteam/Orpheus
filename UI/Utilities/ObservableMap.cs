using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuskyRobotics.Utilities
{
    public class ObservableMap<T> : INotifyPropertyChanged
    {
        private IDictionary<String, MeasuredValue<T>> dictionary = new Dictionary<String, MeasuredValue<T>>();

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the string representation of the value at the given dictionary key. Designed for bindings
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public MeasuredValue<T> this[string key] {
            get => dictionary[key];
        }

        public bool ContainsKey(string key) => dictionary.ContainsKey(key);

        public T GetValue(string key) => dictionary[key].Value;

        public void PutValue(string key, T value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key].Value = value;
            } else
            {
                dictionary[key] = new MeasuredValue<T>(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Count"));
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[" + key + "]"));
        }

        public int Count {
            get => dictionary.Count;
        }
    }

    public class MeasuredValue<T> : INotifyPropertyChanged
    {
        public bool IsPresent {
            get => _value != null;
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
                _value = value;
                LastUpdated = DateTime.Now;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
            }
        }

        public DateTime _lastUpdated;
        public DateTime LastUpdated {
            get => _lastUpdated;
            private set {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LastUpdated"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
