using System;
using System.Text;
using System.Threading;
using Scarlet.IO;

namespace Scarlet.Components.Sensors
{
    public class MTK3339 : ISensor
    {
        private const string UPDATE_200_MSEC = "$PMTK220,200*2C\r\n";
        private const string MEAS_200_MSEC = "$PMTK300,200,0,0,0,0*2F\r\n";
        private const string GPRMC_GPGGA = "$PMTK314,0,1,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0*28\r\n";
        private const string QUERY_GPGSA = "$PSRF103,02,01,00,01*27\r\n";

        public float Latitude { get; private set; }
        public float Longitude { get; private set; }

        private IUARTBus UART;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Scarlet.Components.Sensors.MTK3339"/> class. 
        /// Sets it to send GPRMC and GPGGA every 200 milliseconds. 
        /// </summary>
        /// <param name="UART"> The UART bus to read from and write to. </param>
        public MTK3339(IUARTBus UART)
        {
            this.UART = UART;
            WriteString(GPRMC_GPGGA);
            Thread.Sleep(1);
            WriteString(MEAS_200_MSEC);
            Thread.Sleep(1);
            WriteString(UPDATE_200_MSEC);
            Thread.Sleep(1);
            if (UART.BytesAvailable() > 0) { UART.Read(UART.BytesAvailable(), new byte[UART.BytesAvailable()]); }
        }

        /// <summary> Checks whether this GPS has a fix. </summary>        
        /// <returns> Returns true if the GPS has a fix and false otherwise. </returns>
        public bool Test() => HasFix();

        public void EventTriggered(object sender, EventArgs e) => throw new NotImplementedException("MTK3339 doesn't have events");

        /// <summary> Gets new readings from GPS. </summary>
        public void UpdateState() => GetCoords();

        /// <summary> Queries the GPS to see if it has a fix. </summary>
        /// <returns> <c>true</c>, if fix was hased, <c>false</c> otherwise. </returns>
        public bool HasFix()
        {
            WriteString(QUERY_GPGSA);
            string[] Result = Read();
            if (Result.Length < 3) { return false; }
            return Result[2] != "01";
        }

        /// <summary> Reads an NMEA sentence and splits it. </summary>
        /// <returns> Returns the NMEA sentence split by comma. </returns>
        private string[] Read()
        {
            string GpsResult = "";
            byte[] PrevChar = new byte[1];
            while (PrevChar[0] != '\n')
            {
                if (UART.BytesAvailable() < 1)
                {
                    Thread.Sleep(Utilities.Constants.DEFAULT_MIN_THREAD_SLEEP);
                    continue;
                }
                UART.Read(1, PrevChar);
                GpsResult += Encoding.ASCII.GetString(PrevChar); 
            }
            return GpsResult.Split(',');
        }

        /// <summary> Gets the GPS coordinates of this GPS. </summary>
        /// <returns> Returns a tuple with the GPS coordinates, with Latitude first and Longitude second. </returns>
        public Tuple<float, float> GetCoords()
        {
            string[] Info = Read();
            if (Info[0] == "$GPGGA")
            {
                Latitude = RawToDeg(Info[2]);
                string LatDir = Info[3];
                Longitude = RawToDeg(Info[4]);
                string LngDir = Info[5];
                if (LatDir == "S")
                    Latitude = -Latitude;
                if (LngDir == "W")
                    Longitude = -Longitude;
            }
            return new Tuple<float, float>(Latitude, Longitude);
        }

        /// <summary> Converts a string number to a degree value. </summary>
        /// <returns> The degree value. </returns>
        /// <param name="Val"> The string value. </param>
        private float RawToDeg(string Val)
        {
            if (Val.Length < 2)
                return 0.0f;
            string[] GPSplit = Val.Split('.');
            float Deg = float.Parse(GPSplit[0].Substring(0, GPSplit[0].Length - 2));
            float Min = float.Parse(GPSplit[0].Substring(GPSplit[0].Length - 2) + '.' + GPSplit[1]);
            return Deg + Min / 60.0f;
        }

        /// <summary> Writes a string to the UART as a string of bytes. </summary>
        /// <param name="s"> The string to write. </param>
        private void WriteString(string s) => UART.Write(Encoding.ASCII.GetBytes(s));
    }
}