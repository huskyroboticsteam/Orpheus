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

namespace Science_Base
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void EmergencyStopClick(object sender, EventArgs e)
        {
            Packet EmergencyStopPacket = new Packet((int)PacketType.StopPacket);
            CommHandler.SendAsyncPacket(EmergencyStopPacket);
        }

        private void SendPacketBtn_Click(object sender, EventArgs e)
        {

        }

        private void TimestampTextbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void IDTextbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void DataTextbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void UseCurrentTime_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
