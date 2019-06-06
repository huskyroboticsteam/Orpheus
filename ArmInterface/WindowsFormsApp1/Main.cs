using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArmInterface;
using Scarlet.Communications;
using Scarlet.Utilities;
using static ArmControllerCommonFiles.Values;

namespace TestArmControllerGUI
{
    public partial class Main : Form
    {
        public const string CLIENT_IP = "192.168.7.2";
        public static int TCPPort = 1765;
        public static int UDPPort = 2765;

        private bool Connection;

        private static Dictionary<string, byte> DeviceNames, PacketNames;
        private static Parsing Parser;

        private static bool ERROR_MSG_FLG = false;      // RECEIVE  ( PRIORITY )

        public static Dictionary<Device, JointTelemetry> Telemetry;

        private string[] deviceNames, packetNames;

        public Main()
        {
            Parser = new Parsing();

            InitializeDumbDataStructures();

            InitializeComponent();
            PopulateComboBoxDefaults();
            
            Parser.UpdateGUI += new Parsing.Update(UpdateInformation);
            Server.Start(TCPPort, UDPPort);
            Server.ClientConnectionChange += new EventHandler<EventArgs>(SetConnectionState);
        }
        
        private void PopulateComboBoxDefaults()
        {
            deviceNames = Enum.GetNames(typeof(Device));
            packetNames = Enum.GetNames(typeof(CANPacket));

            devicecombobox.DataSource = deviceNames;
            packetcombobox.DataSource = packetNames;
        }

        private void SetConnectionState(object sender, EventArgs args)
        {
            string text;
            Color color;
            Connection = !Connection;
            if (Connection)
            {
                text = "CONNECTED";
                color = Color.Green;
            }
            else
            {
                text = "NOT CONNECTED";
                color = Color.Red;
            }
            connectionLabel.Invoke(new MethodInvoker(delegate { connectionLabel.Text = text; }));
            connectionLabel.Invoke(new MethodInvoker(delegate { connectionLabel.ForeColor = color; }));
        }

        public void UpdateInformation()
        {
            StringBuilder str = new StringBuilder();
            foreach (Device dev in Vals.DEVICES)
            {
                str.Append(dev.ToString() + " \n");
                str.Append("  CURRENT: " + Telemetry[dev].Current + "\n");
                str.Append("  VOLTAGE: " + Telemetry[dev].Voltage + "\n");
                str.Append("  ENCODER: " + Telemetry[dev].Encoder + "\n");
                str.Append("  SERVO: " + Telemetry[dev].ServoPos + "\n");
                str.Append("  MODEL: " + Telemetry[dev].ModelNumber + "\n");
                str.Append("  ERROR: " + Telemetry[dev].ErrorCode + "\n");
            }
            statusBox.Invoke(new MethodInvoker(delegate { statusBox.Text = str.ToString(); }));
        }

        private void SendInformation()
        {
            byte PacketID = DeviceToCmdID((Device)DeviceNames[devicecombobox.Text]);
            string packetName = packetcombobox.Text;
            byte packet = PacketNames[packetName];
            byte[] Payload = StringToPayload(packet, payloadtextbox.Text);
            Scarlet.Communications.Message message = new Scarlet.Communications.Message(PacketID, Payload);
            Packet pack = new Packet(message, true, "ArmTestClient");
            Server.Send(pack);
        }

        private void SendBtn_Click(object sender, EventArgs e) { SendInformation(); }

        private byte[] StringToPayload(byte firstByte, string str)
        {
            string[] vals = str.Split(' ');
            byte[] byteVals = new byte[vals.Length + 1];

            for (int i = 0; i < vals.Length; i++)
            {
                if (vals[i].Length > 2)
                {
                    string conversionString = vals[i].Substring(2);
                    byteVals[i + 1] = byte.Parse(conversionString, System.Globalization.NumberStyles.HexNumber);
                }
            }
            byteVals[0] = firstByte;
            return byteVals;
        }


        private void InitializeDumbDataStructures()
        {
            DeviceNames = new Dictionary<string, byte>
            {
                { "BASE", (byte)Device.BASE },
                { "SHOULDER", (byte)Device.SHOULDER },
                { "ELBOW", (byte)Device.SHOULDER },
                { "WRIST", (byte)Device.WRIST },
                { "DIFFERENTIAL_1", (byte)Device.DIFFERENTIAL_1 },
                { "DIFFERENTIAL_2", (byte)Device.DIFFERENTIAL_2 },
                { "HAND", (byte)Device.HAND }
            };

            PacketNames = new Dictionary<string, byte>
            {
                { "ANGLE_MAX_SPD", (byte)CANPacket.ANGLE_MAX_SPD },
                { "ENCODER_CNT", (byte)CANPacket.ENCODER_CNT },
                { "ERROR_MSG", (byte)CANPacket.ERROR_MSG },
                { "INDEX", (byte)CANPacket.INDEX },
                { "LASER", (byte)CANPacket.LASER },
                { "MODEL_NUM", (byte)CANPacket.MODEL_NUM },
                { "MODEL_REQ", (byte)CANPacket.MODEL_REQ },
                { "MODE_SELECT", (byte)CANPacket.MODE_SELECT },
                { "RESET", (byte)CANPacket.RESET },
                { "SERVO", (byte)CANPacket.SERVO },
                { "SET_P", (byte)CANPacket.SET_P },
                { "SET_I", (byte)CANPacket.SET_I },
                { "SET_D", (byte)CANPacket.SET_D },
                { "SET_TICKS_PER_REV", (byte)CANPacket.SET_TICKS_PER_REV },
                { "SPEED_DIR", (byte)CANPacket.SPEED_DIR },
                { "STATUS", (byte)CANPacket.STATUS },
                { "TELEMETRY", (byte)CANPacket.TELEMETRY }
            };

            Telemetry = new Dictionary<Device, JointTelemetry>
            {
                { Device.BASE, new JointTelemetry() },
                { Device.SHOULDER, new JointTelemetry() },
                { Device.ELBOW, new JointTelemetry() },
                { Device.WRIST, new JointTelemetry() },
                { Device.DIFFERENTIAL_1, new JointTelemetry() },
                { Device.DIFFERENTIAL_2, new JointTelemetry() },
                { Device.HAND, new JointTelemetry() }
            };
        }
    }
}
