using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Scarlet.Communications;
using Scarlet.Components;
using Scarlet.Components.Motors;
using Scarlet.Components.Sensors;
using Scarlet.IO;
using Scarlet.IO.Utilities;
using Scarlet.Utilities;
using Science.Library;

namespace Science.Systems
{
    public class Turntable : ISubsystem
    {
        public bool TraceLogging { get; set; }

        private const float MOTOR_MAX_SPEED = 0.99F;
        private const int INIT_TIMEOUT = 5500;
        private const float ENCODER_DEG_PER_TICK = -4.3F; // TODO: Update this value.
        private const bool ENABLE_VELOCITY_TRACKING = false;

        private bool InitDone = false;
        private bool Initializing = false;
        private double Angle = 0;
        private int LastEncoderCount;
        private double TargetAngle;

        private readonly PololuHPMDG2 MotorCtrl;
        private readonly LimitSwitch Limit;
        private readonly LS7366R Encoder;

        public Turntable(IPWMOutput MotorPWM, IDigitalOut MotorDir, ISPIBus SPI, IDigitalOut CS_Encoder, IDigitalIn LimitSw)
        {
            this.MotorCtrl = new PololuHPMDG2(MotorPWM, MotorDir, MOTOR_MAX_SPEED);
            this.Limit = new LimitSwitch(new SoftwareInterrupt(LimitSw) { TraceLogging = true});
            this.Encoder = new LS7366R(SPI, CS_Encoder);
            LS7366R.Configuration Config = LS7366R.DefaultConfig;
            Config.QuadMode = LS7366R.QuadMode.X4_QUAD;
            this.Encoder.Configure(Config);
            this.Limit.SwitchToggle += this.EventTriggered;
        }

        private void EventTriggered(object Sender, EventArgs Event)
        {
            if (Event is LimitSwitchToggle && this.Initializing) // We hit the end.
            {
                this.MotorCtrl.SetSpeed(0);
                this.Angle = 0;
                Log.Output(Log.Severity.INFO, Log.Source.MOTORS, "Turntable motor finished initializing.");
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
                this.MotorCtrl.SetSpeed(0);
                this.Angle = 0;
                this.TargetAngle = 0;
                Log.Output(Log.Severity.WARNING, Log.Source.MOTORS, "Turntable motor hit limit switch and was stopped for safety.");
            }
        }

        public void Initialize() { } // Don't initialize at startup.

        public void DoInit()
        {
            Log.Output(Log.Severity.INFO, Log.Source.SUBSYSTEM, "Starting turntable initialization.");
            this.Initializing = true;
            
            System.Timers.Timer TimeoutTrigger = new System.Timers.Timer() { Interval = (ENABLE_VELOCITY_TRACKING ? 30000 : INIT_TIMEOUT), AutoReset = false };
            TimeoutTrigger.Elapsed += this.EventTriggered;
            TimeoutTrigger.Enabled = true;
            this.Encoder.UpdateState();
            this.TargetAngle = 0;
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
            this.MotorCtrl.SetSpeed(0.2F);
            this.MotorCtrl.SetEnabled(true);
            // Either the limit switch will be toggled, or the timeout event will happen after this.
        }

        /// <summary> Moves to a new location on the turntable. </summary>
        public void GotoLocation(int Angle)
        {
            if (!this.InitDone) { Log.Output(Log.Severity.ERROR, Log.Source.MOTORS, "Tried to move turntable without having done init."); return; }
            this.TargetAngle = Angle;
        }

        public void UpdateState()
        {
            this.Limit.UpdateState();
            this.Encoder.UpdateState();
            this.Angle += ((this.LastEncoderCount - this.Encoder.Count) * ENCODER_DEG_PER_TICK);
            this.LastEncoderCount = this.Encoder.Count;
            if (this.TargetAngle < 0) { this.TargetAngle = 0; }

            if (this.TraceLogging) { Log.Trace(this, "TTB @ " + this.Angle + ", target " + this.TargetAngle + ", encoder " + this.Encoder.Count); }

            try
            {
                byte[] Data = UtilData.ToBytes(DateTime.Now.Ticks)
                    .Concat(new byte[] { (byte)((this.InitDone ? 0b1 : 0b0) | (this.Initializing ? 0b10 : 0b00) | (RoverMain.IOHandler.Microscope.Busy ? 0b100 : 0b000)) }) // Basic Data
                    .Concat(UtilData.ToBytes((float)this.Angle))
                    .Concat(UtilData.ToBytes((float)this.TargetAngle))
                    .ToArray();
                Packet Packet = new Packet(new Message(ScienceConstants.Packets.TTB_STATUS, Data), false);
                Client.Send(Packet);
            }
            catch (Exception Exc)
            {
                Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "Failed to send rail status packet.");
                Log.Exception(Log.Source.NETWORK, Exc);
            }

            if (!this.InitDone) { return; } // Don't try to move the turntable if we don't know where we are.

            float SpeedUsed = 0.2F;
            if (Math.Abs(this.TargetAngle - this.Angle) < 2) { SpeedUsed = 0; } // We are close enough, stop.
            else if (Math.Abs(this.TargetAngle - this.Angle) >= 30) { SpeedUsed = 0.2F; }
            else { SpeedUsed = (float)(0.15F + (0.05F * (Math.Abs(this.TargetAngle - this.Angle)) / 30F)); }

            if (this.TargetAngle > this.Angle) { SpeedUsed *= -1; }

            this.MotorCtrl.SetSpeed(SpeedUsed);
        }

        public void EmergencyStop()
        {
            this.TargetAngle = this.Angle;
            this.MotorCtrl.SetEnabled(false);
        }

        public void Exit()
        {
            this.MotorCtrl.SetEnabled(false);
        }

    }
}
