using System;
using Scarlet.Utilities;
using Scarlet.IO;

namespace Scarlet.Components.Sensors
{
    /// <summary>
    /// Quickly releases incoming interrupt events to prevent I/O system from getting bogged down, then will pass them on when UpdateState is called next.
    /// </summary>
    public class LimitSwitch : ISensor
    {
        private IDigitalIn Input;
        private readonly bool Invert;
        private volatile bool HadEvent, NewState;
        public event EventHandler<LimitSwitchToggle> SwitchToggle;

        public LimitSwitch(IDigitalIn Input, bool Invert = false)
        {
            this.Input = Input;
            this.Invert = Invert;
            Input.RegisterInterruptHandler(EventTriggered, InterruptType.ANY_EDGE);
        }
        
        public bool Test()
        {
            return true; // TODO: Determine method for, and implement, limit switch testing.
        }

        public void UpdateState()
        {
            if(this.HadEvent)
            {
                LimitSwitchToggle Event = new LimitSwitchToggle() { CurrentState = this.Invert ? !this.NewState : this.NewState };
                OnSwitchToggle(Event);
                this.HadEvent = false;
            }
        }

        protected virtual void OnSwitchToggle(LimitSwitchToggle Event)
        {
            SwitchToggle?.Invoke(this, Event);
        }

        public void EventTriggered(object Sender, EventArgs Event)
        {
            if(Event is InputInterrupt)
            {
                this.HadEvent = true;
                this.NewState = ((InputInterrupt)Event).NewState;
            }
        }
    }

    public class LimitSwitchToggle : EventArgs
    {
        public bool CurrentState { get; set; }
    }
}
