using System;
using System.Threading;
using System.Windows.Threading;
using Scarlet.Communications;
using Scarlet.Components.Sensors;
using Scarlet.Utilities;
using Science.Library;

namespace Science_Base
{
    public static class DataHandler
    {
        public static DataSeries<float> ThermoInt = new DataSeries<float>("Thermocouple Internal", "Sensor Temperature (°C)");
        public static DataSeries<float> ThermoExt = new DataSeries<float>("Thermocouple External", "Ground Temperature (°C)");
        public static DataSeries<int> UV = new DataSeries<int>("UV Light", "UV Light (µW/cm²)");
        public static DataSeries<float> AirPollution = new DataSeries<float>("Air Pollution", "Air Pollution (PPM)");
        public static DataSeries<double> AirTemp = new DataSeries<double>("Air Temperature", "Temperature (°C)");
        public static DataSeries<double> AirHumidity = new DataSeries<double>("Air Humidity", "Relative Humidity (%)");
        public static DataSeries<double> AirPressure = new DataSeries<double>("Air Pressure", "Pressure (Pa)");
        public static DataSeries<float> SoilMoisture = new DataSeries<float>("Soil Moisture", "Volumetric Water Content (%)");
        // TODO: Add the rest of the serieses.

        public static DataSeries<double> SupplyVoltage = new DataSeries<double>("Supply Voltage", "Supply Voltage (V)");
        public static DataSeries<double> SystemCurrent = new DataSeries<double>("System Current", "System Current (A)");
        public static DataSeries<double> DrillCurrent = new DataSeries<double>("Drill Current", "Drill Current (A)");
        public static DataSeries<double> RailCurrent = new DataSeries<double>("Rail Current", "Rail Current (A)");

        public static DataSeries<double> AIn0 = new DataSeries<double>("Analogue 0", "Analogue Input");
        public static DataSeries<double> AIn1 = new DataSeries<double>("Analogue 1", "Analogue Input");

        public static DataSeries<int> RandomData = new DataSeries<int>("Random", "Rubbish");
        private static Random Random;

        public static void Start()
        {
            Random = new Random();
            Parse.SetParseHandler(ScienceConstants.Packets.GND_SENSOR, PacketGroundSensor);
            Parse.SetParseHandler(ScienceConstants.Packets.SYS_SENSOR, PacketSysSensor);
            Parse.SetParseHandler(ScienceConstants.Packets.RAIL_STATUS, PacketRailStatus);
            new Thread(new ThreadStart(DoAdds)).Start();
        }

        public static object[] GetSeries()
        {
            return new object[] { ThermoInt, ThermoExt, UV, AirPollution, AirTemp, AirHumidity, AirPressure, SoilMoisture, SupplyVoltage, SystemCurrent, DrillCurrent, RailCurrent, RandomData, AIn0, AIn1 };
        }

        public static void PacketGroundSensor(Packet Packet)
        {
            if(Packet == null || Packet.Data == null || Packet.Data.Payload == null || Packet.Data.Payload.Length != 40)
            {
                Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "Ground sensor packet invalid. Discarding. Length: " + Packet?.Data?.Payload?.Length);
                return;
            }
            DateTime Timestamp = DateTime.Now;
            int UVLight = UtilData.ToInt(UtilMain.SubArray(Packet.Data.Payload, 0, 4));
            float AirQuality = UtilData.ToFloat(UtilMain.SubArray(Packet.Data.Payload, 4, 4));
            float SoilMoist = UtilData.ToFloat(UtilMain.SubArray(Packet.Data.Payload, 8, 4));
            uint ThermocoupleData = UtilData.ToUInt(UtilMain.SubArray(Packet.Data.Payload, 12, 4));
            double AtmoTemp = UtilData.ToDouble(UtilMain.SubArray(Packet.Data.Payload, 16, 8));
            double AtmoPres = UtilData.ToDouble(UtilMain.SubArray(Packet.Data.Payload, 24, 8));
            double AtmoHumid = UtilData.ToDouble(UtilMain.SubArray(Packet.Data.Payload, 32, 8));

