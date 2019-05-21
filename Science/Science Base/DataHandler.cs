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
        // Aux sensor packets
        public static DataSeries<int> UV = new DataSeries<int>("UV Light", "UV Light (µW/cm²)"); // TODO: Update for VEML6075
        public static DataSeries<ushort> AirCO2 = new DataSeries<ushort>("Air CO2", "CO2 Concentration (PPM)");
        public static DataSeries<float> AirTVOC = new DataSeries<float>("Air TVOC", "TVOC Concentration (PPB)");
        public static DataSeries<float> AirTemp = new DataSeries<float>("Air Temperature", "Temperature (°C)");
        public static DataSeries<float> AirHumidity = new DataSeries<float>("Air Humidity", "Relative Humidity (%)");
        public static DataSeries<float> AirPressure = new DataSeries<float>("Air Pressure", "Pressure (Pa)");
        public static DataSeries<float> SoilMoisture = new DataSeries<float>("Soil Moisture", "Volumetric Water Content (%)");

        // Rail status packets
        public static DataSeries<float> RailGroundDistance = new DataSeries<float>("GND Distance", "Drill Ground Distance (mm)");
        public static DataSeries<float> RailTopDistance = new DataSeries<float>("Rail Depth", "Rail Depth (mm)");
        public static DataSeries<float> RailVelocity = new DataSeries<float>("Rail Velocity", "Rail Velocity (mm/s)");

        // Turntable status packets
        public static DataSeries<float> TTBPosition = new DataSeries<float>("Turntable Position", "Turntable Position (°)");

        // Drill status packets
        public static DataSeries<byte> DrillSpeed = new DataSeries<byte>("Drill Speed", "Drill Speed (%)");
        public static DataSeries<bool> SampleDoorState = new DataSeries<bool>("Sample Door", "Sample Door State");

        // System sensor packets
        public static DataSeries<float> SupplyVoltage = new DataSeries<float>("Supply Voltage", "Supply Voltage (V)");
        public static DataSeries<float> SystemCurrent = new DataSeries<float>("System Current", "System Current (A)");
        public static DataSeries<float> DrillCurrent = new DataSeries<float>("Drill Current", "Drill Current (A)");
        public static DataSeries<float> RailCurrent = new DataSeries<float>("Rail Current", "Rail Current (A)");
        public static DataSeries<float> TTBCurrent = new DataSeries<float>("Turntable Current", "Turntable Current (A)");
        public static DataSeries<float> SpareCurrent = new DataSeries<float>("Spare Current", "Spare Motor Current (A)");

        // Garbage for testing
        public static DataSeries<int> RandomData = new DataSeries<int>("Random", "Rubbish");
        private static Random Random;

        public static TimeSpan ClientOffset = new TimeSpan(0);

        public static void Start()
        {
            Random = new Random(); // TODO: Re-add packet parsing after figuring out new IDs.
            //Parse.SetParseHandler(ScienceConstants.Packets.GND_SENSOR, PacketGroundSensor);
            //Parse.SetParseHandler(ScienceConstants.Packets.SYS_SENSOR, PacketSysSensor);
            //Parse.SetParseHandler(ScienceConstants.Packets.RAIL_STATUS, PacketRailStatus);
            new Thread(new ThreadStart(DoAdds)).Start();
        }

        public static object[] GetSeries()
        {
            return new object[]
            {
                SupplyVoltage, SystemCurrent, DrillCurrent, RailCurrent, TTBCurrent, SpareCurrent,
                UV, AirCO2, AirTVOC, AirTemp, AirHumidity, AirPressure, SoilMoisture,
                RailGroundDistance, RailTopDistance, RailVelocity,
                TTBPosition,
                DrillSpeed, SampleDoorState,
                RandomData
            };
        }

        public static void PacketAuxSensors(Packet Packet)
        {
            if (Packet == null || Packet.Data == null || Packet.Data.Payload == null || Packet.Data.Payload.Length != 26)
            {
                Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "Aux sensor packet invalid. Discarding. Length: " + Packet?.Data?.Payload?.Length);
                return;
            }
            DateTime Time = ExtractTime(Packet);
            int UVLight = UtilData.ToInt(UtilMain.SubArray(Packet.Data.Payload, 0, 4));
            ushort CO2Reading = UtilData.ToUShort(UtilMain.SubArray(Packet.Data.Payload, 4, 2));
            int TVOCReading = UtilData.ToInt(UtilMain.SubArray(Packet.Data.Payload, 6, 4));
            float SoilMoist = UtilData.ToFloat(UtilMain.SubArray(Packet.Data.Payload, 10, 4));
            float AtmoTemp = UtilData.ToFloat(UtilMain.SubArray(Packet.Data.Payload, 14, 4));
            float AtmoPres = UtilData.ToFloat(UtilMain.SubArray(Packet.Data.Payload, 18, 4));
            float AtmoHumid = UtilData.ToFloat(UtilMain.SubArray(Packet.Data.Payload, 22, 4));

            UV.Data.Add(new Datum<int>(Time, UVLight));
            AirCO2.Data.Add(new Datum<ushort>(Time, CO2Reading));
            AirTVOC.Data.Add(new Datum<float>(Time, TVOCReading));
            SoilMoisture.Data.Add(new Datum<float>(Time, SoilMoist));
            AirTemp.Data.Add(new Datum<float>(Time, AtmoTemp));
            AirPressure.Data.Add(new Datum<float>(Time, AtmoPres));
            AirHumidity.Data.Add(new Datum<float>(Time, AtmoHumid));
        }

        public static void PacketRailStatus(Packet Packet)
        {
            if (Packet == null || Packet.Data == null || Packet.Data.Payload == null || Packet.Data.Payload.Length != 17)
            {
                Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "Rail status packet invalid. Discarding. Length: " + Packet?.Data?.Payload?.Length);
                return;
            }
            DateTime Time = ExtractTime(Packet);
            byte Status = Packet.Data.Payload[0];
            float Velocity = UtilData.ToFloat(UtilMain.SubArray(Packet.Data.Payload, 1, 4));
            float Depth = UtilData.ToFloat(UtilMain.SubArray(Packet.Data.Payload, 5, 4));
            float Height = UtilData.ToFloat(UtilMain.SubArray(Packet.Data.Payload, 9, 4));
            float Target = UtilData.ToFloat(UtilMain.SubArray(Packet.Data.Payload, 13, 4));

            RailGroundDistance.Data.Add(new Datum<float>(Time, Height));
            RailTopDistance.Data.Add(new Datum<float>(Time, Depth));
            RailVelocity.Data.Add(new Datum<float>(Time, Velocity));

            BaseMain.Window.UpdateRail(Velocity, Depth, Height, Target, ((Status & 0b100) == 0b100), ((Status & 0b10) == 0b10), ((Status & 0b1) == 0b1));
        }

        public static void PacketTurntableStatus(Packet Packet)
        {
            if (Packet == null || Packet.Data == null || Packet.Data.Payload == null || Packet.Data.Payload.Length != 9)
            {
                Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "Turntable status packet invalid. Discarding. Length: " + Packet?.Data?.Payload?.Length);
                return;
            }
            DateTime Time = ExtractTime(Packet);
            byte Status = Packet.Data.Payload[0];
            float Position = UtilData.ToFloat(UtilMain.SubArray(Packet.Data.Payload, 1, 4));
            float Target = UtilData.ToFloat(UtilMain.SubArray(Packet.Data.Payload, 5, 4));

            TTBPosition.Data.Add(new Datum<float>(Time, Position));

            BaseMain.Window.UpdateTurntable(Position, ((Status & 0b10) == 0b10), ((Status & 0b1) == 0b1));
        }

        public static void PacketDrillStatus(Packet Packet)
        {
            if (Packet == null || Packet.Data == null || Packet.Data.Payload == null || Packet.Data.Payload.Length != 2)
            {
                Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "Drill status packet invalid. Discarding. Length: " + Packet?.Data?.Payload?.Length);
                return;
            }
            DateTime Time = ExtractTime(Packet);
            byte Status = Packet.Data.Payload[0];
            byte Speed = Packet.Data.Payload[1];

            bool SampleDoor = (Status & 0b1) == 0b1;

            DrillSpeed.Data.Add(new Datum<byte>(Time, Speed));
            SampleDoorState.Data.Add(new Datum<bool>(Time, SampleDoor));

            BaseMain.Window.UpdateDrill(Speed, SampleDoor);
        }

        public static void PacketSystemSensors(Packet Packet)
        {
            if (Packet == null || Packet.Data == null || Packet.Data.Payload == null || Packet.Data.Payload.Length != 24)
            {
                Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "System sensor packet invalid. Discarding. Length: " + Packet?.Data?.Payload?.Length);
                return;
            }
            DateTime Time = ExtractTime(Packet);
            float SupplyV = UtilData.ToFloat(UtilMain.SubArray(Packet.Data.Payload, 0, 4));
            float SysA = UtilData.ToFloat(UtilMain.SubArray(Packet.Data.Payload, 4, 4));
            float DrillA = UtilData.ToFloat(UtilMain.SubArray(Packet.Data.Payload, 8, 4));
            float RailA = UtilData.ToFloat(UtilMain.SubArray(Packet.Data.Payload, 12, 4));
            float TTBA = UtilData.ToFloat(UtilMain.SubArray(Packet.Data.Payload, 16, 4));
            float SpareA = UtilData.ToFloat(UtilMain.SubArray(Packet.Data.Payload, 20, 4));

            SupplyVoltage.Data.Add(new Datum<float>(Time, SupplyV));
            SystemCurrent.Data.Add(new Datum<float>(Time, SysA));
            DrillCurrent.Data.Add(new Datum<float>(Time, DrillA));
            RailCurrent.Data.Add(new Datum<float>(Time, RailA));
            TTBCurrent.Data.Add(new Datum<float>(Time, TTBA));
            SpareCurrent.Data.Add(new Datum<float>(Time, SpareA));
        }

        // Old
        /*
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

            //UV.Data.Add(new Datum<int>(Timestamp, UVLight));
            //AirPollution.Data.Add(new Datum<float>(Timestamp, AirQuality));
            //SoilMoisture.Data.Add(new Datum<float>(Timestamp, SoilMoist));
            //ThermoExt.Data.Add(new Datum<float>(Timestamp, MAX31855.ConvertExternalFromRaw(ThermocoupleData)));
            //ThermoInt.Data.Add(new Datum<float>(Timestamp, MAX31855.ConvertInternalFromRaw(ThermocoupleData)));
            //AirTemp.Data.Add(new Datum<double>(Timestamp, AtmoTemp));
            //AirPressure.Data.Add(new Datum<double>(Timestamp, AtmoPres));
            //AirHumidity.Data.Add(new Datum<double>(Timestamp, AtmoHumid));
        }

        // Old
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

        // Old
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

        // Old
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
        }*/

        private static DateTime ExtractTime(Packet Packet) => new DateTime(UtilData.ToLong(Packet.Data.Timestamp)).Add(ClientOffset);

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
