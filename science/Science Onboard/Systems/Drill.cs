using System;
using Scarlet.Components;
using Scarlet.Components.Motors;

namespace Science.Systems
{
    public class Drill : ISubsystem
    {
        private const float MOTOR_MAX_SPEED = 1.0F;

        public bool DoorOpen
        {
            get { return this.DoorOpen; }
            set
            {
                this.DoorServo.Position = value ? 300 : 0;
            }
        }

        private readonly TalonMC MotorCtrl;
        private readonly Servo DoorServo;

        public Drill()
        {
            this.MotorCtrl = new TalonMC(0, MOTOR_MAX_SPEED);
            this.DoorServo = new Servo(1);
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
            this.MotorCtrl.Initialize();
            this.DoorServo.Initialize();
            this.DoorOpen = false;
        }

        public void UpdateState()
        {
            this.MotorCtrl.UpdateState();
            this.DoorServo.UpdateState();
        }

    }
}
