using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerRewrite
{
    class StreamParameters
    {
        public int Width;
        public int Height;
        public int Port;
        public float Scale;

        public StreamParameters(int Width, int Height, float Scale, int Port)
        {
            this.Width = Width;
            this.Height = Height;
            this.Port = Port;
            this.Scale = Scale;
        }
    }
}
