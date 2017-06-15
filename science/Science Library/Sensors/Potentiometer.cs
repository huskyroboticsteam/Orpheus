using System;
using RoboticsLibrary.Sensors;
using RoboticsLibrary.Utilities;

namespace Science_Library.Sensors
{
    public class Potentiometer : Sensor
    {
        private int Pin;
        public int Angle { get; private set; }
        public event EventHandler<PotentiometerTurn> Turned;

        public Potentiometer(int Pin)
        {
            this.Pin = Pin;
        }

        public override bool Test()
        {
            // TODO: Replace this with an actual check.
            Log.Output(Log.Severity.WARNING, Log.Source.SENSORS, "Potentiometer testing not implemented properly.");
            return true;
        }

        public override bool GetsRegUpdates() { return true; }

        public override void UpdateState()
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
    }

    public class PotentiometerTurn : EventArgs
    {
        public int TurnAmount { get; set; }
        public int Angle { get; set; }
    }
}
