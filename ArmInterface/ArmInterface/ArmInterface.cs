using Scarlet.IO;
using Scarlet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArmInterface
{

    public struct ArmPacket
    {
        /// <summary> When sending, it's the receiver. When receiving, it's the sender. </summary>
        public Device TargetDeviceID;
        public bool Priority;
        public CANPacket PacketType;
        public byte[] Payload;
    }

    public class Arm
    {
        public static byte DEVICE_ADDR = 0x02;

        private bool _readEnable = true;

        public bool ReadEnable
        {
            get { return _readEnable; }
            set
            {
                if (value == true && _readEnable == false)
                {
                    ReadThread?.Start();
                }
                _readEnable = value;
            }
        }

        private readonly ICANBus CANBus;
        private readonly Thread ReadThread;

        private Queue<ArmPacket> ReceiveQueue;

        public Arm(ICANBus CANBus)
        {
            ReceiveQueue = new Queue<ArmPacket>();
            this.CANBus = CANBus;
            // Start Read
            ReadThread = new Thread(new ThreadStart(ReadOnThread));
            ReadThread.Start();
        }

        public void Send(ArmPacket SendPacket)
        {
            byte[] id = ConstructCanID(SendPacket);
            uint constructedID = (uint)(id[2] << 8) | (uint)(id[1] << 4) | id[0];
            CANBus.Write(constructedID, SendPacket.Payload);
        }

        private void ReadOnThread()
        {
            while (ReadEnable)
            {
                // Read
                Tuple<uint, byte[]> CanRead = CANBus.Read();

                byte priority = Convert.ToByte(((CanRead.Item1) >> 8) & 0x01);
                byte sender = Convert.ToByte(((CanRead.Item1) >> 4) & 0x0F);
                byte receiver = Convert.ToByte((CanRead.Item1) & 0x0F);

                ArmPacket newPack = new ArmPacket
                {
                    TargetDeviceID = (Device)sender,
                    Priority = priority == 0 ? false : true,
                    PacketType = (CANPacket)CanRead.Item2[0],
                    Payload = CanRead.Item2
                };
                
                lock (ReceiveQueue)
                {
                    ReceiveQueue.Enqueue(newPack);
                    ReceiveQueue.TrimExcess();
                }

                Thread.Sleep(Constants.DEFAULT_MIN_THREAD_SLEEP);
            }
        }

        private static byte[] ConstructCanID(ArmPacket packet)
        {
            byte[] canID = new byte[3];
            canID[2] = (byte)(packet.Priority ? 0x01 : 0x00);
            canID[1] = DEVICE_ADDR;
            canID[0] = (byte)packet.TargetDeviceID;
            return canID;
        }

        public ArmPacket? ReadNext()
        {
            return ReceiveQueue.Count == 0 ? (ArmPacket?)null : ReceiveQueue.Dequeue();
        }

        public int ReceiveQueueSize() { return ReceiveQueue.Count; }

    }
}
