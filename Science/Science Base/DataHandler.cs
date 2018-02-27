﻿using System;
using System.Collections.Generic;
using System.Threading;
using Scarlet.Communications;
using Scarlet.Components.Sensors;
using Scarlet.Utilities;
using Science.Library;

namespace Science_Base
{
    public static class DataHandler
    {

        //public static DataSet RandomData;
        public static DataSet ThermocoupleData;
        private static Random Random;

        public static void Start()
        {
            Random = new Random();
            //RandomData = new DataSet("Random", new string[] { "RandNumber" }, 500);
            ThermocoupleData = new DataSet("Thermocouple", new string[] { "IntTemp" }, 500);
            Parse.SetParseHandler(ScienceConstants.Packets.GND_SENSOR, PacketGroundSensor);
            //Thread DataAdder = new Thread(new ThreadStart(DoAdds));
            //DataAdder.Start();
        }

        public static void PacketGroundSensor(Packet Packet)
        {
            if(Packet == null || Packet.Data == null || Packet.Data.Payload == null || Packet.Data.Payload.Length != 8)
            {
                Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "Ground sensor packet invalid. Discarding.");
                return;
            }
            //float SoilMoisture = UtilData.ToFloat(UtilMain.SubArray(Packet.Data.Payload, 0, 4));
            int UVLight = UtilData.ToInt(UtilMain.SubArray(Packet.Data.Payload, 0, 4));
            uint Thermocouple = UtilData.ToUInt(UtilMain.SubArray(Packet.Data.Payload, 4, 4));
            ThermocoupleData.Add(new DataUnit("MAX31855")
            {
                { "Time", DateTime.Now },
                { "IntTemp", MAX31855.ConvertInternalFromRaw(Thermocouple) }
            });
        }

        /*private static void DoAdds()
        {
            Log.Output(Log.Severity.INFO, Log.Source.GUI, "Beginning data addition.");
            Thread.Sleep(5000);
            for (int i = 0; i < 600; i++)
            {
                Thread.Sleep(150);
                DataUnit ToAdd = new DataUnit("Garbage")
                {
                    { "Time", DateTime.Now },
                    { "RandNumber", (int)Math.Round(Math.Sin(i * Math.PI / 20) * 50 + 50) }
                };
                lock(RandomData) { RandomData.Add(ToAdd); }
            }
        }*/

    }
}