using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BBBCSIO;
using Scarlet.Components.Motors;
using Scarlet.Components.Sensors;
using Scarlet.Filters;
using Scarlet.IO;
using Scarlet.IO.BeagleBone;
using Scarlet.Utilities;

namespace Science
{
    class BBBTests
    {
        private static IDigitalIn IntTestIn;

        internal static void TestDigO()
        {
            BBBPinManager.AddMappingGPIO(BBBPin.P8_08, true, Scarlet.IO.ResistorState.PULL_DOWN);
            BBBPinManager.ApplyPinSettings(RoverMain.ApplyDevTree);
            IDigitalOut Output = new DigitalOutBBB(BBBPin.P8_08);
            bool Value = false;
            for (int i = 0; i < 50; i++)
            {
                Output.SetOutput(Value);
                Value = !Value;
                Thread.Sleep(100);
            }
            Output.SetOutput(false);
        }

        internal static void TestDigI()
        {
            BBBPinManager.AddMappingGPIO(BBBPin.P9_12, false, Scarlet.IO.ResistorState.PULL_DOWN);
            //if (ApplyDevTree) { BBBPinManager.ApplyPinSettings(); }
            IDigitalIn Input = new DigitalInBBB(BBBPin.P9_12);
            for (int i = 0; i < 50; i++)
            {
                Log.Output(Log.Severity.DEBUG, Log.Source.HARDWAREIO, "Input is " + Input.GetInput());
                Thread.Sleep(250);
            }
        }

        internal static void TestPWM()
        {
            BBBPinManager.AddMappingPWM(BBBPin.P9_14);
            BBBPinManager.AddMappingPWM(BBBPin.P9_16);
            BBBPinManager.ApplyPinSettings(RoverMain.ApplyDevTree);
            IPWMOutput OutA = PWMBBB.PWMDevice1.OutputA;
            IPWMOutput OutB = PWMBBB.PWMDevice1.OutputB;
            PWMBBB.PWMDevice1.SetFrequency(5000);
            OutA.SetEnabled(true);
            OutB.SetEnabled(true);
            int Cycle = 0;
            while (true)
            {
                float A = (float)((Math.Sin(Cycle * Math.PI / 180.000D) + 1) / 2); // Sine waves! Fun!
                float B = (float)((Math.Sin(Cycle * Math.PI / 360.000D) + 1) / 2);

                OutA.SetOutput(A);
                OutB.SetOutput(B);

                Thread.Sleep(50);
                Cycle += 20;
            }
        }

        internal static void TestMotor()
        {
            BBBPinManager.AddMappingPWM(BBBPin.P9_14);
            BBBPinManager.ApplyPinSettings(RoverMain.ApplyDevTree);
            IPWMOutput MotorOut = PWMBBB.PWMDevice1.OutputA;
            IFilter<float> MotorFilter = new Average<float>(5);
            TalonMC Motor = new TalonMC(MotorOut, 1F, MotorFilter);
            Log.SetSingleOutputLevel(Log.Source.MOTORS, Log.Severity.DEBUG);
            Motor.TargetSpeed = 0.2F;
            Motor.UpdateState();
            /*while (true)
            {
                Log.Output(Log.Severity.DEBUG, Log.Source.MOTORS, "Outputs: " + Motor.TargetSpeed + ", " + ((PWMOutputBBB)MotorOut).GetOutput() + ", " + ((PWMOutputBBB)MotorOut).GetFrequency());
                //Motor.UpdateState();
                Thread.Sleep(100);
            }*/
            int Cycle = 0;
            while (true)
            {
                Motor.TargetSpeed = ((Cycle / 10) % 2 == 0) ? 1 : -1;
                Motor.UpdateState();
                Thread.Sleep(25);
                Cycle += 1;
            }
        }

