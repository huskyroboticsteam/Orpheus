using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using LiveCharts.Geared;
using LiveCharts.WinForms;

namespace Science_Base
{
    public class ChartInstance
    {
        public CartesianChart Chart;
        public ListBox DataChooser;

        private HashSet<int> SeriesIndexList = new HashSet<int>();

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
            this.Chart.AxisY.Clear();
            this.Chart.DisableAnimations = true;
            this.Chart.AllowDrop = true;
        }

        public void AddSeries<T>(DataSeries<T> Series)
        {
            if (this.Chart.Series.Count == 0) { this.Chart.AxisY.Clear(); }
            LiveCharts.Wpf.Axis Y = new LiveCharts.Wpf.Axis()
            {
                Title = Series.AxisLabel
            };
            this.Chart.AxisY.Add(Y);

            GLineSeries ChartSeries = new GLineSeries(Series.GetMapper())
            {
                Values = Series.Data,
                Stroke = UIHelper.ScarletColour,
                Fill = UIHelper.ScarletBackColour,
                PointGeometry = null
            };
            this.Chart.Series.Add(ChartSeries);

            object[] AllSeries = DataHandler.GetSeries();
            if (AllSeries.Contains(Series)) { this.SeriesIndexList.Add(Array.IndexOf(AllSeries, Series)); }
        }

        // Thanks Sasha!
        public void AddByIndex(int Index)
        {
            object Series = DataHandler.GetSeries()[Index];
            Type type = Series.GetType();
            Type Generic = type.GetGenericArguments()[0];
            MethodInfo Info = this.GetType().GetMethod("AddSeries");
            Info = Info.MakeGenericMethod(Generic);
            Info.Invoke(this, new object[] { Series });
            this.SeriesIndexList.Add(Index);
        }

        public HashSet<int> GetIndices() { return this.SeriesIndexList; }

        public void Clear()
        {
            this.Chart.Series.Clear();
            this.Chart.AxisY.Clear();
            this.SeriesIndexList.Clear();
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
