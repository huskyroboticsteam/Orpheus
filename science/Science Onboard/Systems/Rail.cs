using System;
using System.Timers;
using RoboticsLibrary.Motors;
using RoboticsLibrary.Sensors;
using RoboticsLibrary.Utilities;

namespace Science.Systems
{
    public class Rail : Motor
    {
        private readonly int Pin;
        private bool Initializing, InitDone;
        private int Speed, Height;
        public int TargetHeight { get; set; }
        private readonly TalonMC MotorCtrl;
        private const int MAX_SPEED = 10;
        private const int INIT_TIMEOUT = 5000;

        /// <summary>
        /// Handles moving the linear rail up and down for the various experiments.
        /// </summary>
        /// <param name="Pin">The ouptut pin to use for the PWM motor output</param>
        public Rail(int Pin) : base(Pin)
        {
            this.Pin = Pin;
            this.MotorCtrl = new TalonMC(Pin);
        }

        public override void EventTriggered(object Sender, EventArgs Event)
        {
            if(Event is LimitSwitchToggle && this.Initializing) // We hit the end.
            {
                this.Stop();
                this.Height = 0;
                Log.Output(Log.Severity.DEBUG, Log.Source.MOTORS, "Rail motor finished initializing.");
                this.Initializing = false;
                this.InitDone = true;
            }
            if(Event is ElapsedEventArgs && this.Initializing) // We timed out trying to initialize.
            {
                this.Stop();
                Log.Output(Log.Severity.ERROR, Log.Source.MOTORS, "Rail motor timed out while trying to initialize.");
                this.Initializing = false;
            }
            if(Event is LimitSwitchToggle && !this.Initializing) // We hit the end during operation.
            {
                this.Stop();
                Log.Output(Log.Severity.WARNING, Log.Source.MOTORS, "Rail motor hit limit switch and was stopped for safety.");
            }
        }

        /// <summary>
        /// Prepares the rail for use by moving the motor all the way up to the top to find the zero position.
        /// </summary>
        public override void Initialize()
        { // TODO: What happens when it is already at the top? This likely won't toggle the switch...
            this.Initializing = true;

            Timer TimeoutTrigger = new Timer() { Interval = INIT_TIMEOUT, AutoReset = false };
            TimeoutTrigger.Elapsed += this.EventTriggered;
            TimeoutTrigger.Enabled = true;
            // Do init stuff.
            this.TargetHeight = 0;
        }

        /// <summary>
        /// Moves the rail to the highest position.
        /// </summary>
        public void GotoTop()
        {

        }

        /// <summary>
        /// Move the rail to where the drill is just above the ground.
        /// </summary>
        public void GotoDrillGround()
        {

        }

        /// <summary>
        /// Move the rail to where the moisture sensor is in the soil.
        /// </summary>
        public void GotoMoistureInsert()
        {

        }

        /// <summary>
        /// Move the rail to where the thermocouple is in the soil.
        /// </summary>
        public void GotoThermoInsert()
        {

        }

        /// <summary>
        /// Immediately stops the rail motor.
        /// </summary>
        public override void Stop()
        {
            this.Speed = 0;
            this.TargetHeight = this.Height;
            UpdateState();
        }

        /// <summary>
        /// Updates the current position, target, intended motor speed. Communicates this to the Talon controller to drive the motor at the new desired rate.
        /// </summary>
        public override void UpdateState()
        {
            if (this.Speed > MAX_SPEED) { this.Speed = MAX_SPEED; }
            if (!this.InitDone) { return; }
            // TODO: Send commands to Talon.
        }
    }
}
