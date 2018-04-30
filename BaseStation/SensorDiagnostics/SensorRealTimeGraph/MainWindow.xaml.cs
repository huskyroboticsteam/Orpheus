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
    /// This class is used to graph sensor values recieved from the BBB in real time
    /// </summary>
    public partial class SensorRealTimeGraph : UserControl, INotifyPropertyChanged
    {
        private DateTime lastUpdate { get; set; } //the last time the graph was updated
        private double _axisMax; //maximum time value
        private double _axisMin; //minimum time value
        private List<DataUnit> lastSensorReadings { get; set; } //holds the previous sensor readings.
        public SeriesCollection seriesCollection { get; set; } // Data to be displayed
        public SeriesCollection exampleSeriesCollection { get; set; } // Example Data used for testing
        public Func<double, string> YFormatter { get; set; } // Yaxis format
        public Func<double, string> DateTimeFormatter { get; set; } // Xaxis format
        public double AxisStep { get; set; } //steps between time axis ticks
        public double AxisUnit { get; set; } //Units for the time axis
        private Random rnd = new Random(); //random num generator used for testing.
        /// <summary>
        /// Constructor: Sets up ui
        /// </summary>
        public SensorRealTimeGraph()
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
            seriesCollection = new SeriesCollection{};
            DataContext = this;
            lastUpdate = DateTime.Now;
            updateGraph();
        }

        /// <summary>
        /// Temporary method that generates random values for testing purposes
        /// </summary>
        public void tempMethod()
        {
            exampleSeriesCollection[0].Values.Add(rnd.NextDouble() * 100);
            exampleSeriesCollection[0].Values.RemoveAt(0);
        }


        public double AxisMax //maximum for time axis
        {
            get { return _axisMax; }
            set
            {
                _axisMax = value;
                OnPropertyChanged("AxisMax");
            }
        }
        public double AxisMin //minimum for time axis
        {
            get { return _axisMin; }
            set
            {
                _axisMin = value;
                OnPropertyChanged("AxisMin");
            }
        }

        /// <summary>
        /// This method sets the scope of the graph to be a certain distance ahead and behind the current time
        /// </summary>
        /// <param name="now"></param>
        private void SetAxisLimits(DateTime now)
        {
            AxisMax = now.Ticks + TimeSpan.FromSeconds(1).Ticks; // forces axis to be 1 second ahead
            AxisMin = now.Ticks - TimeSpan.FromSeconds(8).Ticks; // forces axis to be 8 seconds behind
        }

        /// <summary>
        /// updates the last readings of the graph. then updates the graph
        /// </summary>
        /// <param name="sensorReadings"></param>
        public void updateGraph( List<DataUnit> sensorReadings)
        {
            lastSensorReadings = sensorReadings;
            updateGraph();
        }

        /// <summary>
        /// breaks the last sensor readings data unit into values that can be graphed and plots them.
        /// </summary>
         private void updateGraph()
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

        /// <summary>
        /// Allows the user to click plots on and off
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// onPropertyChanged Event handler.
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
