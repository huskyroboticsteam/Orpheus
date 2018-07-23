using System;
using System.Linq;
using System.Threading;
using System.Timers;
using Scarlet.Communications;
using Scarlet.Components;
using Scarlet.Components.Motors;
using Scarlet.Components.Outputs;
using Scarlet.Components.Sensors;
using Scarlet.IO;
using Scarlet.IO.BeagleBone;
using Scarlet.Utilities;
using Science.Library;

namespace Science.Systems
{
    public class Rail : ISubsystem
    {
        public bool TraceLogging { get; set; }

        private const float MOTOR_MAX_SPEED = 0.5F;
        private const int INIT_TIMEOUT = 5000;
        private const float ENCODER_MM_PER_TICK = 0.9F; // TODO: Placeholder value. Replace.

        private bool P_Initializing = false;
        private bool Initializing
        {
            get => this.P_Initializing;
            set
            {
                this.P_Initializing = value;
                this.LED?.InitStateChange(value);
            }
        }
        private bool InitDone = false;
        private double TopDepth; // The distance that the top of the rail is away from the very top position (limit switch), in mm.
        public bool TargetLocationRefIsTop = true; // The following terget distance is from the top of the rail (true) or the ground (false).
        public double TargetLocation; // Where the operator would like the rail to go.
        private int LastEncoderCount;
        public float RailSpeed = 0.3F;

        private readonly TalonMC MotorCtrl;
        private readonly LimitSwitch Limit;
        private readonly LS7366R Encoder;
        private readonly VL53L0X_MVP Ranger;

        private readonly LEDController LED;

        /// <summary> Handles moving the linear rail up and down for the various experiments. </summary>
        public Rail(IPWMOutput MotorPWM, IDigitalIn LimitSw, ISPIBus EncoderSPI, IDigitalOut EncoderCS, II2CBus RangerBus, RGBLED LED)
        {
            this.MotorCtrl = new TalonMC(MotorPWM, MOTOR_MAX_SPEED);
            this.Limit = new LimitSwitch(LimitSw, false);
            this.Encoder = new LS7366R(EncoderSPI, EncoderCS); // Note: Count goes down when the rail moves down.
            LS7366R.Configuration Config = LS7366R.DefaultConfig;
            Config.QuadMode = LS7366R.QuadMode.X4_QUAD;
            this.Encoder.Configure(Config);
            this.Ranger = new VL53L0X_MVP(RangerBus);
            this.LED = new LEDController(LED);
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
            else if(Event is ElapsedEventArgs && this.Initializing) // We timed out trying to initialize.
            {
                this.MotorCtrl.SetSpeed(0);
                Log.Output(Log.Severity.ERROR, Log.Source.MOTORS, "Rail motor timed out while trying to initialize.");
                this.Initializing = false;
                this.InitDone = false;
            }
            else if(Event is LimitSwitchToggle && ((LimitSwitchToggle)Event).CurrentState && !this.Initializing) // We hit the end during operation.
            {
                this.MotorCtrl.SetEnabled(false); // Immediately stop.
                this.TopDepth = 0;
                this.TargetLocation = 0;
                this.TargetLocationRefIsTop = true;
                Log.Output(Log.Severity.WARNING, Log.Source.MOTORS, "Rail motor hit limit switch and was stopped for safety.");
            }
        }

        /// <summary> Prepares the rail for use by moving the motor all the way up to the top to find the zero position. </summary>
        public void Initialize()
        {
            this.Initializing = true;
            this.Limit.SwitchToggle += this.EventTriggered;

            System.Timers.Timer TimeoutTrigger = new System.Timers.Timer() { Interval = INIT_TIMEOUT, AutoReset = false };
            TimeoutTrigger.Elapsed += this.EventTriggered;
            TimeoutTrigger.Enabled = true;
            this.Encoder.UpdateState();
            this.LastEncoderCount = this.Encoder.Count;
            if (this.Limit.State) // We are already at the top, nothing needs to be done.
            {
                Log.Output(Log.Severity.INFO, Log.Source.MOTORS, "Rail already at top. No need for init.");
                TimeoutTrigger.Enabled = false;
                this.InitDone = true;
                this.Initializing = false;
                this.TopDepth = 0;
                return;
            }
            this.MotorCtrl.SetSpeed(0.3F);
            this.MotorCtrl.SetEnabled(true);
            // Either the limit switch will be toggled, or the timeout event will happen after this.
        }

        /// <summary> Moves the rail to the highest position. </summary>
        public void GotoTop()
        {
            if(!this.InitDone) { Log.Output(Log.Severity.ERROR, Log.Source.MOTORS, "Tried to move rail to top without having done init."); return; }
            this.TargetLocation = 20;
            this.TargetLocationRefIsTop = true;
        }

        /// <summary> Move the rail to where the drill is just above the ground. </summary>
        public void GotoDrillGround()
        {
            if (!this.InitDone) { Log.Output(Log.Severity.ERROR, Log.Source.MOTORS, "Tried to move rail to ground without having done init."); return; }
            this.TargetLocation = 200;
            this.TargetLocationRefIsTop = false;
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
            this.Encoder.UpdateState();
            this.TopDepth += ((this.LastEncoderCount - this.Encoder.Count) * ENCODER_MM_PER_TICK);
            this.LastEncoderCount = this.Encoder.Count;
            //if (this.TraceLogging) { Log.Trace(this, "Encoder now at " + this.Encoder.Count); }

            DateTime Sample = DateTime.Now;
            byte[] Data = UtilData.ToBytes(this.Encoder.Count).Concat(UtilData.ToBytes(Sample.Ticks)).ToArray();
            Packet Packet = new Packet(new Message(ScienceConstants.Packets.TESTING, Data), false);
            Client.Send(Packet);
            //this.Ranger.UpdateState();
            //if (this.TraceLogging) { Log.Trace(this, "Ranger seeing " + this.Ranger.GetDistance() + "mm."); }
            // TODO: Send commands to Talon.

            if (!this.InitDone) { return; } // Don't try to move the rail if we don't know where we are.

            if (this.TraceLogging) { Log.Trace(this, "Rail at " + this.TopDepth.ToString("F2") + "mm from top, and wants to be at " + this.TargetLocation.ToString("F2") + "mm from " + (this.TargetLocationRefIsTop ? "top" : "bottom") + "."); }

            if (this.TargetLocationRefIsTop && (Math.Abs(this.TargetLocation - this.TopDepth) > 5)) // The rail needs to be moved.
            {
                if (this.TraceLogging) { Log.Trace(this, "Moving at " + (this.RailSpeed * (((this.TargetLocation - this.TopDepth) > 0) ? -1 : 1))); }
                this.MotorCtrl.SetSpeed(this.RailSpeed * (((this.TargetLocation - this.TopDepth) > 0) ? -1 : 1));
            }
            else { this.MotorCtrl.SetSpeed(0); }
        }

        public void Exit()
        {
            this.TargetLocation = this.TopDepth;
            this.MotorCtrl.SetEnabled(false);
        }

        private class LEDController
        {
            private RGBLED LED;
            private int Blinking = -1; // -1 for no, otherwise colour.

            public LEDController(RGBLED LED)
            {
                this.LED = LED;
                new Thread(new ThreadStart(this.DoUpdates)).Start();
            }

            public void UnexpectedLimit()
            {
                
            }

            public void InitStateChange(bool NewState)
            {
                if(!NewState) // Initializtion stopped
                {
                    //
                }
            }

            // Run on a seperate thread to keep LED updates from slowing down Rail controls.
            private void DoUpdates()
            {

            }
        }
    }
}
