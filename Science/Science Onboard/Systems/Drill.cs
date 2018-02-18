using System;
using Scarlet.Components;
using Scarlet.Components.Motors;
using Scarlet.IO;
using Scarlet.IO.BeagleBone;

namespace Science.Systems
{
    public class Drill : ISubsystem
    {
        private const float MOTOR_MAX_SPEED = 0.1F;

        private bool P_DoorOpen;
        public bool DoorOpen
        {
            get { return this.P_DoorOpen; }
            set
            {
                this.DoorServo.SetPosition(value ? 300 : 0);
                this.P_DoorOpen = value;
            }
        }

        private TalonMC MotorCtrl;
        private Servo DoorServo;

        public Drill(IPWMOutput MotorPWM, IPWMOutput ServoPWM)
        {
            this.MotorCtrl = new TalonMC(MotorPWM, MOTOR_MAX_SPEED);
            this.DoorServo = new Servo(ServoPWM);
        }

        public void EmergencyStop()
        {
            this.MotorCtrl.SetEnabled(false);
            this.DoorServo.SetEnabled(false);
        }

        public void EventTriggered(object Sender, EventArgs Event)
        {
            
        }

        public void UpdateState()
        {

        }

        public void Initialize()
        {
            
        }
    }
}
