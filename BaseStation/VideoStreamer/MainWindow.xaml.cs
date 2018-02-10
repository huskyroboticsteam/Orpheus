using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

// this program is a video player that uses G-streamer through the console with file saving
// capabilities.
// Gstreamer must be installed to the C drive
// TO DO:
// find a final loaction for ports file and create generic destination to work on any computer

// server command:
// IP will cange when switching to basestation computer
// gst-launch-1.0 v4l2src device="/dev/video0" ! "video/x-raw, format=(string)I420, width=(int)1280, height=(int)720" ! omxh264enc ! 'video/x-h264, stream-format=(string)byte-stream' ! h264parse ! rtph264pay ! udpsink host=192.168.0.5 port=5555
// client test:
// gst-launch-1.0 -vvv -e udpsrc caps=\"application/x-rtp, media=(string)video, clock-rate=(int)90000, encoding-name=(string)H264\" port=5555 ! rtph264depay ! h264parse ! mp4mux ! filesink location=test.mp4
namespace VideoStreamer
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private DispatcherTimer recordingTime;
        private DateTime duration;
        private String portFileLocation;
        private String _destination;
        public String Destination
        {
            get { return _destination; }
            set
            {
                _destination = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Destinaion"));
            }
        }
        private Boolean playing;

        private ObservableCollection<int> ports;
        private Process[] players;
        private Process[] recorders;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<int> Ports
        {
            get => ports;
        }

        public MainWindow()
        {
            recordingTime = new DispatcherTimer();
            recordingTime.Tick += new EventHandler(dispatcherTimer_Tick);
            recordingTime.Interval = new TimeSpan(0, 0, 1);
            duration = new DateTime(0);
            DataContext = this;
            Destination = @"C:\Users\emela\Desktop\";
            // I know this is bad but I don't know where it should go
            portFileLocation = Directory.GetCurrentDirectory() + @"\ports.txt";

            ports = new ObservableCollection<int>();
            initPorts();

            InitializeComponent();
        }

        // initializes the ports from the portfilelocation
        private void initPorts()
        {
            String line;
            int x;
            StreamReader file = new StreamReader(portFileLocation);
            while ((line = file.ReadLine()) != null)
            {
                if (Int32.TryParse(line, out x))
                {
                    ports.Add(x);
                }
            }
            file.Close();
        }

        // initializes the processes used to launch the stream and file recorder
        private void initProcesses(ref Process[] pro)
        {
            pro = new Process[ports.Count];
            for (int i = 0; i < ports.Count; i++)
            {
                pro[i] = new Process();
                pro[i].StartInfo.RedirectStandardInput = false;
                pro[i].StartInfo.UseShellExecute = true;
            }
        }

        // handles the time increase for the timer
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            duration = duration.AddSeconds(1);
            time.Content = "Recording Duration: " + duration.ToString("HH:mm:ss");
        }

        // initiates the recording of the stream
        private void ButtonRecord(object sender, RoutedEventArgs e)
        {
                initProcesses(ref recorders);
                for (int i = 0; i < ports.Count; i++)
                {
                    recordStream(ports.ElementAt(i), recorders[i], i);
                }
                duration = new DateTime(0);
                time.Content = "Recording Duration: " + duration.ToString("HH:mm:ss");
                recordingTime.Start();
        }

        // plays the video streams in their own windows
        private void ButtonLaunch(object sender, RoutedEventArgs e)
        {
            if (playing)
            {
                playing = false;
                btnLaunch.Content = "Launch";
                closeAll(players);
            }
            else
            {
                playing = true;
                btnLaunch.Content = "Stahp";
                initProcesses(ref players);
                for (int i = 0; i < ports.Count; i++)
                {
                    startStream(ports.ElementAt(i), players[i]);
                }
            }
        }

        // closes all windows in the array of processes given
        private void closeAll(Process[] cmd)
        {
            for (int i = 0; i < cmd.Length; i++)
            {
                cmd[i].CloseMainWindow();
            }
        }

        // initiates a stream with a given port
        private void startStream(int port, Process cmd)
        {
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            cmd.StandardInput.WriteLine(@"set PATH=%PATH%;C:\gstreamer\1.0\x86_64\bin");
            cmd.StandardInput.WriteLine("gst-launch-1.0 -vvv udpsrc caps=\"application/x-rtp," +
                " media=(string)video, clock-rate=(int)90000, encoding-name=(string)H264\" port=" +
                port + " ! rtph264depay ! decodebin ! videoconvert ! autovideosink");
        }

        // saves the stream to a file from the given port, destination is set from the control panel
        // and a pseudo-random name is generated for the file using the current date and time
        private void recordStream(int port, Process cmd, int cam)
        {
            cmd.StartInfo.WorkingDirectory = Destination;
            cmd.StartInfo.FileName = @"C:\gstreamer\1.0\x86_64\bin\gst-launch-1.0.exe";
            cmd.StartInfo.Arguments = "-m -vvv -e udpsrc caps=\"application/x-rtp," +
                " media=(string)video, clock-rate=(int)90000, encoding-name=(string)H264\" port=" +
                port + " ! rtph264depay ! h264parse ! mp4mux ! filesink location=" +
                DateTime.Now.ToString("MM-dd-yyyy--HH;mm;ss") + "(" + cam + ")" + ".mp4";
            cmd.Start();
        }

        // adds ports to the port list
        private void Add(object sender, RoutedEventArgs e)
        {
            int x;
            if (Int32.TryParse(portBox.Text, out x))
            {
                Ports.Add(x);
            }
            portBox.Text = "";
        }

        // removes selected items from the port list
        private void Remove(object sender, RoutedEventArgs e)
        {
            List<int> remove = new List<int>();
            foreach (int thing in portList.SelectedItems)
            {
                remove.Add(thing);
            }
            foreach (int eachItem in remove)
            {
                Ports.Remove(eachItem);
            }
        }

        // closes all player processes when the window closes the recorders will not be closed
        // because they must be closed by Ctrl+C
        // ports will be re-written to file in case of edits made
        private void Window_Closing(object sender, EventArgs e)
        {
            System.Windows.MessageBox.Show("Please close all recording streams using Ctrl+C");
            try
            {
                closeAll(players);
            }
            catch { }

            StreamWriter saver = new StreamWriter(portFileLocation);
            for (int i = 0; i < ports.Count; i++)
            {
                saver.WriteLine(ports.ElementAt(i));
            }
            saver.Close();
        }
    }
}
