using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;

namespace HuskyRobotics.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class sensorHistoryGraph : UserControl
    {



        public SeriesCollection seriesCollection { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public Func<double, string> XFormatter { get; set; }
        private const string LogFilesLocation = "Logs";
        private const char Delimiter = ',';
        private int selectedFile;
        private FileInfo[] data;

        public sensorHistoryGraph()
        {
            InitializeComponent();
            seriesCollection = new SeriesCollection();
            getData();
            makeDropBox();
            YFormatter = valueY => valueY.ToString("0.##");
            YFormatter = valueX => valueX.ToString("0.##");
        }

        private void getData()
        {
            DirectoryInfo di = new DirectoryInfo(LogFilesLocation);
            try
            {
                data = di.GetFiles("*.csv");
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine("Logs Directory could not be found");
                Console.Write(e);
            }

        }

        private void makeDropBox()
        {
            for (int i = 0; i < data.Length; i++)
            {
                ComboBoxItem cboxitem = new ComboBoxItem();
                cboxitem.Content = data[i].Name;
                Selection.Items.Add(cboxitem);
            }
        }

        private void updateGraph()
        {
            seriesCollection.Clear();
            StreamReader sr = data[selectedFile].OpenText();
            string[] headers = sr.ReadLine().Split(Delimiter);
            List<double>[] sensorValues = new List<double>[headers.Length];
            for (int i = 0; i < headers.Length; i++)
            {
                sensorValues[i] = new List<double>();
            }
            while (sr.Peek() >= 0)
            {
                string[] sensorLogData = sr.ReadLine().Split(Delimiter);
                double currentdata;
                for (int i = 0; i < sensorLogData.Length; i++)
                {
                    currentdata = Convert.ToDouble(sensorLogData[i]);
                    sensorValues[i].Add(currentdata);
                }
            }

            for (int i=0; i < sensorValues.Length; i++) {
                seriesCollection.Add(new LineSeries
                {
                    Title = headers[i],
                    Values = new ChartValues<double>(sensorValues[i])
                });
            }
            Console.Write("test");
            DataContext = this;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedFile = Selection.SelectedIndex;
            updateGraph();
        }
    }
}

