using HuskyRobotics.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HuskyRobotics.UI
{
    public class MockObservableMap : ObservableMap<float>
    {
        private IList<string> fields = new List<string> {
            "chasis/wheel1/steeringAngle",
            "chasis/wheel2/steeringAngle",
            "chasis/wheel3/steeringAngle",
            "chasis/wheel4/steeringAngle",
            "test",
        };
        private Timer updater;

        public MockObservableMap()
        {
            updateValues();
            updater = new Timer(updateValues, null, 400, 400);
        }
        
        private void updateValues()
        {
            updateValues(null);
        }

        private void updateValues(object state)
        {
            var rad = new Random();
            foreach (string field in fields) {
                var currentValue = ContainsKey(field) ? GetValue(field) : rad.NextDouble();
                var newValue = Math.Min(Math.Max(currentValue + (rad.NextDouble() - .5) * .05, 0), 1);
                PutValue(field, (float) newValue);
            }
        }


    }
}
