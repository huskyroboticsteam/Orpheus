using System;
using System.Collections.Generic;
using System.Threading;
using Scarlet.Communications;
using Scarlet.Utilities;
using Science.Library;

namespace Science_Base
{
    public static class DataHandler
    {

        public static List<DataUnit> RandomData;
        private static Random Random;

        public static void Start()
        {
            Random = new Random();
            RandomData = new List<DataUnit>(500);
            Parse.SetParseHandler(ScienceConstants.Packets.GND_SENSOR, PacketGroundSensor);
            Thread DataAdder = new Thread(new ThreadStart(DoAdds));
            DataAdder.Start();
        }

        public static void PacketGroundSensor(Packet Packet)
        {
            if(Packet == null || Packet.Data == null || Packet.Data.Payload == null || Packet.Data.Payload.Length != 8)
            {
                Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "Ground sensor packet invalid. Discarding.");
                return;
            }
            float SoilMoisture = UtilData.ToFloat(UtilMain.SubArray(Packet.Data.Payload, 0, 4));
            uint Thermocouple = UtilData.ToUInt(UtilMain.SubArray(Packet.Data.Payload, 4, 4));
        }

        private static void DoAdds()
        {
            Log.Output(Log.Severity.INFO, Log.Source.GUI, "Beginning data addition.");
            for (int i = 0; i < 20; i++)
            {
                Thread.Sleep(500);
                DataUnit ToAdd = new DataUnit("Garbage")
                {
                    { "Time", DateTime.Now },
                    { "RandNumber", Random.Next(100) }
                };
                lock(RandomData) { RandomData.Add(ToAdd); }
                BaseMain.Window.UpdateGraph();
            }
        }

    }
}
