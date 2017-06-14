using RoboticsLibrary.Sensors;
using RoboticsLibrary.Motors;
using RoboticsLibrary.Utilities;
using Science.Motors;

namespace Science
{
    class IOHandler
    {
        public LimitSwitch RailLimit { get; private set; }
        public Encoder RailEncoder { get; private set; }
        public Motor RailMotor { get; private set; }

        public LimitSwitch TurntableZero { get; private set; }
        public Encoder TurntableEncoder { get; private set; }
        public Motor TurntableMotor { get; private set; }

        public LimitSwitch ToolheadLimit { get; private set; }
        public Encoder ToolheadEncoder { get; private set; }
        public Motor ToolheadMotor { get; private set; }

        public Servo SampleDoorServo { get; private set; }

        public Motor DrillMotor { get; private set; }

        public IOHandler()
        {
            Log.Output(Log.Severity.DEBUG, Log.Source.SENSORS, "Initializing I/O handler and all components...");

            // TODO: Define these pins properly.
            this.RailLimit = new LimitSwitch(0, false);
            this.RailEncoder = new Encoder(0, 0, 80);
            //this.RailMotor = new Motor(0); // TODO: Replace this with a application-specific class.
            this.RailLimit.SwitchToggle += this.RailMotor.EventTriggered;
            this.RailEncoder.Turned += this.RailMotor.EventTriggered;

            this.TurntableZero = new LimitSwitch(1, false);
            this.TurntableEncoder = new Encoder(1, 1, 420);
            this.TurntableMotor = new MotorTurntable(1);
            this.TurntableZero.SwitchToggle += this.TurntableMotor.EventTriggered;
            this.TurntableEncoder.Turned += this.TurntableMotor.EventTriggered;

            this.ToolheadLimit = new LimitSwitch(2, false);
            this.ToolheadEncoder = new Encoder(2, 2, 80);
            //this.ToolheadMotor = new Motor(2); // TODO: Replace this with a application-specific class.
            this.ToolheadLimit.SwitchToggle += this.ToolheadMotor.EventTriggered;
            this.ToolheadEncoder.Turned += this.ToolheadMotor.EventTriggered;

            //this.SampleDoorServo = new Servo(); // TODO: Replace this with a application-specific class.

            //this.DrillMotor = new Motor(3); // TODO: Replace this with a application-specific class.
        }

        public void InitializeMotors()
        {
            this.RailMotor.Initialize();
            this.TurntableMotor.Initialize();
            this.ToolheadMotor.Initialize();
        }

        public void StopAllMotors()
        {
            this.RailMotor.Stop();
            this.TurntableMotor.Stop();
            this.ToolheadMotor.Stop();
        }
    }
}
