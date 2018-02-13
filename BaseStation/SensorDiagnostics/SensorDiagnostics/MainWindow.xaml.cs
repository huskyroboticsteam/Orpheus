using System;
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
using HuskyRobotics.Utilities;
using System.Diagnostics;
using System.IO;

namespace SensorHistoryGraph
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string LogFilesLocation = "Logs";
        private const char Delimiter = ',';
        private const double margin = 10;
        private const double xmin = margin;
        private double xmax;
        private const double ymin = margin;
        private double ymax;
        private const double step = 10;

        private Regex encoderRegex = new Regex(@"Encoder_\d+");
        private Regex LimitSwitchRegex = new Regex(@"LimitSwitch_\d+");
        private Regex MAX31855Regex = new Regex(@"MAX31855_\d+");
        private Regex MPU6050Regex = new Regex(@"MPU6050_\d+");
        private Regex MTK3339Regex = new Regex(@"MTK3339_\d+");
        private Regex PotentiometerRegex = new Regex(@"Potentiometer_\d+");
        private Regex VEML6070Regex = new Regex(@"VEML6070_\d+");
        private int selectedFile;
        private FileInfo[] data;
        private int maxValue = 0;

        public MainWindow()
        {
            InitializeComponent();

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            getData();
            makeDropBox();
            makeGraph();
        }

        private void getData()
        {
            DirectoryInfo di = new DirectoryInfo(LogFilesLocation);
            data = di.GetFiles("*.csv");
        }

        private void makeGraph()
        {

            xmax = canGraph.Width - margin;
            ymax = canGraph.Height - margin;

            // Make the X axis.
            GeometryGroup xaxis_geom = new GeometryGroup();
            xaxis_geom.Children.Add(new LineGeometry(
                new Point(0, ymax), new Point(canGraph.Width, ymax)));
            for (double x = xmin + step;
                x <= canGraph.Width - step; x += step)
            {
                xaxis_geom.Children.Add(new LineGeometry(
                    new Point(x, ymax - margin / 2),
                    new Point(x, ymax + margin / 2)));
            }

            System.Windows.Shapes.Path xaxis_path = new System.Windows.Shapes.Path();
            xaxis_path.StrokeThickness = 1;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;

            canGraph.Children.Add(xaxis_path);

            // Make the Y ayis.
            GeometryGroup yaxis_geom = new GeometryGroup();
            yaxis_geom.Children.Add(new LineGeometry(
                new Point(xmin, 0), new Point(xmin, canGraph.Height)));
            for (double y = step; y <= canGraph.Height - step; y += step)
            {
                yaxis_geom.Children.Add(new LineGeometry(
                    new Point(xmin - margin / 2, y),
                    new Point(xmin + margin / 2, y)));
            }

            System.Windows.Shapes.Path yaxis_path = new System.Windows.Shapes.Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = yaxis_geom;

            canGraph.Children.Add(yaxis_path);
        }

        private void makeDropBox()
        {
            ComboBox cbox = new ComboBox();
            cbox.Background = Brushes.LightGray;
            for (int i = 0; i < data.Length; i++)
            {
                ComboBoxItem cboxitem = new ComboBoxItem();
                cboxitem.Content = data[i].Name;
                cbox.Items.Add(cboxitem);
            }
            cbox.SelectionChanged += (object sender, SelectionChangedEventArgs args) =>
            {
                selectedFile = cbox.SelectedIndex;
                updateGraph();
            };
            grid.Children.Add(cbox);
        }

        private void updateGraph()
        {
            StreamReader sr = data[selectedFile].OpenText();
            while (sr.Peek() >= 0)
            {
                int[] sensorNums = new int[7];
                string[] sensorLogData = sr.ReadLine().Split(Delimiter);
                PointCollection points = createPoints(sensorLogData);
                string sensorType = sensorLogData[0];
                Color brushColor = Color.FromRgb(0, 0, 0);
                if (encoderRegex.IsMatch(sensorType))
                {
                    brushColor = Color.FromRgb(Convert.ToByte(255 - 10 * sensorNums[0]), 0, 0);
                    if (sensorNums[0] < 255)
                    {
                        sensorNums[0]++;
                    }
                    else
                    {
                        sensorNums[0] = 0;
                    }
                }
                else
                    if (LimitSwitchRegex.IsMatch(sensorType))
                {
                    brushColor = Color.FromRgb(0, Convert.ToByte(255 - 10 * sensorNums[0]), 0);
                    if (sensorNums[1] < 255)
                    {
                        sensorNums[1]++;
                    }
                    else
                    {
                        sensorNums[1] = 0;
                    }
                }
                else
                    if (MAX31855Regex.IsMatch(sensorType))
                {
                    brushColor = Color.FromRgb(0, 0, Convert.ToByte(255 - 10 * sensorNums[0]));
                    if (sensorNums[2] < 255)
                    {
                        sensorNums[2]++;
                    }
                    else
                    {
                        sensorNums[2] = 0;
                    }
                }
                else
                    if (MPU6050Regex.IsMatch(sensorType))
                {
                    brushColor = Color.FromRgb(Convert.ToByte(255 - 10 * sensorNums[0]), Convert.ToByte(255 - 10 * sensorNums[0]), 0);
                    if (sensorNums[3] < 255)
                    {
                        sensorNums[3]++;
                    }
                    else
                    {
                        sensorNums[3] = 0;
                    }
                }
                else
                    if (MTK3339Regex.IsMatch(sensorType))
                {
                    brushColor = Color.FromRgb(Convert.ToByte(255 - 10 * sensorNums[0]), 0, Convert.ToByte(255 - 10 * sensorNums[0]));
                    if (sensorNums[4] < 255)
                    {
                        sensorNums[4]++;
                    }
                    else
                    {
                        sensorNums[4] = 0;
                    }
                }
                else
                    if (PotentiometerRegex.IsMatch(sensorType))
                {
                    brushColor = Color.FromRgb(0, Convert.ToByte(255 - 10 * sensorNums[0]), Convert.ToByte(255 - 10 * sensorNums[0]));
                    if (sensorNums[5] < 255)
                    {
                        sensorNums[5]++;
                    }
                    else
                    {
                        sensorNums[5] = 0;
                    }
                }
                else
                    if (VEML6070Regex.IsMatch(sensorType))
                {
                    brushColor = Color.FromRgb(Convert.ToByte(255 - 10 * sensorNums[0]), Convert.ToByte(255 - 10 * sensorNums[0]), Convert.ToByte(255 - 10 * sensorNums[0]));
                    if (sensorNums[6] < 255)
                    {
                        sensorNums[6]++;
                    }
                    else
                    {
                        sensorNums[6] = 0;
                    }
                }
                Polyline polyline = new Polyline();
                polyline.StrokeThickness = 1;
                polyline.Stroke = new SolidColorBrush(brushColor);
                polyline.Points = points;
                canGraph.Children.Add(polyline);
            }
        }

        private PointCollection[] createPoints(string[] pointData)
        {
            PointCollection[] result = new PointCollection[6]();
            int mode = 0;
            string sensorType = pointData[0];
            double[,] dataArray = new double[result.Length, pointData.Length - 1];
            double data;
            for (int i = 1; i < pointData.Length; i++)
            {
                if (encoderRegex.IsMatch(sensorType))
                {
                    data = Convert.ToDouble(pointData[i]);
                }
                else
                if (LimitSwitchRegex.IsMatch(sensorType))
                {
                    data = Convert.ToDouble(Convert.ToBoolean(pointData[i]));
                }
                else
                if (MAX31855Regex.IsMatch(sensorType))
                {
                    data = Convert.ToDouble(pointData[i]);
                }
                else
                if (MPU6050Regex.IsMatch(sensorType))
                {
                    string[] temp1 = sensorType.Split('-');
                    string[] temp2 = temp1[0].Split('/');
                    string[] temp1 = temp1[1].Split('/');
                    string[] tempF = new string[temp1.Length + temp2.Length];
                    Array.Copy(temp1, tempF, temp1.Length);
                    Array.Copy(temp2, 0, tempF, temp1.Length, temp2.Length);
                    data = Convert.ToDouble(tempF[0]);
                    for (int p=1; p<tempF.Length; p++)
                    {
                        dataArray[p,i] = Convert.ToDouble(tempF[p]);
                    }
                }
                else
                if (MTK3339Regex.IsMatch(sensorType))
                {
                    string[] temp = sensorType.Split('/');
                    data = Convert.ToDouble(temp[0]);
                    dataArray[1, i] = Convert.ToDouble(temp[1]);
                }
                else
                if (PotentiometerRegex.IsMatch(sensorType))
                {
                    data = Convert.ToDouble(pointData[i]);
                }
                else
                if (VEML6070Regex.IsMatch(sensorType))
                {
                    data = Convert.ToDouble(pointData[i]);
                }
                if (maxValue < data)
                {
                    maxValue = data;
                }
                dataArray[0,i] = data;
            }
            for (int j = 0; j < dataArray.Length; JournalEntry++)
            {
                double yvalue = d / maxValue * (ymax + ymin);
                double xvalue = j * (xmax / dataArray.Length);
                result.Add(new Point(xvalue, yvalue));
            }
            return result;
        }
    }
}
