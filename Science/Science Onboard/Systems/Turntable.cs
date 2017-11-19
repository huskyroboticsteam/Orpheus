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
    class Turntable : ISubsystem
    {
        private const float MOTOR_MAX_SPEED = 0.30F;
        private const int INIT_TIMEOUT = 20000;

        private bool Initializing, InitDone;
        private int CurrentAngle;
        public int TargetAngle;

        private TalonMC MotorCtrl;
        private LimitSwitch Limit;
        private Encoder Encoder;

        public Turntable()
        {
            BBBPinManager.AddMappingPWM(BBBPin.P9_14);
            BBBPinManager.AddMappingGPIO(BBBPin.P8_12, false, ResistorState.PULL_UP);
        }

        public void EventTriggered(object Sender, EventArgs Event)
        {
            if(Event is LimitSwitchToggle && this.Initializing) // We hit the end.
            {
                this.MotorCtrl.Stop();
                this.CurrentAngle = 0;
                Log.Output(Log.Severity.DEBUG, Log.Source.MOTORS, "Turntable motor finished initializing.");
                this.Initializing = false;
                this.InitDone = true;
            }
            if(Event is ElapsedEventArgs && this.Initializing) // We timed out trying to initialize.
            {
                this.MotorCtrl.Stop();
                Log.Output(Log.Severity.ERROR, Log.Source.MOTORS, "Turntable motor timed out while attempting to initialize.");
                this.Initializing = false;
            }
            if(Event is LimitSwitchToggle)
            {
                this.CurrentAngle = 0;
            }
            if(Event is EncoderTurn)
            {
                // TODO: Track this.
            }
        }

        public void Initialize()
        {
            this.Initializing = true;

            IPWMOutput MotorOut = PWMBBB.PWMDevice1.OutputA;
            IDigitalIn LimitSw = new DigitalInBBB(BBBPin.P8_12);
            this.MotorCtrl = new TalonMC(MotorOut, MOTOR_MAX_SPEED);
            this.Limit = new LimitSwitch(LimitSw, false);
            //this.Encoder = new Encoder(6, 7, 420);

            this.Limit.SwitchToggle += this.EventTriggered;
            //this.Encoder.Turned += this.EventTriggered;

            Timer TimeoutTrigger = new Timer() { Interval = INIT_TIMEOUT, AutoReset = false };
            TimeoutTrigger.Elapsed += this.EventTriggered;
            TimeoutTrigger.Enabled = true;
            // Do init stuff.
            this.TargetAngle = 360;
        }

        public void EmergencyStop()
        {
            this.TargetAngle = this.CurrentAngle;
            this.MotorCtrl.Stop();
            this.UpdateState();
        }

        public void UpdateState()
        {
            if (!this.InitDone)
            {
                Log.Output(Log.Severity.WARNING, Log.Source.SUBSYSTEM, "Turntable has not been initialized yet.");
                //return;
            }
            this.Limit.UpdateState();
            //this.Encoder.UpdateState();
            Log.Output(Log.Severity.DEBUG, Log.Source.SUBSYSTEM, "Turntable: Current Angle: " + this.CurrentAngle + ", Target: " + this.TargetAngle + ", Outputting " + (float)Math.Sin(this.Cycle / 50.000)*0.3F);
            this.MotorCtrl.SetSpeed((this.TargetAngle > this.CurrentAngle) ? -0.1F : 0.1F);
        }
    }
}
