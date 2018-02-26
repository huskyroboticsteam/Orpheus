using DarkUI.Forms;
using Scarlet.Communications;
using Scarlet.Utilities;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Net;
using Science.Library;

namespace Science_Base
{
    public partial class MainWindow : DarkForm
    {
        public MainWindow()
        {
            InitializeComponent();
            this.EmergencyStopBtn.NotifyDefault(false);
            this.UIUpdate.Enabled = true;
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
            });
        }
    }
}
