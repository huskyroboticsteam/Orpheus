using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenTK.Input;
using Scarlet.Communications;
using Scarlet.Utilities;

// this code will be used for the basestation and takes in all of the controller inputs then sends
// them to the rover side. Whichever is client or server side has yet to be determined

namespace ControllerServer
{
    class Program
    {
        private static byte PacketID = 0; // Packet ID must be formalized, 0 is just a placeholder
        static void Main(string[] args)
        {
            int PortTCP = 5287;
            int PortUDP = 5288;
            Parse.SetParseHandler(PacketID, PrintPacketData);

            Log.SetGlobalOutputLevel(Log.Severity.ERROR);
            Server.Start(PortTCP, PortUDP);
            SendGamePadData();
        }

        // sends the gamepad state data to the client named Controller Parser every 50ms
        private static void SendGamePadData()
        {
            while (true)
            {
                if (Server.GetClients().Contains("Controller Parser"))
                {
                    GamePadState state = GamePad.GetState(0);
                    Packet info = new Packet(new Message(PacketID,
                        UtilData.ToBytes(state.ThumbSticks.Left.X)), true, "Controller Parser");
                    info.AppendData(UtilData.ToBytes(state.ThumbSticks.Left.Y));
                    info.AppendData(UtilData.ToBytes(state.ThumbSticks.Right.X));
                    info.AppendData(UtilData.ToBytes(state.ThumbSticks.Right.Y));
                    info.AppendData(UtilData.ToBytes(state.Triggers.Right));
                    info.AppendData(UtilData.ToBytes(state.Triggers.Left));
                    info.AppendData(UtilData.ToBytes((int)state.DPad.Up));
                    info.AppendData(UtilData.ToBytes((int)state.DPad.Down));
                    info.AppendData(UtilData.ToBytes((int)state.DPad.Left));
                    info.AppendData(UtilData.ToBytes((int)state.DPad.Right));
                    info.AppendData(UtilData.ToBytes((int)state.Buttons.RightShoulder));
                    info.AppendData(UtilData.ToBytes((int)state.Buttons.LeftShoulder));
                    info.AppendData(UtilData.ToBytes((int)state.Buttons.A));
                    info.AppendData(UtilData.ToBytes((int)state.Buttons.B));
                    info.AppendData(UtilData.ToBytes((int)state.Buttons.X));
                    info.AppendData(UtilData.ToBytes((int)state.Buttons.Y));
                    info.AppendData(UtilData.ToBytes((int)state.Buttons.RightStick));
                    info.AppendData(UtilData.ToBytes((int)state.Buttons.LeftStick));
                    info.AppendData(UtilData.ToBytes((int)state.Buttons.Back));
                    info.AppendData(UtilData.ToBytes((int)state.Buttons.Start));
                    info.AppendData(UtilData.ToBytes((int)state.Buttons.BigButton));

                    Server.Send(info);
                }
                Thread.Sleep(50);
            }
        }

        // package handler prints package contents to console
        private static void PrintPacketData(Packet packet)
        {
            Console.WriteLine(UtilData.ToString(packet.Data.Payload));
        }

    }
}
