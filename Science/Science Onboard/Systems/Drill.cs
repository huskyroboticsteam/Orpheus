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
        private const float MOTOR_MAX_SPEED = 0.2F;

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
        private LimitSwitch LimitSwitch;

        public Drill(IPWMOutput MotorPWM, IPWMOutput ServoPWM)
        {
            this.Out = MotorPWM;
            ((Scarlet.Components.Outputs.PCA9685.PWMOutputPCA9685)MotorPWM).Reset();
            //this.MotorCtrl = new TalonMC(MotorPWM, MOTOR_MAX_SPEED);
            //this.DoorServo = new Servo(ServoPWM);
            //this.LimitSwitch = new LimitSwitch(new DigitalInPi(11));
            //this.LimitSwitch.SwitchToggle += this.SwitchToggle;
            new Thread(new ThreadStart(DoDrive)).Start();
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

        public void EventTriggered(object Sender, EventArgs Event)
        {
            
        }

        public void SetSpeed(byte Percent, bool Reverse)
        {
            Log.Output(Log.Severity.DEBUG, Log.Source.MOTORS, "Drill outputting " + (Reverse ? "-" : "") + Percent + "%.");
            //this.MotorCtrl.SetSpeed((Percent / 100) * (Reverse ? -1 : 1));
            //this.MotorCtrl.SetEnabled(Percent != 0);
            this.Out.SetOutput(0.4F);
            this.Out.SetEnabled(true);
        }

        public void UpdateState()
        {
            
        }

        private void DoDrive()
        {
            int i = 0;
            while (true)
            {
                SetSpeed((byte)((Math.Sin(i * 0.25F) + 1) * 20), false);
                //this.LimitSwitch.UpdateState();
                Thread.Sleep(50);
                i++;
            }
        }

        public void Initialize()
        {
            
        }
    }
}