            UV.Data.Add(new Datum<int>(Timestamp, UVLight));
            AirPollution.Data.Add(new Datum<float>(Timestamp, AirQuality));
            SoilMoisture.Data.Add(new Datum<float>(Timestamp, SoilMoist));
            ThermoExt.Data.Add(new Datum<float>(Timestamp, MAX31855.ConvertExternalFromRaw(ThermocoupleData)));
            ThermoInt.Data.Add(new Datum<float>(Timestamp, MAX31855.ConvertInternalFromRaw(ThermocoupleData)));
            AirTemp.Data.Add(new Datum<double>(Timestamp, AtmoTemp));
            AirPressure.Data.Add(new Datum<double>(Timestamp, AtmoPres));
            AirHumidity.Data.Add(new Datum<double>(Timestamp, AtmoHumid));
        }

        public static void PacketSysSensor(Packet Packet)
        {
            if (Packet == null || Packet.Data == null || Packet.Data.Payload == null || Packet.Data.Payload.Length != 40)
            {
                Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "System sensor packet invalid. Discarding. Length: " + Packet?.Data?.Payload?.Length);
                return;
            }
            double SysA = UtilData.ToDouble(UtilMain.SubArray(Packet.Data.Payload, 0, 8));
            double DrillA = UtilData.ToDouble(UtilMain.SubArray(Packet.Data.Payload, 8, 8));
            double RailA = UtilData.ToDouble(UtilMain.SubArray(Packet.Data.Payload, 16, 8));
            double SysV = UtilData.ToDouble(UtilMain.SubArray(Packet.Data.Payload, 24, 8));

            DateTime Sample = new DateTime(UtilData.ToLong(UtilMain.SubArray(Packet.Data.Payload, 32, 8)));
            //Log.Output(Log.Severity.INFO, Log.Source.GUI, "Got sysA:" + SysCurrent + ", DrlA:" + DrillCurrent + ", SysV:" + SysVoltage);
            BaseMain.Window.UpdateGauges(SysV, SysA, DrillA, RailA);
            SupplyVoltage.Data.Add(new Datum<double>(Sample, SysV));
            SystemCurrent.Data.Add(new Datum<double>(Sample, SysA));
            DrillCurrent.Data.Add(new Datum<double>(Sample, DrillA));
            RailCurrent.Data.Add(new Datum<double>(Sample, RailA));
        }

        public static void PacketTesting(Packet Packet)
        {
            if (Packet == null || Packet.Data == null || Packet.Data.Payload == null || Packet.Data.Payload.Length != 12)
            {
                Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "System sensor packet invalid. Discarding. Length: " + Packet?.Data?.Payload?.Length);
                return;
            }
            int Enc = UtilData.ToInt(UtilMain.SubArray(Packet.Data.Payload, 0, 4));
            DateTime Sample = new DateTime(UtilData.ToLong(UtilMain.SubArray(Packet.Data.Payload, 4, 8)));
        }

        public static void PacketRailStatus(Packet Packet)
        {
            if (Packet == null || Packet.Data == null || Packet.Data.Payload == null || Packet.Data.Payload.Length != 17)
            {
                Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "Rail status packet invalid. Discarding. Length: " + Packet?.Data?.Payload?.Length);
                return;
            }

            byte Status = Packet.Data.Payload[0];
            float RailSpeed = UtilData.ToFloat(UtilMain.SubArray(Packet.Data.Payload, 1, 4));
            float DepthTop = UtilData.ToFloat(UtilMain.SubArray(Packet.Data.Payload, 5, 4));
            float HeightGround = UtilData.ToFloat(UtilMain.SubArray(Packet.Data.Payload, 9, 4));
            float Target = UtilData.ToFloat(UtilMain.SubArray(Packet.Data.Payload, 13, 4));
            BaseMain.Window.UpdateRail(RailSpeed, DepthTop, HeightGround, Target, ((Status & 0b100) == 0b100), ((Status & 0b10) == 0b10), ((Status & 0b1) == 0b1));
        }

        private static void DoAdds()
        {
            Log.Output(Log.Severity.DEBUG, Log.Source.GUI, "Beginning random data addition.");
            for (int i = 0; i < 600; i++)
            {
                Thread.Sleep(150);
                RandomData.Data.Add(new Datum<int>(DateTime.Now, Random.Next(100)));
            }
            Log.Output(Log.Severity.DEBUG, Log.Source.GUI, "Random data addition finished.");
            
        }

    }
}
