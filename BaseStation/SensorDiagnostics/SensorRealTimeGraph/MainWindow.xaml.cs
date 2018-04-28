using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Configurations;
using Scarlet.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HuskyRobotics.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class sensorRealTimeGraph : UserControl, INotifyPropertyChanged
    {
        private DateTime lastUpdate { get; set; }
        private double _axisMax;
        private double _axisMin;
        private List<DataUnit> lastSensorReadings { get; set; }
        public SeriesCollection seriesCollection { get; set; } // Data to be displayed
        public SeriesCollection exampleSeriesCollection { get; set; } // Example Data used for testing
        public Func<double, string> YFormatter { get; set; } // Yaxis format
        public Func<double, string> DateTimeFormatter { get; set; } // Xaxis format
        public double AxisStep { get; set; }
        public double AxisUnit { get; set; }
        private Random rnd = new Random();
        /// <summary>
        /// Constructor: Sets up ui
        /// </summary>
        public sensorRealTimeGraph()
        {
            InitializeComponent();
            lastSensorReadings = new List<DataUnit>();
            var mapper = Mappers.Xy<MeasureModel>()
                .X(model => model.DateTime.Ticks)   //use DateTime.Ticks as X
                .Y(model => model.Value);           //use the value property as Y
            //lets save the mapper globally.
            Charting.For<MeasureModel>(mapper);
            YFormatter = valueY => valueY.ToString("0.##");
            DateTimeFormatter = value => new DateTime((long)value).ToString("mm:ss");
            //AxisStep forces the distance between each separator in the X axis
            AxisStep = TimeSpan.FromSeconds(1).Ticks;
            //AxisUnit forces lets the axis know that we are plotting seconds
            //this is not always necessary, but it can prevent wrong labeling
            AxisUnit = TimeSpan.TicksPerSecond;
            SetAxisLimits(DateTime.Now);
            seriesCollection = new SeriesCollection
            {
            };
            DataContext = this;
            lastUpdate = DateTime.Now;
            updateGraph();
        }

        public void tempMethod()
        {
            seriesCollection[0].Values.Add(rnd.NextDouble() * 100);
            seriesCollection[0].Values.RemoveAt(0);
        }

        public double AxisMax
        {
            get { return _axisMax; }
            set
            {
                _axisMax = value;
                OnPropertyChanged("AxisMax");
            }
        }
        public double AxisMin
        {
            get { return _axisMin; }
            set
            {
                _axisMin = value;
                OnPropertyChanged("AxisMin");
            }
        }

        private void SetAxisLimits(DateTime now)
        {
            AxisMax = now.Ticks + TimeSpan.FromSeconds(1).Ticks; // lets force the axis to be 1 second ahead
            AxisMin = now.Ticks - TimeSpan.FromSeconds(8).Ticks; // and 8 seconds behind
        }

        public void updateGraph( List<DataUnit> sensorReadings)
        {
            lastSensorReadings = sensorReadings;
            updateGraph();
        }

         public void updateGraph()
         {
            DateTime now = DateTime.Now;
            if (now.Subtract(lastUpdate).Seconds >=1)
            {
                SetAxisLimits(now);
                bool newKey = true;
                foreach (DataUnit sensor in lastSensorReadings)
                {
                    foreach (string key in sensor.Keys)
                    {
                        double keyValue = sensor.GetValue<double>(key);
                        for (int i = 0; i < seriesCollection.Count; i++)
                        {
                            if (seriesCollection[i].Title == sensor.System + "_" + key)
                            {
                                seriesCollection[i].Values.Add(new MeasureModel { DateTime = now, Value = keyValue });
                                newKey = false;
                                if (seriesCollection[i].Values.Count > 50) seriesCollection[i].Values.RemoveAt(0);
                                break;
                            }
                        }
                        if (newKey)
                        {
                            LineSeries newSeries = new LineSeries
                            {
                                Values = new ChartValues<MeasureModel>(),
                                Title = sensor.System + "_" + key
                            };
                            seriesCollection.Add(newSeries);
                            lBox.ItemsSource = seriesCollection;
                        }
                    }
                }
                lastUpdate = now;
            }
        }
        private void ListBox_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(lBox, (DependencyObject)e.OriginalSource) as ListBoxItem;
            if (item == null) return;

            var series = (LineSeries)item.Content;
            series.Visibility = series.Visibility == Visibility.Visible
                ? Visibility.Hidden
                : Visibility.Visible;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
