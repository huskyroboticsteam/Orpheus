using DarkUI.Forms;
using Scarlet.Communications;
using Scarlet.Utilities;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Windows.Forms.VisualStyles;
using Science.Library;
using LiveCharts.Wpf;
using Science_Base.Properties;

namespace Science_Base
{
    public partial class MainWindow : DarkForm
    {
        private ChartManager Charts;

        public MainWindow()
        {
            UIHelper.Init();
            InitializeComponent();
            InitWindow();
            this.EmergencyStopBtn.NotifyDefault(false);
            this.UIUpdate.Enabled = true;
            UIHelper.SetMode(this.StatusImgNetwork, Resources.Network, 2);
            UIHelper.SetMode(this.StatusImgPower, Resources.Power, 3);
            UIHelper.SetMode(this.StatusImgSystem, Resources.CPU, 3);
            this.Charts = new ChartManager(this.ChartLeft, this.ChartRight, null);
            UIHelper.LoadCharts(this.Charts);
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
            this.GaugeSysVoltage.Base.Foreground = UIHelper.TextColour;
            this.GaugeSysVoltage.NeedleFill = UIHelper.TextColour;
            this.GaugeSysVoltage.Sections.Add(new AngularSection() // Low Red
            {
                FromValue = 22,
                ToValue = 22.5,
                Fill = UIHelper.GaugeRedColour
            });
            this.GaugeSysVoltage.Sections.Add(new AngularSection() // Low Yellow
            {
                FromValue = 22.5,
                ToValue = 23.5,
                Fill = UIHelper.GaugeYellowColour
            });
            this.GaugeSysVoltage.Sections.Add(new AngularSection() // Green
            {
                FromValue = 23.5,
                ToValue = 28.5,
                Fill = UIHelper.GaugeGreenColour
            });
            this.GaugeSysVoltage.Sections.Add(new AngularSection() // High Yellow
            {
                FromValue = 28.5,
                ToValue = 29.25,
                Fill = UIHelper.GaugeYellowColour
            });
            this.GaugeSysVoltage.Sections.Add(new AngularSection() // High Red
            {
                FromValue = 29.25,
                ToValue = 30,
                Fill = UIHelper.GaugeRedColour
            });

            // System Current
            this.GaugeSysCurrent.FromValue = 0;
            this.GaugeSysCurrent.ToValue = 3;
            this.GaugeSysCurrent.Wedge = 240;
            this.GaugeSysCurrent.LabelsStep = 0.5;
            this.GaugeSysCurrent.TickStep = 0.125;
            this.GaugeSysCurrent.SectionsInnerRadius = 0.96;
            this.GaugeSysCurrent.Base.Foreground = UIHelper.TextColour;
            this.GaugeSysCurrent.NeedleFill = UIHelper.TextColour;
            this.GaugeSysCurrent.Sections.Add(new AngularSection() // Green
            {
                FromValue = 0,
                ToValue = 1.5,
                Fill = UIHelper.GaugeGreenColour
            });
            this.GaugeSysCurrent.Sections.Add(new AngularSection() // High Yellow
            {
                FromValue = 1.5,
                ToValue = 2.25,
                Fill = UIHelper.GaugeYellowColour
            });
            this.GaugeSysCurrent.Sections.Add(new AngularSection() // High Red
            {
                FromValue = 2.25,
                ToValue = 3,
                Fill = UIHelper.GaugeRedColour
            });

            // Drill Current
            this.GaugeDrillCurrent.FromValue = 0;
            this.GaugeDrillCurrent.ToValue = 5;
            this.GaugeDrillCurrent.Wedge = 240;
            this.GaugeDrillCurrent.LabelsStep = 1;
            this.GaugeDrillCurrent.TickStep = 0.25;
            this.GaugeDrillCurrent.SectionsInnerRadius = 0.96;
            this.GaugeDrillCurrent.Base.Foreground = UIHelper.TextColour;
            this.GaugeDrillCurrent.NeedleFill = UIHelper.TextColour;
            this.GaugeDrillCurrent.Sections.Add(new AngularSection() // Green
            {
                FromValue = 0,
                ToValue = 2,
                Fill = UIHelper.GaugeGreenColour
            });
            this.GaugeDrillCurrent.Sections.Add(new AngularSection() // High Yellow
            {
                FromValue = 2,
                ToValue = 3.5,
                Fill = UIHelper.GaugeYellowColour
            });
            this.GaugeDrillCurrent.Sections.Add(new AngularSection() // High Red
            {
                FromValue = 3.5,
                ToValue = 5,
                Fill = UIHelper.GaugeRedColour
            });

            // Rail Current
            this.GaugeRailCurrent.FromValue = 0;
            this.GaugeRailCurrent.ToValue = 8;
            this.GaugeRailCurrent.Wedge = 240;
            this.GaugeRailCurrent.LabelsStep = 2;
            this.GaugeRailCurrent.TickStep = 0.5;
            this.GaugeRailCurrent.SectionsInnerRadius = 0.96;
            this.GaugeRailCurrent.Base.Foreground = UIHelper.TextColour;
            this.GaugeRailCurrent.NeedleFill = UIHelper.TextColour;
            this.GaugeRailCurrent.Sections.Add(new AngularSection() // Green
            {
                FromValue = 0,
                ToValue = 5,
                Fill = UIHelper.GaugeGreenColour
            });
            this.GaugeRailCurrent.Sections.Add(new AngularSection() // High Yellow
            {
                FromValue = 5,
                ToValue = 7,
                Fill = UIHelper.GaugeYellowColour
            });
            this.GaugeRailCurrent.Sections.Add(new AngularSection() // High Red
            {
                FromValue = 7,
                ToValue = 8,
                Fill = UIHelper.GaugeRedColour
            });

            // Chart Series Chooser
            ListViewItem[] Items = new ListViewItem[DataHandler.GetSeries().Length];
            for (int i = 0; i < Items.Length; i++)
            {
                Items[i] = new ListViewItem(DataHandler.GetSeries()[i].ToString());
                Items[i].Tag = DataHandler.GetSeries()[i];
            }
            this.ChartDataChooser.Items.AddRange(Items);
        }

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
                byte[] Timestamp = UtilMain.StringToBytes(this.TimestampTextbox.Text).Reverse().ToArray();
                byte ID = UtilMain.StringToBytes(this.IDTextbox.Text)[0];
                byte[] Data = InterpretInput(this.DataTextbox.Text.ToCharArray());
                Packet Pack = new Packet(ID, this.SendAsUDP.Checked, this.ClientSelector.SelectedItem.ToString());
                Log.Output(Log.Severity.DEBUG, Log.Source.NETWORK, "Sending packet with data length: " + Data.Length);
                Pack.AppendData(Data);
                Server.Send(Pack);
                Log.Output(Log.Severity.INFO, Log.Source.GUI, "Sending custom packet: " + Pack.ToString());
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
            catch { this.InterpretationTimestamp.Text = "Unknown"; }
        }

