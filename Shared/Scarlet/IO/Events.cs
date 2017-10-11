using System;

namespace Scarlet.IO
{
    public class InputInterrupt : EventArgs
    {
        public bool NewState;
        public InputInterrupt(bool State) { this.NewState = State; }
    }
}
