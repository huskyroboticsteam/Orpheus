using System;
using Scarlet.Utilities;

namespace Scarlet.Components.Sensors
{
    public class Potentiometer : ISensor
    {
        private int Pin;
        public int Angle { get; private set; }
        public event EventHandler<PotentiometerTurn> Turned;

        public Potentiometer(int Pin)
        {
            this.Pin = Pin;
        }

        public bool Test()
        {
            // TODO: Replace this with an actual check.
            Log.Output(Log.Severity.WARNING, Log.Source.SENSORS, "Potentiometer testing not implemented properly.");
            return true;
        }

        public void UpdateState()
        {
            // TODO: Actually check state.
            Log.Output(Log.Severity.WARNING, Log.Source.SENSORS, "Potentiometer updating not implemented properly.");
            int AngleChange = 0;

            this.Angle += AngleChange;
            if(AngleChange != 0)
            {
                PotentiometerTurn Event = new PotentiometerTurn() { TurnAmount = AngleChange, Angle = this.Angle };
                OnTurn(Event);
            }
        }

        protected virtual void OnTurn(PotentiometerTurn Event)
        {
            Turned?.Invoke(this, Event);
        }

        public void Initialize()
        {
            // TODO: Set up GPIO pins.
        }

        public void EventTriggered(object Sender, EventArgs Event)
        {
            
        }
    }

    public class PotentiometerTurn : EventArgs
    {
        public int TurnAmount { get; set; }
        public int Angle { get; set; }
    }
}
