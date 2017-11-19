using System;
using Scarlet.Components;
using Scarlet.Components.Motors;
using Scarlet.Components.Sensors;
using Scarlet.IO;
using Scarlet.IO.BeagleBone;

namespace Science.Systems
{
    public class Toolhead : ISubsystem
    {
        private const float MOTOR_MAX_SPEED = 0.05F;

        private int Angle;

        private TalonMC MotorCtrl;
        private Potentiometer Pot;

        public Toolhead()
        {
            BBBPinManager.AddMappingPWM(BBBPin.P9_16);
        }

        public void EmergencyStop()
        {
            this.MotorCtrl.Stop();
            this.UpdateState();
        }

        public void EventTriggered(object Sender, EventArgs Event)
        {
            if(Event is PotentiometerTurn)
            {
                // TODO: Track this.
            }
        }

        public void Initialize()
        {
            IPWMOutput MotorOut = PWMBBB.PWMDevice1.OutputB;
            this.MotorCtrl = new TalonMC(MotorOut, MOTOR_MAX_SPEED);
            //this.Pot = new Potentiometer(null, 300);

            //this.Pot.Turned += this.EventTriggered;

            this.GotoDrill();
        }

        public void GotoMoisture()
        {
            // TODO: Implement.
        }

        public void GotoDrill()
        {
            // TODO: Implement.
        }

        public void GotoThermo()
        {
            // TODO: Implement.
        }

        public void UpdateState()
        {
            //this.Pot.UpdateState();
            // TODO: Calculate and set motor speed.
        }
    }
}