        private void IDTextbox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                byte[] ID = UtilMain.StringToBytes(this.IDTextbox.Text);
                this.InterpretationID.Text = "0x" + UtilMain.BytesToNiceString(new byte[] { ID.Last() }, false);
            }
            catch { this.InterpretationID.Text = "Unknown"; }
        }

        private void DataTextbox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                byte[] Data = InterpretInput(this.DataTextbox.Text.ToCharArray());
                this.InterpretationData.Text = "0x" + UtilMain.BytesToNiceString(Data, true);
            }
            catch { this.InterpretationData.Text = "Unknown"; }
        }

        private void UseCurrentTime_CheckedChanged(object sender, EventArgs e)
        {
            this.TimestampTextbox.Enabled = !this.UseCurrentTime.Checked;
            this.SecTimer.Enabled = this.UseCurrentTime.Checked;
            UpdateTime();
        }

        private void UpdateTime() { this.TimestampTextbox.Text = UtilMain.BytesToNiceString(Packet.GetCurrentTime(), true); }

        private void SecTimer_Tick(object sender, EventArgs e) { UpdateTime(); }

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
            Log.Output(Log.Severity.INFO, Log.Source.GUI, "Base station shutting down, saving graph settings...");
            DataHandler.ShuttingDown = true;
            UIHelper.SaveCharts(this.Charts);
            Log.Output(Log.Severity.INFO, Log.Source.GUI, "Stopping server... (If this hangs, it should be safe to force close now)");
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
                if (Server.GetClients().Contains(ScienceConstants.CLIENT_NAME)) { UIHelper.SetMode(this.StatusImgNetwork, Resources.Network, 0); }
                else { UIHelper.SetMode(this.StatusImgNetwork, Resources.Network, 2); }
            });
        }

        private void DrillSpeed_ValueChanged(object sender, EventArgs e)
        {
            Science_Base.Controls.DrillSpeedChange(this.DrillSpeed.Value * (this.DrillReverse.Checked ? -1 : 1) * (Science_Base.Controls.IsDrillEnabled ? 1 : 0));
            this.DrillReverse.Enabled = (this.DrillSpeed.Value == 0 || !Science_Base.Controls.IsDrillEnabled);
        }

        private void DrillToggle_Click(object sender, EventArgs e)
        {
            Science_Base.Controls.IsDrillEnabled = !Science_Base.Controls.IsDrillEnabled;
            this.DrillToggle.Text = Science_Base.Controls.IsDrillEnabled ? "STOP" : "START";
            Science_Base.Controls.DrillSpeedChange(this.DrillSpeed.Value * (this.DrillReverse.Checked ? -1 : 1) * (Science_Base.Controls.IsDrillEnabled ? 1 : 0));
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

        private void ChartAddLeft_Click(object sender, EventArgs e)
        {
            foreach(int Selected in this.ChartDataChooser.SelectedIndices) { this.Charts.Left.AddByIndex(Selected); }
        }

        private void ChartAddRight_Click(object sender, EventArgs e)
        {
            foreach (int Selected in this.ChartDataChooser.SelectedIndices) { this.Charts.Right.AddByIndex(Selected); }
        }

        private void RailSpeed_ValueChanged(object sender, EventArgs e)
        {
            Science_Base.Controls.RailSpeedChange(this.RailSpeed.Value);
            this.railDisplay1.DrillLocation = this.RailSpeed.Value;
        }

        private void TopDepth_MouseEnter(object sender, EventArgs e) { this.railDisplay1.ShowDistanceTop = true; }
        private void TopDepth_MouseLeave(object sender, EventArgs e) { this.railDisplay1.ShowDistanceTop = false; }

        private void BottomDepth_MouseEnter(object sender, EventArgs e) { this.railDisplay1.ShowDistanceBottom = true; }
        private void BottomDepth_MouseLeave(object sender, EventArgs e) { this.railDisplay1.ShowDistanceBottom = false; }

        private void SampleTubeToggle_Click(object sender, EventArgs e)
        {
            Science_Base.Controls.SampleDoorState = !Science_Base.Controls.SampleDoorState;
            Science_Base.Controls.SampleDoorChange(Science_Base.Controls.SampleDoorState ? 180 : 50);
            this.SampleTubeStatus.Text = (Science_Base.Controls.SampleDoorState ? "Opening" : "Closing");
        }

        private void GoToTop_Click(object sender, EventArgs e)
        {
            Science_Base.Controls.RailTargetChange(true, float.NaN, this.ButtonWillDoRailInit);
        }

        private void GoToGround_Click(object sender, EventArgs e)
        {
            Science_Base.Controls.RailTargetChange(false, float.NaN);
        }

        private void RailGoCustom_Click(object sender, EventArgs e)
        {
            if (int.TryParse(this.RailDistEntry.Text, out int TargetDepth))
            {
                Science_Base.Controls.RailTargetChange(true, TargetDepth);
            }
        }

        private void RailGoCustomBottom_Click(object sender, EventArgs e)
        {
            if (int.TryParse(this.RailDistEntry.Text, out int TargetDepth))
            {
                Science_Base.Controls.RailTargetChange(false, TargetDepth);
            }
        }

        private void RailDistEntry_TextChanged(object sender, EventArgs e)
        {
            bool Enable = true;
            if (!int.TryParse(this.RailDistEntry.Text, out int TargetDepth)) { Enable = false; }
            if (TargetDepth > 500 || TargetDepth < -110) { Enable = false; }

            this.RailGoCustomTop.Enabled = Enable;
            this.RailGoCustomBottom.Enabled = Enable;
        }

        private bool ButtonWillDoRailInit = false;

        public void UpdateRail(float CurrentRailSpeed, float DistTop, float DistGround, float DistTarget, bool TargetTopRef, bool InitInProgress, bool InitDone)
        {
            Invoke((MethodInvoker) delegate
            {
                this.DistTopMeasurementLabel.Text = DistTop.ToString("N1") + " mm";
                this.DistBottomMeasurementLabel.Text = DistGround.ToString("N1") + " mm";
                if(InitDone) { this.railDisplay1.InitStatus = 2; }
                else if(InitInProgress) { this.railDisplay1.InitStatus = 1; }
                else { this.railDisplay1.InitStatus = 0; }
                if(!InitDone) // User needs to start init
                {
                    this.RailGoGround.Enabled = false;
                    this.RailDistEntry.Enabled = false;
                    this.RailGoCustomBottom.Enabled = false;
                    this.RailGoCustomTop.Enabled = false;
                    this.ButtonWillDoRailInit = true;
                }
                else // Enable controls
                {
                    this.RailGoGround.Enabled = true;
                    this.RailDistEntry.Enabled = true;
                    this.RailGoCustomBottom.Enabled = true;
                    this.RailGoCustomTop.Enabled = true;
                    this.ButtonWillDoRailInit = false;
                }
            });
        }

        private bool ButtonWillDoTTBInit = false;

        public void UpdateTurntable(float CurrentPosition, bool InitInProgress, bool InitDone)
        {
            Invoke((MethodInvoker)delegate
            {
                this.turntableDisplay1.Angle = (int)Math.Round(CurrentPosition);
                if (InitDone) { this.turntableDisplay1.InitStatus = 2; }
                else if (InitInProgress) { this.turntableDisplay1.InitStatus = 1; }
                else { this.turntableDisplay1.InitStatus = 0; }

                if (!InitDone) // User needs to start init
                {
                    this.TTBGoTo1.Enabled = false;
                    this.TTBGoTo2.Enabled = false;
                    this.TTBGoTo3.Enabled = false;
                    this.ButtonWillDoTTBInit = true;
                }
                else // Enable controls
                {
                    this.TTBGoTo1.Enabled = true;
                    this.TTBGoTo2.Enabled = true;
                    this.TTBGoTo3.Enabled = true;
                    this.ButtonWillDoTTBInit = true; // Yes, this is intended.
                }
            });
        }

        public void UpdateDrill(byte Speed, bool SampleDoorOpen)
        {
            Invoke((MethodInvoker)delegate
            {
                this.SampleTubeStatus.Text = SampleDoorOpen ? "OPEN" : "Closed";
            });
        }

        private void TTBGoHome_Click(object sender, EventArgs e)
        {
            Science_Base.Controls.TTBTargetChange(0, this.ButtonWillDoTTBInit);
        }

        private void TTBGoTo1_Click(object sender, EventArgs e)
        {
            Science_Base.Controls.TTBTargetChange(143);
        }

        private void TTBGoTo2_Click(object sender, EventArgs e)
        {
            Science_Base.Controls.TTBTargetChange(66);
        }

        private void TTBGoTo3_Click(object sender, EventArgs e)
        {
            Science_Base.Controls.TTBTargetChange(0);
        }

        private void TTBGoTo4_Click(object sender, EventArgs e)
        {
            Science_Base.Controls.TTBTargetChange(30);
        }

        private void DebugBar_Scroll(object sender, EventArgs e)
        {
            // Ooh, spooky.
            //Science_Base.Controls.SampleDoorChange((int)(this.DebugBar.Value * 3.6F));
        }

        private void DebugShipButton_Click(object sender, EventArgs e)
        {
            // Ooh, spooky.
            Science_Base.Controls.TTBTargetChange((int)(this.DebugBar.Value * 1.5F));
        }

        private void StartMicroscope_Click(object sender, EventArgs e)
        {
            Science_Base.Controls.ActivateMicroscope((byte)(this.UseAutofocus.Checked ? 0x01 : 0x00), 0);
        }

        private void MicroscopeFocusBar_Scroll(object sender, EventArgs e)
        {
            Science_Base.Controls.ActivateMicroscope(0x02, this.MicroscopeFocusBar.Value);
        }
    }
}
