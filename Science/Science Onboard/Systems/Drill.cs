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
                this.DoorServo.SetPosition(value ? 150 : 30);
                this.P_DoorOpen = value;
            }
        }

        private readonly TalonMC MotorCtrl;
        private readonly Servo DoorServo;
        private readonly IPWMOutput Out;

        public Drill(IPWMOutput MotorPWM, IPWMOutput ServoPWM)
        {
            this.Out = MotorPWM;
            ((Scarlet.Components.Outputs.PCA9685.PWMOutputPCA9685)MotorPWM).Reset();
            ((Scarlet.Components.Outputs.PCA9685.PWMOutputPCA9685)MotorPWM).SetPolarity(true);
            this.MotorCtrl = new TalonMC(MotorPWM, MOTOR_MAX_SPEED);
            this.DoorServo = new Servo(ServoPWM) { TraceLogging = true };
            this.DoorServo.SetEnabled(true);
        }

        public void SwitchToggle(object Sender, EventArgs evt)
        {
            Log.Output(Log.Severity.INFO, Log.Source.MOTORS, "Limit switch toggled.");
        }

        public void EmergencyStop()
        {
            this.MotorCtrl.SetEnabled(false);
            this.DoorServo.SetEnabled(false);
        }

        public void SetSpeed(float Speed, bool Enable)
        {
            this.MotorCtrl.SetSpeed(Speed);
            //this.Out.SetOutput(0.5F);
            //this.Out.SetEnabled(true);
            this.MotorCtrl.SetEnabled(Enable);
        }

        public void UpdateState()
        {
            
        }

        public void Initialize()
        {
            
        }

        public void Exit() { }
    }
}
