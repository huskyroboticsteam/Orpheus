using System;
using System.Collections.Generic;
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
// fix file not opening
// add port adding and subtracting

// server command:
// IP will cange when switching to basestation computer
// gst-launch-1.0 v4l2src device="/dev/video0" ! "video/x-raw, format=(string)I420, width=(int)1280, height=(int)720" ! omxh264enc ! 'video/x-h264, stream-format=(string)byte-stream' ! h264parse ! rtph264pay ! udpsink host=192.168.0.5 port=5555
// client test:
// gst-launch-1.0 -vvv -e udpsrc caps=\"application/x-rtp, media=(string)video, clock-rate=(int)90000, encoding-name=(string)H264\" port=5555 ! rtph264depay ! h264parse ! mp4mux ! filesink location=test.mp4
namespace Video_player
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private DispatcherTimer recordingTime;
        private DateTime duration;
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
        private Boolean recording;
        List<int> ports;
        Process[] players;
        Process[] recorders;
        
        // experimentation
        const int CTRL_C_EVENT = 0;
        const int CTRL_BREAK_EVENT = 1;

        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        //

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            recordingTime = new DispatcherTimer();
            recordingTime.Tick += new EventHandler(dispatcherTimer_Tick);
            recordingTime.Interval = new TimeSpan(0, 0, 1);
            duration = new DateTime(0);
            DataContext = this;
            Destination = @"C:\Users\emela\Desktop\";

            ports = new List<int>();
            initPorts();

            players = new Process[ports.Count];
            recorders = new Process[ports.Count];

            InitializeComponent();

            Console.WriteLine(Directory.GetCurrentDirectory());
        }

        private void initPorts()
        {

        }

        // initializes the processes used to launch the stream and file recorder
        private void initProcesses(Process[] pro)
        {
            for (int i = 0; i < players.Length; i++)
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
        // ALL PLACES WHERE ONE PROCESS IS STARTED SHOULD BE REPLACED WITH A LOOP TO START ALL IN ARRAY
        private void ButtonRecord(object sender, RoutedEventArgs e)
        {
            // stop currently does nothing
            if (recording)
            {
                closeAll(recorders);
                recording = false;
                btnRecord.Content = "Record";
                recordingTime.Stop();
            }
            else
            {
                initProcesses(recorders);
                recordStream(5555, recorders[0], 0);
                recording = true;
                btnRecord.Content = "Stop";
                duration = new DateTime(0);
                time.Content = "Recording Duration: " + duration.ToString("HH:mm:ss");
                recordingTime.Start();
                //debug
                Console.WriteLine(recorders[0].Id);
            }
        }

        // plays the video streams in their own windows
        private void ButtonLaunch(object sender, RoutedEventArgs e)
        {
            initProcesses(players);
            startStream(5555, players[0]);
        }

        [DllImport("user32")]
        public static extern int SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32")]
        public static extern int PostMessage(IntPtr hwnd, UInt32 Msg, int wParam, int lParam);

        // closes all windows in the array of processes given
        private void closeAll(Process[] cmd)
        {
            //SendMessage(cmd[0].MainWindowHandle, CTRL_C_EVENT, (IntPtr)0, (IntPtr)0);
            //SetForegroundWindow(cmd[0].MainWindowHandle);
            //PostMessage(cmd[0].MainWindowHandle, 0x011, 0x43, 0);
            //Thread.Sleep(1000);
            //SendKeys.Send("^{BREAK}");
            //SendKeys.Send("^C");
            //Thread.Sleep(1000);
            //cmd[0].Close();
            //cmd[0].Kill();

            //Console.WriteLine(cmd[0].HasExited);
            //cmd[0].StandardInput.Write("\x3");
            //Console.WriteLine(cmd[0].StandardOutput.ReadToEnd());
            //Console.WriteLine(cmd[0].HasExited);

            //cmd[0].StandardInput.Flush();
            //cmd[0].WaitForExit();

            //cmd[0].StandardInput.Close();

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
            Console.WriteLine(cmd.Id);

            //cmd.Start();
            //cmd.StandardInput.WriteLine("cd " + Destination);
            //cmd.StandardInput.WriteLine(@"set PATH=%PATH%;C:\gstreamer\1.0\x86_64\bin");
            //cmd.StandardInput.WriteLine("gst-launch-1.0 -v -e udpsrc caps=\"application/x-rtp," +
            //    " media=(string)video, clock-rate=(int)90000, encoding-name=(string)H264\" port=" +
            //    port + " ! rtph264depay ! h264parse ! mp4mux ! filesink location=" +
            //    DateTime.Now.ToString("MM-dd-yyyy--HH;mm;ss") + "(" + cam + ")" + ".mp4");
        }

        // closes all processes when the window closes
        private void Window_Closing(object sender, EventArgs e)
        {
            try
            {
                closeAll(recorders);
            }
            catch { }
            try
            {
                closeAll(players);
            }
            catch { }
        }
    }
}
