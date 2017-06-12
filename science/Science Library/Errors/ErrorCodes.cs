using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboticsLibrary.Errors
{
    public static class ErrorCodes
    {
        public static readonly string[][] List = new string[][] { new string[]
        { // Main System
            "Everything is OK",
            "Unknown error",
            "System Errors Found",
            "Could not setup Digital IO Pin",
            "Could not read Digital IO Pin"
        }, new string[]
        { // Thermocouple
            "Could not get reading",
            "Could not get internal reading",
            "Invalid reading",
            "Open Circuit",
            "GND Short",
            "VCC Short",
            "General Failure",
            "Commnication Failure"
        }, new string[]
        { // UV Sensor
            "Could not get reading",
            "Invalid reading",
            "Communication Failure"
        }, new string[]
        { // Distance Sensor
            "Communication Failure",
            "Failed to begin ranging",
            "Failed to stop ranging"
        }, new string[]
        { // Humidity Sensor
            "Could not get reading",
            "Invalid reading",
            "Overzealous insertion detected",
            "ADC failed to initialize"
        }, new string[]
        { // Communications (NOT YET IMPLEMENTED)
            "",
            ""
        }, new string[]
        { // Motors (NOT YET IMPLEMENTED)
            "",
            ""
        }};
    }
}
