using System;
using System.Linq;
using System.Threading;
using Scarlet.Communications;
using Scarlet.Components;
using Scarlet.Components.Motors;
using Scarlet.Components.Sensors;
using Scarlet.IO;
using Scarlet.Utilities;
using Science.Library;
using static Scarlet.Components.Outputs.PCA9685;

namespace Science.Systems
{
    public class Drill : ISubsystem
    {
        public bool TraceLogging { get; set; }

        private const float MOTOR_MAX_SPEED = 0.99F; // Setting motor speed to 1 will cause the PWM generator to stop oscillating, and actually stop the motor.

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
        private float Speed;

        private readonly PololuHPMDG2 MotorCtrl;
        private readonly Servo DoorServo;
        private readonly IPWMOutput Out;
        private bool MotorEnabled = false;

        public Drill(IPWMOutput MotorPWM, IDigitalOut MotorDir, IPWMOutput ServoPWM)
        {
            this.Out = MotorPWM;
            this.MotorCtrl = new PololuHPMDG2(MotorPWM, MotorDir, MOTOR_MAX_SPEED);
            ((PWMOutputPCA9685)ServoPWM).SetPolarity(true);
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
            this.Speed = Speed;
            if (Enable) { this.MotorCtrl.SetSpeed(Speed); }
            else { this.MotorCtrl.SetSpeed(0); }
        }

        public void UpdateState()
        {
            DateTime Time = DateTime.Now;
            byte[] Data = UtilData.ToBytes(Time.Ticks)
                    .Concat(new byte[] { (byte)(this.DoorOpen ? 0b1 : 0b0), (byte)this.Speed })
                    .ToArray();

            Packet Packet = new Packet(new Message(ScienceConstants.Packets.DRL_STATUS, Data), false);
            Client.Send(Packet);
        }

        public void Initialize() { }

        public void Exit()
        {
            this.MotorCtrl.SetEnabled(false);
            this.DoorServo.SetEnabled(false);
        }
    }
}
