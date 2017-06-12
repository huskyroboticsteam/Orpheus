using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboticsLibrary.Sensors
{
    public interface ISensor
    {
        // Get a new reading and call the relevant event handlers.
        void UpdateState();

        // Test to make sure that the sensor is functioning as expected, without updating state.
        bool Test();
    }
}
