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
        public UInt16 P;
        public UInt16 I;
        public UInt16 D;
        public UInt16 TicksPerRev;

        public override string ToString()
        {
            return "Model:" + Model + " P:" + P + " I:" + I + " D:" + D + " TicksPerRev:" + TicksPerRev;
        }
    }
}
