using System;
using System.Timers;
using Scarlet.Components;
using Scarlet.Components.Motors;
using Scarlet.Components.Sensors;
using Scarlet.IO;
using Scarlet.IO.BeagleBone;
using Scarlet.Utilities;

namespace Science.Systems
{
    public class Rail : ISubsystem
    {
        private const float MOTOR_MAX_SPEED = 0.10F;
        private const int INIT_TIMEOUT = 5000;
        private const float ENCODER_MM_PER_TICK = 0.001F; // TODO: Placeholder value. Replace.

        private bool Initializing, InitDone;
        private int TopDepth; // The distance that the top of the rail is away from the very top position (limit switch), in mm.
        public bool TargetLocationRefIsTop = true; // The following terget distance is from the top of the rail (true) or the ground (false).
        public int TargetLocation; // Where the operator would like the rail to go.

        private TalonMC MotorCtrl;
        private LimitSwitch Limit;
        //private LS7366R Encoder;
        //private VL53L0X Ranger;

        /// <summary> Handles moving the linear rail up and down for the various experiments. </summary>
        public Rail(IPWMOutput MotorPWM, IDigitalIn LimitSw, ISPIBus EncoderSPI, IDigitalOut EncoderCS, II2CBus RangerBus)
        {
            this.MotorCtrl = new TalonMC(MotorPWM, MOTOR_MAX_SPEED);
            this.Limit = new LimitSwitch(LimitSw, false);
            //this.Encoder = new LS7366R(EncoderSPI, EncoderCS);
            //this.Ranger = new VL53L0X(RangerBus);
        }

        public void EventTriggered(object Sender, EventArgs Event)
        {
            if(Event is LimitSwitchToggle && this.Initializing) // We hit the end.
            {
                this.MotorCtrl.SetSpeed(0);
                this.TopDepth = 0;
                Log.Output(Log.Severity.DEBUG, Log.Source.MOTORS, "Rail motor finished initializing.");
                this.Initializing = false;
                this.InitDone = true;
            }
            if(Event is ElapsedEventArgs && this.Initializing) // We timed out trying to initialize.
            {
                this.MotorCtrl.SetSpeed(0);
                Log.Output(Log.Severity.ERROR, Log.Source.MOTORS, "Rail motor timed out while trying to initialize.");
                this.Initializing = false;
                this.InitDone = false;
            }
            if(Event is LimitSwitchToggle && !this.Initializing) // We hit the end during operation.
            {
                this.MotorCtrl.SetEnabled(false); // Immediately stop.
                this.TopDepth = 0;
                Log.Output(Log.Severity.WARNING, Log.Source.MOTORS, "Rail motor hit limit switch and was stopped for safety.");
            }
        }

        /// <summary> Prepares the rail for use by moving the motor all the way up to the top to find the zero position. </summary>
        public void Initialize()
        {
            this.Initializing = true;
            this.Limit.SwitchToggle += this.EventTriggered;

            Timer TimeoutTrigger = new Timer() { Interval = INIT_TIMEOUT, AutoReset = false };
            TimeoutTrigger.Elapsed += this.EventTriggered;
            TimeoutTrigger.Enabled = true;
            if (this.Limit.State) // We are already at the top, nothing needs to be done.
            {
                TimeoutTrigger.Enabled = false;
                this.InitDone = true;
                this.Initializing = false;
                this.TopDepth = 0;
                return;
            }
            this.MotorCtrl.SetSpeed(-0.05F);
            // Either the limit switch will be toggled, or the timeout event will happen after this.
        }

        /// <summary> Moves the rail to the highest position. </summary>
        public void GotoTop()
        {
            if(!this.InitDone) { Log.Output(Log.Severity.ERROR, Log.Source.MOTORS, "Tried to move rail to top without having done init."); return; }
        }

        /// <summary> Move the rail to where the drill is just above the ground. </summary>
        public void GotoDrillGround()
        {
            if (!this.InitDone) { Log.Output(Log.Severity.ERROR, Log.Source.MOTORS, "Tried to move rail to ground without having done init."); return; }
        }

        /// <summary> Move the rail to where the moisture sensor is in the soil. </summary>
        public void GotoMoistureInsert()
        {

        }

        /// <summary> Immediately stops the rail motor. </summary>
        public void EmergencyStop()
        {
            this.TargetLocation = this.TopDepth;
            this.MotorCtrl.SetEnabled(false);
            this.UpdateState();
            this.MotorCtrl.SetEnabled(false);
        }

        /// <summary>
        /// Updates the current position, target, intended motor speed. Communicates this to the Talon controller to drive the motor at the new desired rate.
        /// </summary>
        public void UpdateState()
        {
            this.Limit.UpdateState();
            //this.Encoder.UpdateState();
            //this.Ranger.UpdateState();
            // TODO: Send commands to Talon.
        }
    }
}
