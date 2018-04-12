using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuskyRobotics.UI
{
    public partial class ArmTopView : ArmView
    {
        protected override (float, float) ProjectCanvas((float, float, float) input)
        {
            return (input.Item1, input.Item3);
        }

        protected override (float, float, float) UnprojectCanvas((float, float) input, (float, float, float) setpoint)
        {
            return (input.Item1, setpoint.Item2, input.Item2);
        }
    }
}
