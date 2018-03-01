using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts.WinForms;

namespace Science_Base
{
    public class ChartInstance
    {
        public CartesianChart Chart;
        public ListBox DataChooser;

        public ChartInstance(CartesianChart Chart, ListBox Chooser)
        {
            this.Chart = Chart;
            this.DataChooser = Chooser;

            this.Chart.AnimationsSpeed = TimeSpan.FromMilliseconds(100);
            LiveCharts.Wpf.Axis X = new LiveCharts.Wpf.Axis()
            {
                LabelFormatter = value => new DateTime((long)value).ToString("T")
            };
            this.Chart.AxisX.Add(X);
            this.Chart.DisableAnimations = true;
        }

        public void AddSeries<T>(DataSeries<T> Series)
        {
            LiveCharts.Wpf.Axis Y = new LiveCharts.Wpf.Axis()
            {
                Title = Series.AxisLabel
            };
            this.Chart.AxisY.Add(Y);

            LiveCharts.Wpf.LineSeries ChartSeries = new LiveCharts.Wpf.LineSeries(Series.GetMapper())
            {
                Values = Series.Data,
                Stroke = MainWindow.Scarlet,
                Fill = MainWindow.ScarletBack
            };
            this.Chart.Series.Add(ChartSeries);
        }

        public void Clear()
        {
            this.Chart.Series.Clear();
            this.Chart.AxisY.Clear();
        }
    }

    public class ChartManager
    {
        public ChartInstance Left, Right;

        public ChartManager(CartesianChart ChartLeft, CartesianChart ChartRight, ListBox Chooser)
        {
            this.Left = new ChartInstance(ChartLeft, Chooser);
            this.Right = new ChartInstance(ChartRight, Chooser);
        }
    }
}
