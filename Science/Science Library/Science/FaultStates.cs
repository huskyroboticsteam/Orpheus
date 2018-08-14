using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Science.Library
{
    public static class FaultStates
    {        
        public struct Fault
        {
            public string Description;
            public byte Severity; // 0: Good, 1: Warning, 2: Error

            public Fault(string Description, byte Severity)
            {
                this.Description = Description;
                this.Severity = Severity;
            }
        }

        public static class Network
        {
            public static readonly Fault NOT_CONNECTED = new Fault("Client not connected", 2);
            public static readonly Fault LATENCY_HIGH = new Fault("High connection latency", 1);
        }

        public static class Power
        {
            public static readonly Fault SYS_VOLTAGE_LOW = new Fault("System voltage (battery) low", 1);
            public static readonly Fault SYS_VOLTAGE_CRITICAL = new Fault("System voltage (battery) critically low", 2);
            public static readonly Fault SYS_CURRENT_HIGH = new Fault("System current high", 1);
            public static readonly Fault DRILL_CURRENT_HIGH = new Fault("Drill in reduced power mode due to high load", 1);
            public static readonly Fault DRILL_OVERLOAD = new Fault("Drill shut down due to excessive load", 2);
            public static readonly Fault RAIL_CURRENT_HIGH = new Fault("Rail in reduced power mode due to high load", 1);
            public static readonly Fault RAIL_OVERLOAD = new Fault("Rail shut down due to excessive load", 2);
        }

        public static class System
        {
            public static readonly Fault CPU_HIGH = new Fault("CPU usage high", 1);
            public static readonly Fault RAM_HIGH = new Fault("RAM usage high", 1);
            public static readonly Fault DISK_SPACE_LOW = new Fault("Storage space low", 1);
        };
    }
    
}
