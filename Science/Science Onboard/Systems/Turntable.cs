using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Scarlet.Components;
using Scarlet.Components.Motors;
using Scarlet.Components.Sensors;
using Scarlet.IO;
using Scarlet.IO.Utilities;
using Scarlet.Utilities;

namespace Science.Systems
{
    public class Turntable : ISubsystem
    {
        public bool TraceLogging { get; set; }

        private const float MOTOR_MAX_SPEED = 1F;
        private const int INIT_TIMEOUT = 5000;
        private const float ENCODER_DEG_PER_TICK = 0.935F; // TODO: Update this value.
        private const bool ENABLE_VELOCITY_TRACKING = true;

        private bool InitDone = false;
        private bool Initializing = false;
        private double Angle = 0;
        private int LastEncoderCount;
        public double TargetLocation;

        private readonly PololuHPMDG2 MotorCtrl;
        private readonly LimitSwitch Limit;
        private readonly LS7366R Encoder;

        public Turntable(IPWMOutput MotorPWM, IDigitalOut MotorDir, ISPIBus SPI, IDigitalOut CS_Encoder, IDigitalIn LimitSw)
        {
            this.MotorCtrl = new PololuHPMDG2(MotorPWM, MotorDir, MOTOR_MAX_SPEED);
            this.Limit = new LimitSwitch(new SoftwareInterrupt(LimitSw));
            this.Encoder = new LS7366R(SPI, CS_Encoder);
            LS7366R.Configuration Config = LS7366R.DefaultConfig;
            Config.QuadMode = LS7366R.QuadMode.X4_QUAD;
            this.Encoder.Configure(Config);
        }

        private void EventTriggered(object Sender, EventArgs Event)
        {
            if (Event is LimitSwitchToggle && this.Initializing) // We hit the end.
            {
                this.MotorCtrl.SetSpeed(0);
                this.Angle = 0;
                Log.Output(Log.Severity.DEBUG, Log.Source.MOTORS, "Turntable motor finished initializing.");
                this.Initializing = false;
                this.InitDone = true;
            }
            else if (Event is ElapsedEventArgs && this.Initializing) // We timed out trying to initialize.
            {
                this.MotorCtrl.SetSpeed(0);
                Log.Output(Log.Severity.ERROR, Log.Source.MOTORS, "Turntable motor timed out while trying to initialize.");
                this.Initializing = false;
                this.InitDone = false;
            }
            else if (Event is LimitSwitchToggle && ((LimitSwitchToggle)Event).CurrentState && !this.Initializing) // We hit the end during operation.
            {
                this.MotorCtrl.SetEnabled(false); // Immediately stop.
                this.Angle = 0;
                this.TargetLocation = 0;
                Log.Output(Log.Severity.WARNING, Log.Source.MOTORS, "Turntable motor hit limit switch and was stopped for safety.");
            }
        }

        public void Initialize()
        {
            this.Initializing = true;
            this.Limit.SwitchToggle += this.EventTriggered;

            System.Timers.Timer TimeoutTrigger = new System.Timers.Timer() { Interval = (ENABLE_VELOCITY_TRACKING ? 30000 : INIT_TIMEOUT), AutoReset = false };
            TimeoutTrigger.Elapsed += this.EventTriggered;
            TimeoutTrigger.Enabled = true;
            this.Encoder.UpdateState();
            this.LastEncoderCount = this.Encoder.Count;
            if (this.Limit.State) // We are already at the top, nothing needs to be done.
            {
                Log.Output(Log.Severity.INFO, Log.Source.MOTORS, "Turntable already at home. No need for init.");
                TimeoutTrigger.Enabled = false;
                this.InitDone = true;
                this.Initializing = false;
                this.Angle = 0;
                return;
            }
            this.MotorCtrl.SetSpeed(0.2F); // TODO: Check polarity
            this.MotorCtrl.SetEnabled(true);
            // Either the limit switch will be toggled, or the timeout event will happen after this.
        }

        /// <summary> Moves to a preset location on the turntable. </summary>
        /// <param name="LocationID"> 0 for home position, 1/2/3 for cup locations. </param>
        public void GotoLocation(int LocationID)
        {
            if (!this.InitDone) { Log.Output(Log.Severity.ERROR, Log.Source.MOTORS, "Tried to move turntable without having done init."); return; }
            switch (LocationID)
            {
                case 0: this.TargetLocation = 0; break;
                case 1: this.TargetLocation = 20; break;
                case 2: this.TargetLocation = 50; break;
                case 3: this.TargetLocation = 80; break;
            }
        }

        public void UpdateState()
        {
            
        }

        public void EmergencyStop()
        {
            this.TargetLocation = this.Angle;
            this.MotorCtrl.SetEnabled(false);
        }

        public void Exit()
        {
            this.MotorCtrl.SetEnabled(false);
        }

    }
}
