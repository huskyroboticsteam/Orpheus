using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scarlet.Components
{
    public interface IServo
    {
        /// <summary>Updates the position of the servo</summary>
        void SetPosition(int NewPosition);

        /// <summary>Stops the servo immediately.</summary>
        void Stop();

        /// <summary>Used to send events to Motors.</summary>
        void EventTriggered(object Sender, EventArgs Event);
    }
}
