using System;
using Scarlet.Utilities;

namespace Scarlet.Components.Sensors
{
    [Obsolete] // Marked as such to warn users, as this class is not ready for use yet.
    public class Encoder : ISensor
    {
        private int PinA, PinB;
        private int PulsesPerTurn;
        public int Angle { get; private set; }
        public event EventHandler<EncoderTurn> Turned;

        // TODO: Can we implement this (as a separate Component) using BB CPU Encoder counter?
        public Encoder(int PinA, int PinB, int PulsesPerTurn)
        {
            this.PinA = PinA;
            this.PinB = PinB;
            this.PulsesPerTurn = PulsesPerTurn;
        }

        public bool Test()
        {
            // TODO: Call a GPIO library to check functionality.
            Log.Output(Log.Severity.WARNING, Log.Source.SENSORS, "Encoder testing not implemented properly.");
            return true;
        }

        public void UpdateState()
        {
            // TODO: Call a GPIO library to update the state.
            Log.Output(Log.Severity.WARNING, Log.Source.SENSORS, "Encoder updating not implemented properly.");
            int AngleChange = 0;

            Random RandomGen = new Random();
            AngleChange = RandomGen.Next(20) - 10;
            this.Angle += AngleChange;
            if (AngleChange != 0)
            {
                EncoderTurn Event = new EncoderTurn() { TurnAmount = AngleChange, Angle = this.Angle };
                OnTurn(Event);
            }
        }

        protected virtual void OnTurn(EncoderTurn Event)
        {
            Turned?.Invoke(this, Event);
        }

        public void EventTriggered(object Sender, EventArgs Event)
        {
            
        }
    }

    public class EncoderTurn : EventArgs
    {
        public int TurnAmount { get; set; }
        public int Angle { get; set; }
    }
}
