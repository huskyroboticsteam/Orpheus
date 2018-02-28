using DarkUI.Forms;
using Scarlet.Communications;
using Scarlet.Utilities;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Net;
using Science.Library;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using LiveCharts;
using System.Windows.Media;
using System.Drawing.Imaging;
using System.Drawing;
using Science_Base.Properties;

namespace Science_Base
{
    public partial class MainWindow : DarkForm
    {
        public MainWindow()
        {
            InitializeComponent();
            InitWindow();
            this.EmergencyStopBtn.NotifyDefault(false);
            this.UIUpdate.Enabled = true;
            SetMode(this.StatusImgNetwork, Resources.Network, 2);
            SetMode(this.StatusImgPower, Resources.Power, 3);
            SetMode(this.StatusImgSystem, Resources.CPU, 3);
        }

        private void InitWindow()
        {
            SolidColorBrush Red = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0xD1, 0x26, 0x26));
            SolidColorBrush Yellow = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0xD1, 0xD1, 0x26));
            SolidColorBrush Green = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0x26, 0xD1, 0x26));
            Red.Freeze();
            Yellow.Freeze();
            Green.Freeze();

            this.DataGraph.AnimationsSpeed = TimeSpan.FromMilliseconds(100);
            // Supply Voltage
            this.GaugeSysVoltage.FromValue = 22;
            this.GaugeSysVoltage.ToValue = 30;
            this.GaugeSysVoltage.Wedge = 240;
            this.GaugeSysVoltage.LabelsStep = 1;
            this.GaugeSysVoltage.TickStep = 0.25;
            this.GaugeSysVoltage.Value = 22;
            this.GaugeSysVoltage.SectionsInnerRadius = 0.96;
            this.GaugeSysVoltage.Sections.Add(new AngularSection() // Low Red
            {
                FromValue = 22,
                ToValue = 22.5,
                Fill = Red
            });
            this.GaugeSysVoltage.Sections.Add(new AngularSection() // Low Yellow
            {
                FromValue = 22.5,
                ToValue = 23.5,
                Fill = Yellow
            });
            this.GaugeSysVoltage.Sections.Add(new AngularSection() // Green
            {
                FromValue = 23.5,
                ToValue = 28.5,
                Fill = Green
            });
            this.GaugeSysVoltage.Sections.Add(new AngularSection() // High Yellow
            {
                FromValue = 28.5,
                ToValue = 29.25,
                Fill = Yellow
            });
            this.GaugeSysVoltage.Sections.Add(new AngularSection() // High Red
            {
                FromValue = 29.25,
                ToValue = 30,
                Fill = Red
            });

            // System Current
            this.GaugeSysCurrent.FromValue = 0;
            this.GaugeSysCurrent.ToValue = 10;
            this.GaugeSysCurrent.Wedge = 240;
            this.GaugeSysCurrent.LabelsStep = 2;
            this.GaugeSysCurrent.TickStep = 0.25;
            this.GaugeSysCurrent.SectionsInnerRadius = 0.96;
            this.GaugeSysCurrent.Sections.Add(new AngularSection() // Green
            {
                FromValue = 0,
                ToValue = 6,
                Fill = Green
            });
            this.GaugeSysCurrent.Sections.Add(new AngularSection() // High Yellow
            {
                FromValue = 6,
                ToValue = 8,
                Fill = Yellow
            });
            this.GaugeSysCurrent.Sections.Add(new AngularSection() // High Red
            {
                FromValue = 8,
                ToValue = 10,
                Fill = Red
            });

            // Drill Current
            this.GaugeDrillCurrent.FromValue = 0;
            this.GaugeDrillCurrent.ToValue = 15;
            this.GaugeDrillCurrent.Wedge = 240;
            this.GaugeDrillCurrent.LabelsStep = 3;
            this.GaugeDrillCurrent.TickStep = 0.5;
            this.GaugeDrillCurrent.SectionsInnerRadius = 0.96;
            this.GaugeDrillCurrent.Sections.Add(new AngularSection() // Green
            {
                FromValue = 0,
                ToValue = 9,
                Fill = Green
            });
            this.GaugeDrillCurrent.Sections.Add(new AngularSection() // High Yellow
            {
                FromValue = 9,
                ToValue = 12,
                Fill = Yellow
            });
            this.GaugeDrillCurrent.Sections.Add(new AngularSection() // High Red
            {
                FromValue = 12,
                ToValue = 15,
                Fill = Red
            });

            // Rail Current
            this.GaugeRailCurrent.FromValue = 0;
            this.GaugeRailCurrent.ToValue = 60;
            this.GaugeRailCurrent.Wedge = 240;
            this.GaugeRailCurrent.LabelsStep = 10;
            this.GaugeRailCurrent.TickStep = 2;
            this.GaugeRailCurrent.SectionsInnerRadius = 0.96;
            this.GaugeRailCurrent.Sections.Add(new AngularSection() // Green
            {
                FromValue = 0,
                ToValue = 35,
                Fill = Green
            });
            this.GaugeRailCurrent.Sections.Add(new AngularSection() // High Yellow
            {
                FromValue = 35,
                ToValue = 45,
                Fill = Yellow
            });
            this.GaugeRailCurrent.Sections.Add(new AngularSection() // High Red
            {
                FromValue = 45,
                ToValue = 60,
                Fill = Red
            });
        }

        public void SetMode(PictureBox Box, Image Original, byte Mode)
        {
            float[][] Good =
            {
                new float[] { 0, 0, 0, 0, 0 },
                new float[] { 0, 0.8F, 0, 0, 0 },
                new float[] { 0, 0, 0, 0, 0 },
                new float[] { 0, 0, 0, 1, 0 },
                new float[] { 0, 0, 0, 0, 0 }
            };

            float[][] Warning =
            {
                new float[] { 0.8F, 0, 0, 0, 0 },
                new float[] { 0, 0.8F, 0, 0, 0 },
                new float[] { 0, 0, 0, 0, 0 },
                new float[] { 0, 0, 0, 1, 0 },
                new float[] { 0, 0, 0, 0, 0 }
            };

            float[][] Error =
            {
                new float[] { 0.8F, 0, 0, 0, 0 },
                new float[] { 0, 0, 0, 0, 0 },
                new float[] { 0, 0, 0, 0, 0 },
                new float[] { 0, 0, 0, 1, 0 },
                new float[] { 0, 0, 0, 0, 0 }
            };

            float[][] Default =
            {
                new float[] { 0.8F, 0, 0, 0, 0 },
                new float[] { 0, 0.8F, 0, 0, 0 },
                new float[] { 0, 0, 0.8F, 0, 0 },
                new float[] { 0, 0, 0, 1, 0 },
                new float[] { 0, 0, 0, 0, 0 }
            };

            ColorMatrix Matrix;
            switch(Mode)
            {
                case 0: Matrix = new ColorMatrix(Good); break;
                case 1: Matrix = new ColorMatrix(Warning); break;
                case 2: Matrix = new ColorMatrix(Error); break;
                default: Matrix = new ColorMatrix(Default); break;
            }
            ImageAttributes Attributes = new ImageAttributes();
            Attributes.SetColorMatrix(Matrix);
            Image Image = (Image)Original.Clone();
            Graphics Graphics = Graphics.FromImage(Image);
            Rectangle Output = new Rectangle(0, 0, Image.Width, Image.Height);
            Graphics.DrawImage(Image, Output, 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, Attributes);
            Box.Image = Image;
        }

        public void StartData()
        {
            Axis XAxis = new Axis()
            {
                //MinValue = DateTime.Now.AddSeconds(5).Ticks,
                //MaxValue = DateTime.Now.AddSeconds(35).Ticks,
                LabelFormatter = value => new DateTime((long)value).ToString("T"),
                DisableAnimations = true
            };
            Axis YAxis = new Axis()
            {
                //MinValue = 0,
                //MaxValue = 100
            };
            SolidColorBrush Red = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0x81, 0x14, 0x26));
            SolidColorBrush Back = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0x3F, 0x81, 0x14, 0x26));
            Red.Freeze();
            Back.Freeze();
            LineSeries Series = new LineSeries(DataSet.GetMapper("IntTemp"))
            {
                Values = new ChartValues<DataUnit>(),
                Stroke = Red,
                Fill = Back
            };
            this.DataGraph.Series.Add(Series);
            this.DataGraph.AxisX.Add(XAxis);
            this.DataGraph.AxisY.Add(YAxis);
            this.DataGraph.DisableAnimations = true;
            DataHandler.ThermocoupleData.ItemAdd += this.DataAdded;
        }

        public void DataAdded(object Sender, DataEvent Event)
        {
            this.DataGraph.Series.First().Values.Add(Event.Unit);
        }

        /*public void UpdateGraph()
        {
            Invoke((MethodInvoker)delegate
            {

                this.cartesianChart1.Series.First().Values = new ChartValues<DataUnit>(DataHandler.RandomData);
                this.cartesianChart1.Update();
                Log.Output(Log.Severity.INFO, Log.Source.GUI, "Updating graph");
            });
        }*/

        private void EmergencyStopClick(object sender, EventArgs e)
        {
            try
            {
                Packet EmergencyStopPacket = new Packet(ScienceConstants.Packets.EMERGENCY_STOP, false, ScienceConstants.CLIENT_NAME);
                EmergencyStopPacket.AppendData(UtilData.ToBytes("Homura"));
                Server.SendNow(EmergencyStopPacket);
            }
            catch(Exception Exc)
            {
                Log.Output(Log.Severity.FATAL, Log.Source.GUI, "FAILED TO SEND EMERGENCY STOP!");
                Log.Exception(Log.Source.GUI, Exc);
                DarkMessageBox.ShowError("Failed to send emergency stop!\n\n" + Exc.ToString(), "Science");
            }
        }

        private void SendPacketBtn_Click(object sender, EventArgs e)
        {
            try
            {
                for(int i = 0; i < 100; i++)
                {
                    byte[] Timestamp = UtilMain.StringToBytes(this.TimestampTextbox.Text).Reverse().ToArray();
                    byte ID = UtilMain.StringToBytes(this.IDTextbox.Text)[0];
                    byte[] Data = InterpretInput(this.DataTextbox.Text.ToCharArray());
                    Packet Pack = new Packet(ID, this.SendAsUDP.Checked, this.ClientSelector.SelectedItem.ToString());
                    Log.Output(Log.Severity.DEBUG, Log.Source.NETWORK, "Sending packet with data length: " + Data.Length);
                    Pack.AppendData(Data);
                    Server.Send(Pack);
                    Log.Output(Log.Severity.INFO, Log.Source.GUI, "Sending custom packet: " + Pack.ToString());
                }
            }
            catch(Exception Exc)
            {
                Log.Output(Log.Severity.ERROR, Log.Source.NETWORK, "Something went wrong while sending custom packet.");
                Log.Exception(Log.Source.NETWORK, Exc);
            }
        }

        private void TimestampTextbox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime ParsedTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                ParsedTime = ParsedTime.AddSeconds(Convert.ToInt32(this.TimestampTextbox.Text.Replace(" ", ""), 16)).ToLocalTime();
                this.InterpretationTimestamp.Text = ParsedTime.ToLongDateString() + " " + ParsedTime.ToLongTimeString() + " UTC";
            }
            catch
            {
                this.InterpretationTimestamp.Text = "Unknown";
            }
        }

        private void IDTextbox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                byte[] ID = UtilMain.StringToBytes(this.IDTextbox.Text);
                this.InterpretationID.Text = "0x" + UtilMain.BytesToNiceString(new byte[] { ID.Last() }, false);
            }
            catch
            {
                this.InterpretationID.Text = "Unknown";
            }
        }

        private void DataTextbox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                byte[] Data = InterpretInput(this.DataTextbox.Text.ToCharArray());
                this.InterpretationData.Text = "0x" + UtilMain.BytesToNiceString(Data, true);
            }
            catch
            {
                this.InterpretationData.Text = "Unknown";
            }
        }

        private void UseCurrentTime_CheckedChanged(object sender, EventArgs e)
        {
            this.TimestampTextbox.Enabled = !this.UseCurrentTime.Checked;
            this.SecTimer.Enabled = this.UseCurrentTime.Checked;
            UpdateTime();
        }

        private void UpdateTime()
        {
            this.TimestampTextbox.Text = UtilMain.BytesToNiceString(Packet.GetCurrentTime(), true);
        }

        private void SecTimer_Tick(object sender, EventArgs e)
        {
            UpdateTime();
        }

        private byte[] InterpretInput(char[] Input)
        {
            if(!Input.Contains('"')) { return UtilMain.StringToBytes(new string(Input)).Reverse().ToArray(); }
            int LocOfStart = Array.IndexOf(Input, '"');
            int StrLen = Array.IndexOf(Input.Skip(LocOfStart + 1).ToArray(), '"');
            char[] BeforeChars = Input.Take(LocOfStart).ToArray();
            List<byte> Output = new List<byte>();
            Output.AddRange(UtilMain.StringToBytes(new string(BeforeChars)).Reverse().ToArray());
            string Str = new string(Input.Skip(LocOfStart + 1).Take(StrLen).ToArray());
            Output.AddRange(UtilData.ToBytes(Str));
            Output.AddRange(InterpretInput(Input.Skip(LocOfStart + StrLen + 2).ToArray()));
            return Output.ToArray();
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Server.Stop();
            Environment.Exit(0);
        }

        private void UIUpdate_Tick(object sender, EventArgs e)
        {
            //this.StatsPacketQueueOut.Text = "Packet Queue Out: " + CommHandler.GetSendQueueLength();
            //this.StatsPacketQueueIn.Text = "Packet Queue In: " + CommHandler.GetReceiveQueueLength();
        }

        public void UpdateClientList(object Sender, EventArgs Event)
        {
            Invoke((MethodInvoker)delegate
            {
                this.ClientSelector.Items.Clear();
                this.ClientSelector.Items.AddRange(Server.GetClients().ToArray());
                if (Server.GetClients().Contains(ScienceConstants.CLIENT_NAME)) { SetMode(this.StatusImgNetwork, Resources.Network, 0); }
                else { SetMode(this.StatusImgNetwork, Resources.Network, 2); }
            });
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            Science_Base.Controls.DrillSpeedChange(this.DrillSpeed.Value * (this.DrillReverse.Checked ? -1 : 1));
            this.DrillReverse.Enabled = (this.DrillSpeed.Value == 0 || !Science_Base.Controls.IsDrillEnabled);
        }

        private void darkButton1_Click(object sender, EventArgs e)
        {
            Science_Base.Controls.IsDrillEnabled = !Science_Base.Controls.IsDrillEnabled;
            this.DrillToggle.Text = Science_Base.Controls.IsDrillEnabled ? "STOP" : "START";
            Science_Base.Controls.DrillSpeedChange(this.DrillSpeed.Value * (this.DrillReverse.Checked ? -1 : 1));
            this.DrillReverse.Enabled = (this.DrillSpeed.Value == 0 || !Science_Base.Controls.IsDrillEnabled);
        }
    }
}