        internal static void TestPWMLow()
        {
            BBBPinManager.AddMappingPWM(BBBPin.P9_14);
            BBBPinManager.ApplyPinSettings(RoverMain.ApplyDevTree);

            PWMPortMM Port = new PWMPortMM(PWMPortEnum.PWM1_A);
            Port.PeriodNS = 1000000;
            Port.DutyPercent = 0;
            Port.RunState = true;
            while (true)
            {
                for (int i = 0; i < 100; i++)
                {
                    Port.DutyPercent = i;
                    Thread.Sleep(10);
                }
                Port.DutyPercent = 100;
                Thread.Sleep(50);
                for (int i = 100; i > 0; i--)
                {
                    Port.DutyPercent = i;
                    Thread.Sleep(10);
                }
                Port.DutyPercent = 0;
                Thread.Sleep(50);
            }
        }

        internal static void TestI2C()
        {
            BBBPinManager.AddMappingGPIO(BBBPin.P8_08, true, Scarlet.IO.ResistorState.PULL_DOWN);
            BBBPinManager.AddMappingsI2C(BBBPin.P9_24, BBBPin.P9_26);
            BBBPinManager.ApplyPinSettings(RoverMain.ApplyDevTree);
            VEML6070 UV = new VEML6070(I2CBBB.I2CBus1);
            Log.SetSingleOutputLevel(Log.Source.SENSORS, Log.Severity.DEBUG);
            for (int i = 0; i < 20; i++)
            {
                UV.UpdateState();
                Log.Output(Log.Severity.DEBUG, Log.Source.SENSORS, "UV Reading: " + UV.GetData());
                Thread.Sleep(200);
            }
        }

        internal static void TestSPI()
        {
            BBBPinManager.AddMappingsSPI(BBBPin.P9_21, BBBPin.NONE, BBBPin.P9_22);
            BBBPinManager.AddMappingSPI_CS(BBBPin.P9_12);
            BBBPinManager.ApplyPinSettings(RoverMain.ApplyDevTree);
            IDigitalOut CS_Thermo = new DigitalOutBBB(BBBPin.P9_12);
            MAX31855 Thermo = new MAX31855(SPIBBB.SPIBus0, CS_Thermo);
            Log.SetSingleOutputLevel(Log.Source.SENSORS, Log.Severity.DEBUG);
            for (int i = 0; i < 100; i++)
            {
                Thermo.UpdateState();
                Log.Output(Log.Severity.DEBUG, Log.Source.SENSORS, "Thermocouple Data, Faults: " + string.Format("{0:G}", Thermo.GetFaults()) + ", Internal: " + Thermo.GetInternalTemp() + ", External: " + Thermo.GetExternalTemp() + " (Raw: " + Thermo.GetRawData() + ")");
                Thread.Sleep(500);
            }
        }

        internal static void TestADC()
        {
            BBBPinManager.AddMappingADC(BBBPin.P9_36);
            BBBPinManager.ApplyPinSettings(RoverMain.ApplyDevTree);
            IAnalogueIn Input = new AnalogueInBBB(BBBPin.P9_36);
            for (int i = 0; i < 200; i++)
            {
                Log.Output(Log.Severity.DEBUG, Log.Source.HARDWAREIO, "ADC Input: " + Input.GetInput() + " (Raw: " + Input.GetRawInput() + ")");
                Thread.Sleep(100);
            }
        }

        internal static void TestInterrupt()
        {
            BBBPinManager.AddMappingGPIO(BBBPin.P9_12, true, Scarlet.IO.ResistorState.PULL_DOWN);
            BBBPinManager.ApplyPinSettings(RoverMain.ApplyDevTree);
            IntTestIn = new DigitalInBBB(BBBPin.P9_12);
            IntTestIn.RegisterInterruptHandler(GetInterrupt, InterruptType.ANY_EDGE);
            Log.Output(Log.Severity.DEBUG, Log.Source.HARDWAREIO, "Interrupt handler added.");
            while (true)
            {
                //Log.Output(Log.Severity.DEBUG, Log.Source.HARDWAREIO, "State: " + IntTestIn.GetInput());
                Thread.Sleep(100);
            }
        }

        public static void GetInterrupt(object Senser, InputInterrupt Event)
        {
            Log.Output(Log.Severity.DEBUG, Log.Source.HARDWAREIO, "Interrupt Received! Now " + Event.NewState);
        }
    }
}
