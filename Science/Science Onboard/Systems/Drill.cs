using System;
using System.Threading;
using Scarlet.Components;
using Scarlet.Components.Motors;
using Scarlet.Components.Sensors;
using Scarlet.IO;
using Scarlet.IO.BeagleBone;
using Scarlet.IO.RaspberryPi;
using Scarlet.Utilities;

namespace Science.Systems
{
    public class Drill : ISubsystem
    {
        private const float MOTOR_MAX_SPEED = 1F;

        /*private bool P_DoorOpen;
        public bool DoorOpen
        {
            get { return this.P_DoorOpen; }
            set
            {
                this.DoorServo.SetPosition(value ? 300 : 0);
                this.P_DoorOpen = value;
            }
        }*/

        private TalonMC MotorCtrl;
        //private Servo DoorServo;
        private IPWMOutput Out;

        public Drill(IPWMOutput MotorPWM, IPWMOutput ServoPWM)
        {
            this.Out = MotorPWM;
            ((Scarlet.Components.Outputs.PCA9685.PWMOutputPCA9685)MotorPWM).Reset();
            ((Scarlet.Components.Outputs.PCA9685.PWMOutputPCA9685)MotorPWM).SetPolarity(true);
            this.MotorCtrl = new TalonMC(MotorPWM, MOTOR_MAX_SPEED);
            //this.DoorServo = new Servo(ServoPWM);
        }

        public void SwitchToggle(object Sender, EventArgs evt)
        {
            Log.Output(Log.Severity.INFO, Log.Source.MOTORS, "Limit switch toggled.");
        }

        public void EmergencyStop()
        {
            this.MotorCtrl.SetEnabled(false);
            //this.DoorServo.SetEnabled(false);
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
    }
}
