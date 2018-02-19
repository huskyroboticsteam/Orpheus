<<<<<<< HEAD
﻿using System.Windows;
=======
﻿using System;
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
>>>>>>> 4e4814bfd57fc939a6af1d502b75e3a22a26e1c9

// this program is a video player that uses G-streamer through the console with file saving
// capabilities.
// Gstreamer must be installed to the C drive

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
        private DateTime recordingStartTime;
        private DateTime duration;
        private String portFileLocation;
        private String _recordingFolder;
        public String RecordingFolder
        {
            get { return _recordingFolder; }
            set
            {
                _recordingFolder = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Destinaion"));
            }
        }
        private bool playing;

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
            RecordingFolder = Directory.GetCurrentDirectory();
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

        // handles the time increase for the timer
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            duration = new DateTime(DateTime.Now.Subtract(recordingStartTime).Ticks);
            time.Content = "Recording Duration: " + duration.ToString("HH:mm:ss");
        }

        // initiates the recording of the stream
        private void ButtonRecord(object sender, RoutedEventArgs e)
        {
            if (File.Exists(@"C:\gstreamer\1.0\x86_64\bin\gst-launch-1.0.exe"))
            {
                if (!playing)
                {
                    ButtonLaunch(null, null);
                }
                recorders = new Process[ports.Count];
                recordingStartTime = DateTime.Now;
                for (int i = 0; i < ports.Count; i++)
                {
                    makeRecorderShellProcess(recorders[i], ports.ElementAt(i), i);
                }
            }
            else
            {
                System.Windows.MessageBox.Show(@"Please ensure that gst-launch-1.0.exe is located in C:\gstreamer\1.0\x86_64\bin\");
            }
            duration = new DateTime(0);
            time.Content = "Recording Duration: " + duration.ToString("HH:mm:ss");
            recordingTime.Start();
        }

        // plays the video streams in their own windows and if already playing will close them
        private void ButtonLaunch(object sender, RoutedEventArgs e)
        {
            if (playing)
            {
                playing = false;
                btnLaunch.Content = "Launch";
                closeAllPlayers();
            }
            else
            {
                if (File.Exists(@"C:\gstreamer\1.0\x86_64\bin\gst-launch-1.0.exe"))
                {
                    playing = true;
                    btnLaunch.Content = "Stahp";
                    players = new Process[ports.Count];
                    for (int i = 0; i < ports.Count; i++)
                    {
                        startStreamPlayer(ports.ElementAt(i), ref players[i]);
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show(@"Please ensure that gst-launch-1.0.exe is located in C:\gstreamer\1.0\x86_64\bin\");
                }
            }
        }

        // closes all video player windows
        private void closeAllPlayers()
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].CloseMainWindow();
            }
        }

        // initiates a player stream with a given port
        private void startStreamPlayer(int port, ref Process pro)
        {
            pro = new Process();
            pro.StartInfo.FileName = "cmd.exe";
            pro.StartInfo.RedirectStandardInput = true;
            pro.StartInfo.UseShellExecute = false;
            pro.Start();
            pro.StandardInput.WriteLine(@"set PATH=%PATH%;C:\gstreamer\1.0\x86_64\bin");
            pro.StandardInput.WriteLine("gst-launch-1.0 udpsrc caps=\"application/x-rtp," +
                " media=(string)video, clock-rate=(int)90000, encoding-name=(string)H264\" port=" +
                port + " ! rtph264depay ! decodebin ! videoconvert ! autovideosink");
        }

        // initializes and launches the file recording processes
        private void makeRecorderShellProcess(Process pro, int port, int cam)
        {
            pro = new Process();
            pro.StartInfo.RedirectStandardInput = false;
            pro.StartInfo.UseShellExecute = true;
            pro.StartInfo.WorkingDirectory = RecordingFolder;
            pro.StartInfo.FileName = @"C:\gstreamer\1.0\x86_64\bin\gst-launch-1.0.exe";
            pro.StartInfo.Arguments = "-m -e udpsrc caps=\"application/x-rtp," +
                " media=(string)video, clock-rate=(int)90000, encoding-name=(string)H264\" port=" +
                port + " ! rtph264depay ! h264parse ! mp4mux ! filesink location=" +
                DateTime.Now.ToString("MM-dd-yyyy--HH;mm;ss") + "(" + cam + ")" + ".mp4";
            pro.Start();
        }

        // adds ports to the port list
        private void AddPort_Textbox(object sender, RoutedEventArgs e)
        {
            int x;
            if (Int32.TryParse(portBox.Text, out x))
            {
                Ports.Add(x);
            }
            portBox.Text = "";
        }

        // removes selected items from the port list
        private void RemoveSelectedPorts(object sender, RoutedEventArgs e)
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
                closeAllPlayers();
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
