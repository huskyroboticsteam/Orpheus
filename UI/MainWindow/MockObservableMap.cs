using HuskyRobotics.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;

namespace HuskyRobotics.UI
{
    public class MockObservableMap : ObservableMap<float>
    {
        private IList<string> fields = new List<string> {
            "chasis/steeringAngle",
            "chasis/wheel1/steeringAngle",
            "chasis/wheel2/steeringAngle",
            "chasis/wheel3/steeringAngle",
            "chasis/wheel4/steeringAngle",
            "test",
        };
        private Timer updater;

        public MockObservableMap()
        {
            UpdateValues();
            updater = new Timer(UpdateValues, null, 400, 400);
        }
        
        private void UpdateValues()
        {
            UpdateValues(null);
        }

        private void UpdateValues(object state) 
        {
            Random rad = new Random();
            foreach (string field in fields) {
                var currentValue = ContainsKey(field) ? GetValue(field) : rad.NextDouble();
                var newValue = Math.Min(Math.Max(currentValue + (rad.NextDouble() - .5) * .1, 0), 1);
                PutValue(field, (float)newValue);
            }
        }
    }
}
