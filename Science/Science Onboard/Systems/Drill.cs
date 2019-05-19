using System;
using System.Threading;
using Scarlet.Components;
using Scarlet.Components.Motors;
using Scarlet.Components.Sensors;
using Scarlet.IO;
using Scarlet.Utilities;

namespace Science.Systems
{
    public class Drill : ISubsystem
    {
        public bool TraceLogging { get; set; }

        private const float MOTOR_MAX_SPEED = 1F;

        private bool P_DoorOpen;
        public bool DoorOpen
        {
            get { return this.P_DoorOpen; }
            set
            {
                this.DoorServo.SetPosition(value ? 0 : 300);
                this.P_DoorOpen = value;
            }
        }

        private readonly PololuHPMDG2 MotorCtrl;
        private readonly Servo DoorServo;
        private readonly IPWMOutput Out;
        private bool MotorEnabled = false;

        public Drill(IPWMOutput MotorPWM, IDigitalOut MotorDir, IPWMOutput ServoPWM)
        {
            this.Out = MotorPWM;
            this.MotorCtrl = new PololuHPMDG2(MotorPWM, MotorDir, MOTOR_MAX_SPEED);
            this.DoorServo = new Servo(ServoPWM) { TraceLogging = true };
            this.DoorServo.SetEnabled(true);
        }

        public void EmergencyStop()
        {
            this.MotorEnabled = false;
            this.MotorCtrl.SetEnabled(false);
            this.DoorServo.SetEnabled(false);
        }

        public void SetSpeed(float Speed, bool Enable)
        {
            if (this.TraceLogging) { Log.Trace(this, "Setting drill speed: " + Speed + " (" + Enable + ")."); }
            if (this.MotorEnabled != Enable) { this.MotorCtrl.SetEnabled(Enable); }
            this.MotorEnabled = Enable;
            if (Enable) { this.MotorCtrl.SetSpeed(Speed); }
            else { this.MotorCtrl.SetSpeed(0); }
        }

        public void UpdateState() { }

        public void Initialize() { }

        public void Exit()
        {
            this.MotorCtrl.SetEnabled(false);
            this.DoorServo.SetEnabled(false);
        }
    }
}
