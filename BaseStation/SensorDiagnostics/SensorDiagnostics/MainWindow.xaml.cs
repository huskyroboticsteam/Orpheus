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
        private const string LogFilesLocation = "Logs";
        private const char Delimiter = ',';
        private const double margin = 10;
        private const double xmin = margin;
        private double xmax;
        private const double ymin = margin;
        private double ymax;
        private const double step = 10;

        private int selectedFile;
        private FileInfo[] data;
        private double maxValue = 0;

        public sensorHistoryGraph()
        {
            InitializeComponent();
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
            for (int i = 0; i < data.Length; i++)
            {
                ComboBoxItem cboxitem = new ComboBoxItem();
                cboxitem.Content = data[i].Name;
                Selection.Items.Add(cboxitem);
            }
        }

        private void updateGraph()
        {
            StreamReader sr = data[selectedFile].OpenText();
            string[] headers = sr.ReadLine().Split(Delimiter);
            List<double>[] sensorValues = new List<double>[headers.Length];
            while (sr.Peek() >= 0)
            {
                string[] sensorLogData = sr.ReadLine().Split(Delimiter);
                double currentdata;
                for(int i=0; i<sensorLogData.Length; i++)
                {
                    currentdata = Convert.ToDouble(sensorLogData[i]);
                    if (maxValue < currentdata)
                    {
                        maxValue = currentdata;
                    }
                    sensorValues[i].Add(currentdata);
                }
                
            }
            Polyline polyline;
            polyline = new Polyline();
            polyline.StrokeThickness = 1;
            int R = 128, G = 0, B = 128;
            SolidColorBrush brushColor = new SolidColorBrush(Color.FromArgb(255, (byte)R, (byte)G, (byte)B));
            PointCollection[] points = new PointCollection[headers.Length];
            Point temp=new Point();
            for(int i=0; i<headers.Length;i++)
            {
                foreach (double d in sensorValues[i])
                {
                    temp = new Point(ymax * (d / maxValue), i * (xmax / sensorValues[i].Count));
                    points[i].Add(temp);
                }
                polyline.Stroke = brushColor;
                polyline.Points = points[i];
                canGraph.Children.Add(polyline);
                Label lineLabel = new Label();
                lineLabel.Content = headers[i];
                canGraph.Children.Add(lineLabel);
                Canvas.SetBottom(lineLabel,temp.Y);
                Canvas.SetRight(lineLabel,temp.X);
                canGraph.Children.Add(lineLabel);
                R += 20;
                B += 20;
                brushColor.Color = Color.FromArgb(255, (byte)R, (byte)G, (byte)B);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedFile = Selection.SelectedIndex;
            updateGraph();
        }
    }
}

