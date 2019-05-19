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
        private bool Enabled = false;
        private IDigitalIn Fault;

        public Drill(IPWMOutput MotorPWM, IDigitalOut MotorDir, IDigitalIn MotorFault, IPWMOutput ServoPWM)
        {
            this.Out = MotorPWM;
            //((Scarlet.Components.Outputs.PCA9685.PWMOutputPCA9685)MotorPWM).Reset();
            //((Scarlet.Components.Outputs.PCA9685.PWMOutputPCA9685)MotorPWM).SetPolarity(true);
            this.MotorCtrl = new PololuHPMDG2(MotorPWM, MotorDir, MOTOR_MAX_SPEED);
            this.DoorServo = new Servo(ServoPWM) { TraceLogging = true };
            this.DoorServo.SetEnabled(true);
            this.Fault = MotorFault;
        }

        public void EmergencyStop()
        {
            this.Enabled = false;
            this.MotorCtrl.SetEnabled(false);
            this.DoorServo.SetEnabled(false);
        }

        public void SetSpeed(float Speed, bool Enable)
        {
            Log.Output(Log.Severity.INFO, Log.Source.MOTORS, "Setting drill speed: " + Speed + " (" + Enable + "). Fault? " + !this.Fault.GetInput());
            if (this.Enabled != Enable) { this.MotorCtrl.SetEnabled(Enable); }
            this.Enabled = Enable;
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
