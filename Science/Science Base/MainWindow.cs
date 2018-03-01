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
        public static SolidColorBrush ScarletColour = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0x81, 0x14, 0x26));
        public static SolidColorBrush ScarletBackColour = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0x3F, 0x81, 0x14, 0x26));
        public static SolidColorBrush TextColour = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0xCC, 0xCC, 0xCC));
        public static SolidColorBrush GaugeRedColour = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0xD1, 0x26, 0x26));
        public static SolidColorBrush GaugeYellowColour = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0xD1, 0xD1, 0x26));
        public static SolidColorBrush GaugeGreenColour = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0x26, 0xD1, 0x26));

        private ChartManager Charts;

        public MainWindow()
        {
            ScarletColour.Freeze();
            ScarletBackColour.Freeze();
            TextColour.Freeze();
            GaugeRedColour.Freeze();
            GaugeYellowColour.Freeze();
            GaugeGreenColour.Freeze();

            InitializeComponent();
            InitWindow();
            this.EmergencyStopBtn.NotifyDefault(false);
            this.UIUpdate.Enabled = true;
            SetMode(this.StatusImgNetwork, Resources.Network, 2);
            SetMode(this.StatusImgPower, Resources.Power, 3);
            SetMode(this.StatusImgSystem, Resources.CPU, 3);
            this.Charts = new ChartManager(this.ChartLeft, this.ChartRight, this.ChartDataChooser);
            this.Charts.Left.AddSeries(DataHandler.RandomData);
        }

        private void InitWindow()
        {
            // Supply Voltage
            this.GaugeSysVoltage.FromValue = 22;
            this.GaugeSysVoltage.ToValue = 30;
            this.GaugeSysVoltage.Wedge = 240;
            this.GaugeSysVoltage.LabelsStep = 1;
            this.GaugeSysVoltage.TickStep = 0.25;
            this.GaugeSysVoltage.Value = 22;
            this.GaugeSysVoltage.SectionsInnerRadius = 0.96;
            this.GaugeSysVoltage.Base.Foreground = TextColour;
            this.GaugeSysVoltage.NeedleFill = TextColour;
            this.GaugeSysVoltage.Sections.Add(new AngularSection() // Low Red
            {
                FromValue = 22,
                ToValue = 22.5,
                Fill = GaugeRedColour
            });
            this.GaugeSysVoltage.Sections.Add(new AngularSection() // Low Yellow
            {
                FromValue = 22.5,
                ToValue = 23.5,
                Fill = GaugeYellowColour
            });
            this.GaugeSysVoltage.Sections.Add(new AngularSection() // Green
            {
                FromValue = 23.5,
                ToValue = 28.5,
                Fill = GaugeGreenColour
            });
            this.GaugeSysVoltage.Sections.Add(new AngularSection() // High Yellow
            {
                FromValue = 28.5,
                ToValue = 29.25,
                Fill = GaugeYellowColour
            });
            this.GaugeSysVoltage.Sections.Add(new AngularSection() // High Red
            {
                FromValue = 29.25,
                ToValue = 30,
                Fill = GaugeRedColour
            });

            // System Current
            this.GaugeSysCurrent.FromValue = 0;
            this.GaugeSysCurrent.ToValue = 10;
            this.GaugeSysCurrent.Wedge = 240;
            this.GaugeSysCurrent.LabelsStep = 2;
            this.GaugeSysCurrent.TickStep = 0.25;
            this.GaugeSysCurrent.SectionsInnerRadius = 0.96;
            this.GaugeSysCurrent.Base.Foreground = TextColour;
            this.GaugeSysCurrent.NeedleFill = TextColour;
            this.GaugeSysCurrent.Sections.Add(new AngularSection() // Green
            {
                FromValue = 0,
                ToValue = 6,
                Fill = GaugeGreenColour
            });
            this.GaugeSysCurrent.Sections.Add(new AngularSection() // High Yellow
            {
                FromValue = 6,
                ToValue = 8,
                Fill = GaugeYellowColour
            });
            this.GaugeSysCurrent.Sections.Add(new AngularSection() // High Red
            {
                FromValue = 8,
                ToValue = 10,
                Fill = GaugeRedColour
            });

            // Drill Current
            this.GaugeDrillCurrent.FromValue = 0;
            this.GaugeDrillCurrent.ToValue = 15;
            this.GaugeDrillCurrent.Wedge = 240;
            this.GaugeDrillCurrent.LabelsStep = 3;
            this.GaugeDrillCurrent.TickStep = 0.5;
            this.GaugeDrillCurrent.SectionsInnerRadius = 0.96;
            this.GaugeDrillCurrent.Base.Foreground = TextColour;
            this.GaugeDrillCurrent.NeedleFill = TextColour;
            this.GaugeDrillCurrent.Sections.Add(new AngularSection() // Green
            {
                FromValue = 0,
                ToValue = 9,
                Fill = GaugeGreenColour
            });
            this.GaugeDrillCurrent.Sections.Add(new AngularSection() // High Yellow
            {
                FromValue = 9,
                ToValue = 12,
                Fill = GaugeYellowColour
            });
            this.GaugeDrillCurrent.Sections.Add(new AngularSection() // High Red
            {
                FromValue = 12,
                ToValue = 15,
                Fill = GaugeRedColour
            });

            // Rail Current
            this.GaugeRailCurrent.FromValue = 0;
            this.GaugeRailCurrent.ToValue = 60;
            this.GaugeRailCurrent.Wedge = 240;
            this.GaugeRailCurrent.LabelsStep = 10;
            this.GaugeRailCurrent.TickStep = 2;
            this.GaugeRailCurrent.SectionsInnerRadius = 0.96;
            this.GaugeRailCurrent.Base.Foreground = TextColour;
            this.GaugeRailCurrent.NeedleFill = TextColour;
            this.GaugeRailCurrent.Sections.Add(new AngularSection() // Green
            {
                FromValue = 0,
                ToValue = 35,
                Fill = GaugeGreenColour
            });
            this.GaugeRailCurrent.Sections.Add(new AngularSection() // High Yellow
            {
                FromValue = 35,
                ToValue = 45,
                Fill = GaugeYellowColour
            });
            this.GaugeRailCurrent.Sections.Add(new AngularSection() // High Red
            {
                FromValue = 45,
                ToValue = 60,
                Fill = GaugeRedColour
            });

            
            this.ChartDataChooser.DisplayMember = "SeriesName";
            this.ChartDataChooser.ValueMember = null;
            this.ChartDataChooser.DataSource = DataHandler.GetSeries();
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

        public void UpdateGauges(double SupplyVoltage, double SystemCurrent, double DrillCurrent, double RailCurrent)
        {
            Invoke((MethodInvoker)delegate
            {
                this.GaugeSysVoltage.Value = SupplyVoltage;
                this.GaugeSysCurrent.Value = SystemCurrent;
                this.GaugeDrillCurrent.Value = DrillCurrent;
                this.GaugeRailCurrent.Value = RailCurrent;
            });
        }

        private void ChartClearLeft_Click(object sender, EventArgs e) => this.Charts.Left.Clear();
        private void ChartClearRight_Click(object sender, EventArgs e) => this.Charts.Right.Clear();
    }
}
