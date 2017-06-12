using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoboticsLibrary.Utilities;

namespace RoboticsLibrary.Sensors
{
    public class Encoder : Sensor
    {
        private int PinA, PinB;
        private int PulsesPerTurn;
        public int Angle { get; private set; }

        public Encoder(int PinA, int PinB, int PulsesPerTurn)
        {
            this.PinA = PinA;
            this.PinB = PinB;
            this.PulsesPerTurn = PulsesPerTurn;
        }

        public event EventHandler<EncoderTurn> Turned;

        public override bool Test()
        {
            // TODO: Call a GPIO library to check functionality.
            Log.Output(Log.Severity.WARNING, Log.Source.SENSORS, "Encoder testing not implemented properly.");
            return true;
        }

        public override void UpdateState()
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
    }

    public class EncoderTurn : EventArgs
    {
        public int TurnAmount { get; set; }
        public int Angle { get; set; }
    }
}
