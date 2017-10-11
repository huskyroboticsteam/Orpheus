using System;
using Scarlet.Components;
using Scarlet.Components.Motors;

namespace Science.Systems
{
    public class Drill : ISubsystem
    {
        private const float MOTOR_MAX_SPEED = 1.0F;

        private bool P_DoorOpen;
        public bool DoorOpen
        {
            get { return this.P_DoorOpen; }
            set
            {
                this.DoorServo.TargetPosition = value ? 300 : 0;
                this.P_DoorOpen = value;
            }
        }

        private readonly TalonMC MotorCtrl;
        private readonly Servo DoorServo;

        public Drill()
        {
            this.MotorCtrl = new TalonMC(null, MOTOR_MAX_SPEED); // TODO: Provide actual IPWMOuput.
            this.DoorServo = new Servo(null); // TODO: Provide actual IPWMOuput.
        }

        public void EmergencyStop()
        {
            this.MotorCtrl.Stop();
            this.DoorServo.Stop();
        }

        public void EventTriggered(object Sender, EventArgs Event)
        {
            
        }

        public void Initialize()
        {
            this.DoorOpen = false;
        }

        public void UpdateState()
        {
            this.MotorCtrl.UpdateState();
            this.DoorServo.UpdateState();
        }

    }
}
