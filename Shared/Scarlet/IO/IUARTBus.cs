using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scarlet.IO
{
    public interface IUARTBus
    {
        void Initialize();

        void Write(byte[] Data);

        byte[] Read(int Length);

        void Dispose();
    }
}
