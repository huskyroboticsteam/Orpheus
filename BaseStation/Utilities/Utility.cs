using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuskyRobotics.Utilities
{
    public class Utility
    {
        public static short PreventOverflow(short shortVal)
        {
            if (shortVal == -32768)
            {
                shortVal++;
            }
            return shortVal;
        }
    }
}
