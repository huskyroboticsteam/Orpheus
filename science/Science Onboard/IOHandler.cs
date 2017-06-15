using RoboticsLibrary.Sensors;
using RoboticsLibrary.Motors;
using RoboticsLibrary.Utilities;
using Science.Systems;

namespace Science
{
    class IOHandler
    {
        public LimitSwitch RailLimit { get; private set; }
        public Encoder RailEncoder { get; private set; }
        public Motor RailController { get; private set; }

        public LimitSwitch TurntableZero { get; private set; }
        public Encoder TurntableEncoder { get; private set; }
        public Motor TurntableController { get; private set; }

        public LimitSwitch ToolheadLimit { get; private set; }
        public Encoder ToolheadEncoder { get; private set; }
        public Motor ToolheadController { get; private set; }

        public Servo SampleDoorServo { get; private set; }

        public Motor DrillMotor { get; private set; }

        public IOHandler()
        {
            Log.Output(Log.Severity.DEBUG, Log.Source.SENSORS, "Initializing I/O handler and all components...");

            // TODO: Define these pins properly.
            this.RailLimit = new LimitSwitch(0, false);
            this.RailEncoder = new Encoder(0, 0, 80);
            this.RailController = new Rail(0);
            this.RailLimit.SwitchToggle += this.RailController.EventTriggered;
            this.RailEncoder.Turned += this.RailController.EventTriggered;

            this.TurntableZero = new LimitSwitch(1, false);
            this.TurntableEncoder = new Encoder(1, 1, 420);
            this.TurntableController = new Turntable(1);
            this.TurntableZero.SwitchToggle += this.TurntableController.EventTriggered;
            this.TurntableEncoder.Turned += this.TurntableController.EventTriggered;

            this.ToolheadLimit = new LimitSwitch(2, false);
            this.ToolheadEncoder = new Encoder(2, 2, 80); // TODO: Replace this with a potentiometer.
            //this.ToolheadController = new Motor(2); // TODO: Replace this with an application-specific class.
            this.ToolheadLimit.SwitchToggle += this.ToolheadController.EventTriggered;
            this.ToolheadEncoder.Turned += this.ToolheadController.EventTriggered;

            //this.SampleDoorServo = new Servo(); // TODO: Replace this with a application-specific class.

            //this.DrillMotor = new Motor(3); // TODO: Replace this with a application-specific class.
        }

        /// <summary>
        /// Prepares all motor-driven systems for use by zeroing them. This takes a while.
        /// </summary>
        public void InitializeMotors()
        {
            this.RailController.Initialize();
            this.TurntableController.Initialize();
            this.ToolheadController.Initialize();
        }

        /// <summary>
        /// Immediately stops all motors.
        /// </summary>
        public void StopAllMotors()
        {
            this.RailController.Stop();
            this.TurntableController.Stop();
            this.ToolheadController.Stop();
            this.SampleDoorServo.Stop();
            this.DrillMotor.Stop();
        }
    }
}
