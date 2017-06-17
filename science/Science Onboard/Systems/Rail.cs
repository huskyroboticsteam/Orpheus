using System;
using System.Timers;
using Scarlet.Components;
using Scarlet.Components.Motors;
using Scarlet.Components.Sensors;
using Scarlet.Utilities;

namespace Science.Systems
{
    public class Rail : ISubsystem
    {
        private const float MOTOR_MAX_SPEED = 0.10F;
        private const int INIT_TIMEOUT = 5000;

        private bool Initializing, InitDone;
        private int Height;
        public int TargetHeight { get; set; }

        private readonly TalonMC MotorCtrl;
        private readonly LimitSwitch Limit;
        private readonly Encoder Encoder;

        /// <summary>
        /// Handles moving the linear rail up and down for the various experiments.
        /// </summary>
        public Rail()
        {
            // TODO: Set these to actual pins.
            this.MotorCtrl = new TalonMC(0, MOTOR_MAX_SPEED);
            this.Limit = new LimitSwitch(1, false);
            this.Encoder = new Encoder(2, 3, 80);

            this.Limit.SwitchToggle += this.EventTriggered;
            this.Encoder.Turned += this.EventTriggered;
        }

        public void EventTriggered(object Sender, EventArgs Event)
        {
            if(Event is LimitSwitchToggle && this.Initializing) // We hit the end.
            {
                this.MotorCtrl.Stop();
                this.Height = 0;
                Log.Output(Log.Severity.DEBUG, Log.Source.MOTORS, "Rail motor finished initializing.");
                this.Initializing = false;
                this.InitDone = true;
            }
            if(Event is ElapsedEventArgs && this.Initializing) // We timed out trying to initialize.
            {
                this.MotorCtrl.Stop();
                Log.Output(Log.Severity.ERROR, Log.Source.MOTORS, "Rail motor timed out while trying to initialize.");
                this.Initializing = false;
            }
            if(Event is LimitSwitchToggle && !this.Initializing) // We hit the end during operation.
            {
                this.MotorCtrl.Stop();
                Log.Output(Log.Severity.WARNING, Log.Source.MOTORS, "Rail motor hit limit switch and was stopped for safety.");
            }
            if(Event is EncoderTurn)
            {
                // TODO: Track this.
            }
        }

        /// <summary>
        /// Prepares the rail for use by moving the motor all the way up to the top to find the zero position.
        /// </summary>
        public void Initialize()
        { // TODO: What happens when it is already at the top? This likely won't toggle the switch...
            this.MotorCtrl.Initialize();
            this.Limit.Initialize();
            this.Encoder.Initialize();

            this.Initializing = true;

            Timer TimeoutTrigger = new Timer() { Interval = INIT_TIMEOUT, AutoReset = false };
            TimeoutTrigger.Elapsed += this.EventTriggered;
            TimeoutTrigger.Enabled = true;
            this.GotoTop();
        }

        /// <summary>
        /// Moves the rail to the highest position.
        /// </summary>
        public void GotoTop()
        {
            this.MotorCtrl.Speed = MOTOR_MAX_SPEED;
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
        public void EmergencyStop()
        {
            this.TargetHeight = this.Height;
            this.MotorCtrl.Stop();
            this.UpdateState();
        }

        /// <summary>
        /// Updates the current position, target, intended motor speed. Communicates this to the Talon controller to drive the motor at the new desired rate.
        /// </summary>
        public void UpdateState()
        {
            this.Limit.UpdateState();
            this.Encoder.UpdateState();
            this.MotorCtrl.UpdateState();
            if (!this.InitDone)
            {
                Log.Output(Log.Severity.WARNING, Log.Source.SUBSYSTEM, "Rail has not been initialized yet.");
                return;
            }
            // TODO: Send commands to Talon.
        }
    }
}
