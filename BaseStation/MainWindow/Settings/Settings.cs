using HuskyRobotics.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace HuskyRobotics.UI
{
    public class Settings : INotifyPropertyChanged
    {
        /// <summary>
        /// Maps device names to their addresses
        /// </summary>
        private string _currentMapFile;
        private ObservableCollection<RemoteDevice> _devices = new ObservableCollection<RemoteDevice>();
        private String _puttyPath = @"C:\Program Files (x86)\PuTTY\putty.exe";

        /// <summary>
        /// Triggered when any value in any part of the settings is changed.
        /// </summary>
        public event PropertyChangedEventHandler ValueChanged;
        // Boilerplate
        public event PropertyChangedEventHandler PropertyChanged;
        
        public IEnumerable<RemoteDevice> Devices {
            get => _devices;
        }

        public String PuttyPath {
            get => _puttyPath;
            set {
                _puttyPath = value;
                NotifyChanged("PuttyPath");
            }
        }

        public String CurrentMapFile {
            get => _currentMapFile;
            set {
                _currentMapFile = value;
                NotifyChanged("CurrentMapFile");
            }
        }


        // For serialization only
        [XmlElement("Devices"), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public List<RemoteDevice> DevicesSurrogate { get => _devices.ToList(); set => _devices = new ObservableCollection<RemoteDevice>(value); }
        
        public Settings()
        {
            _devices.CollectionChanged += CollectionListener("Devices");
        }

        private NotifyCollectionChangedEventHandler CollectionListener(string collectionName)
        {
            return (sender, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (var item in e.NewItems.OfType<INotifyPropertyChanged>())
                    {
                        item.PropertyChanged += Settings_PropertyChanged;
                    }
                } else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (var item in e.OldItems.OfType<INotifyPropertyChanged>())
                    {
                        item.PropertyChanged -= Settings_PropertyChanged;
                    }
                }
                ValueChanged?.Invoke(this, new PropertyChangedEventArgs(collectionName));
            };
        }

        private void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ValueChanged.Invoke(this, new PropertyChangedEventArgs(null));
        }

        private void NotifyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            ValueChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceName"></param>
        /// <returns>Returns null if the device does not have an associated entry</returns>
        public String AddressOfDevice(string deviceName)
        {
            // Linear search but we should be fine with only a few devices
            return Devices.FirstOrDefault(d => d.Name.Equals(deviceName))?.Address;
        }
    }
}
