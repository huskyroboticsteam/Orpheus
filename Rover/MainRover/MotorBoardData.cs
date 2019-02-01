using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainRover
{
    class MotorBoardData
    {
        public int Model;
        public float P;
        public float I;
        public float D;
        public int TicksPerRev;

        public override string ToString()
        {
            return "Model:" + Model + " P:" + P + " I:" + I + " D:" + D + " TicksPerRev:" + TicksPerRev;
        }
    }
}
