using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.NetworkInformation;
using System.Net;

namespace HuskyRobotics.UI
{
    public class PingResult : INotifyPropertyChanged
    {
        private String _address;
        private int _responseTimeMs = -1;

        private Timer pinger;

        public PingResult(String address)
        {
            Address = address;

            // Put this on another thread so it doesn't block the UI
            pinger = new Timer(updatePing, null, 0, 1000);
        }

        private void updatePing(object state)
        {
            Ping sender = new Ping();
            try
            {
                PingReply reply = sender.Send(_address, 500); // 500 ms timeout

                if (reply.Status == IPStatus.Success)
                {
                    ResponseTimeMs = (int)reply.RoundtripTime;
                }
                else
                {
                    ResponseTimeMs = -1; // I don't like using -1 to indicate a failure, but it works. Eh
                }
            }
            catch (PingException e)
            {
                ResponseTimeMs = -1;
            }
        }

        public override string ToString()
        {
            if (ResponseTimeMs >= 0)
            {
                return Address + " (" + ResponseTimeMs + "ms)";
            }
            else
            {
                return Address + " (Timeout)";
            }
        }

        // Boilerplate
        public event PropertyChangedEventHandler PropertyChanged;

        public String Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Address"));
            }
        }

        public int ResponseTimeMs
        {
            get
            {
                return _responseTimeMs;
            }
            private set
            { // Not externally settable
                _responseTimeMs = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ResponseTimeMs"));
            }
        }
    }
}