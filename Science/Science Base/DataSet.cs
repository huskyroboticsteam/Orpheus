using System;
using System.Collections.Generic;
using LiveCharts;
using LiveCharts.Configurations;
using Scarlet.Utilities;

namespace Science_Base
{
    public class DataSet<DataType>
    {
        private List<DataUnit> Data;
        public readonly string Name;
        public readonly string[] SeriesNames;
        public event EventHandler<DataEvent> ItemAdd;

        public DataSet(string Name, string[] SeriesNames, int InitCapacity = 100)
        {
            this.Name = Name;
            this.SeriesNames = SeriesNames;
            this.Data = new List<DataUnit>(InitCapacity);
        }

        public void Add(DataUnit New)
        {
            this.Data.Add(New);
            this.ItemAdd?.Invoke(this, new DataEvent() { Unit = New });
        }

        public List<DataUnit> Get() => this.Data;
        public ChartValues<DataUnit> GetValues() => new ChartValues<DataUnit>(this.Data);

        public static CartesianMapper<DataUnit> GetMapper(string SeriesName)
        {
            return Mappers.Xy<DataUnit>()
                .X(du => du.GetValue<DateTime>("Time").Ticks)
                .Y(du => GetYVal(du, SeriesName));
        }

        public static double GetYVal(DataUnit Data, string SeriesName)
        {
            DataType Val = Data.GetValue<DataType>(SeriesName);
            return double.Parse(Val.ToString());
        }
    }

    public class DataEvent : EventArgs
    {
        public DataUnit Unit { get; set; }
    }
}
