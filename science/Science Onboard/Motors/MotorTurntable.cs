using System;
using System.Timers;
using RoboticsLibrary.Motors;
using RoboticsLibrary.Sensors;
using RoboticsLibrary.Utilities;

namespace Science.Motors
{
    class MotorTurntable : Motor
    {
        private readonly int Pin;
        private bool Initializing, InitDone;
        private int Speed, CurrentAngle;
        public int TargetAngle { get; set; }
        private readonly TalonMC MotorCtrl;
        private const int MAX_SPEED = 30;
        private const int INIT_TIMEOUT = 5000;

        public MotorTurntable(int Pin) : base(Pin)
        {
            this.Pin = Pin;
            this.MotorCtrl = new TalonMC(Pin);
        }

        public override void EventTriggered(object Sender, EventArgs Event)
        {
            if(Event is LimitSwitchToggle && this.Initializing) // We hit the end.
            {
                this.Stop();
                this.CurrentAngle = 0;
                Log.Output(Log.Severity.DEBUG, Log.Source.MOTORS, "Turntable motor finished initializing.");
                this.Initializing = false;
                this.InitDone = true;
            }
            if(Event is ElapsedEventArgs && this.Initializing) // We timed out trying to initialize.
            {
                this.Stop();
                Log.Output(Log.Severity.ERROR, Log.Source.MOTORS, "Turntable motor timed out while attempting to initialize.");
                this.Initializing = false;
            }
        }

        public override void Initialize()
        {
            this.Initializing = true;
            
            Timer TimeoutTrigger = new Timer() { Interval = INIT_TIMEOUT, AutoReset = false };
            TimeoutTrigger.Elapsed += this.EventTriggered;
            TimeoutTrigger.Enabled = true;
            // Do init stuff.
            this.TargetAngle = 360;
        }

        public override void Stop()
        {
            this.Speed = 0;
            this.TargetAngle = this.CurrentAngle;
            UpdateState();
        }

        public override void UpdateState()
        {
            if (this.Speed > MAX_SPEED) { this.Speed = MAX_SPEED; }
            if (!this.InitDone) { return; }
            // TODO: Come up with way to communicate speed and target to TalonMC.
        }
    }
}
