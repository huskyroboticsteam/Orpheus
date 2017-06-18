using DarkUI.Forms;
using Scarlet.Communications;
using Scarlet.Science;
using Scarlet.Utilities;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

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
            Packet EmergencyStopPacket = new Packet(PacketType.EMERGENCY_STOP);
            EmergencyStopPacket.AppendData(UtilData.ToBytes("Homura"));
            CommHandler.SendAsyncPacket(EmergencyStopPacket);
        }

        private void SendPacketBtn_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] Timestamp = UtilMain.StringToBytes(this.TimestampTextbox.Text).Reverse().ToArray();
                byte ID = UtilMain.StringToBytes(this.IDTextbox.Text)[0];
                byte[] Data = UtilMain.StringToBytes(this.DataTextbox.Text).Reverse().ToArray();
                Packet Pack = new Packet(ID);
                Log.Output(Log.Severity.DEBUG, Log.Source.NETWORK, "Sending packet with data length: " + Data.Length);
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
                byte[] Data = InterpretInput(this.DataTextbox.Text.ToCharArray()).Reverse().ToArray();//UtilMain.StringToBytes(this.DataTextbox.Text);
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
            this.TimestampTextbox.Text = UtilMain.BytesToNiceString(Packet.GetTimestamp().Reverse().ToArray(), true);
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
            CommHandler.Stop();
            Environment.Exit(0);
        }
    }
}
