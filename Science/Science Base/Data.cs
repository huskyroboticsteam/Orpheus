using System;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Configurations;
using Scarlet.Utilities;

namespace Science_Base
{
    public struct Datum<DataType>
    {
        public DateTime Time;
        public DataType Data;

        public Datum(DateTime Time, DataType Data) { this.Time = Time; this.Data = Data; }
    }

    public class DataSeries<DataType>
    {
        public string SeriesName { get; set; } // e.g. "Internal Temp"
        public string AxisLabel { get; set; } // e.g. "°C"
        public string DataUnitEntry { get; set; } // e.g. "IntTemp"
        public Color Colour { get; set; }
        public ChartValues<Datum<DataType>> Data { get; set; }

        public DataSeries(string Name, string Label = null)
        {
            this.SeriesName = Name;
            this.AxisLabel = Label;
            this.Colour = Color.FromRgb(0x81, 0x14, 0x26);
            this.Data = new ChartValues<Datum<DataType>>();
        }

        public CartesianMapper<Datum<DataType>> GetMapper()
        {
            return Mappers.Xy<Datum<DataType>>()
                .X(du => du.Time.Ticks)
                .Y(du => GetYVal(du));
        }

        public void Add(DataUnit Unit)
        {
            Datum<DataType> Added = new Datum<DataType>
            {
                Time = Unit.GetValue<DateTime>("Time"),
                Data = Unit.GetValue<DataType>(DataUnitEntry)
            };
            this.Data.Add(Added);
        }

        private double GetYVal(Datum<DataType> Datum)
        {
            return double.Parse(Datum.Data.ToString()); // TODO: This is extremely terrible.
        }
    }
}
