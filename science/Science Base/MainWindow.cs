using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RoboticsLibrary.Utilities;
using RoboticsLibrary.Communications;
using DarkUI.Forms;
using DarkUI.Docking;
using DarkUI.Controls;

namespace Science_Base
{
    public partial class MainWindow : DarkForm
    {
        public MainWindow()
        {
            InitializeComponent();
            this.EmergencyStopBtn.NotifyDefault(false);
        }

        private void EmergencyStopClick(object sender, EventArgs e)
        {
            Packet EmergencyStopPacket = new Packet((int)PacketType.StopPacket);
            CommHandler.SendAsyncPacket(EmergencyStopPacket);
        }

        private void SendPacketBtn_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] Timestamp = UtilMain.StringToBytes(this.TimestampTextbox.Text);
                byte[] ID = UtilMain.StringToBytes(this.IDTextbox.Text);
                byte[] Data = UtilMain.StringToBytes(this.DataTextbox.Text);
                Packet Pack = new Packet(ID.Last());
                Log.Output(Log.Severity.DEBUG, Log.Source.NETWORK, "Data len:" + Data.Length);
                Pack.AppendData(Data);
                Pack.SendWithTimestamp(Timestamp);
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
                DateTimeOffset DT = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(this.TimestampTextbox.Text.Replace(" ", ""), 16));
                this.InterpretationTimestamp.Text = DT.DateTime.ToLongDateString() + " " + DT.DateTime.ToLongTimeString() + " UTC";
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
                byte[] Data = UtilMain.StringToBytes(this.DataTextbox.Text);
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
            this.TimestampTextbox.Text = UtilMain.BytesToNiceString(Packet.GetTimestamp(), true);
        }

        private void SecTimer_Tick(object sender, EventArgs e)
        {
            UpdateTime();
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
