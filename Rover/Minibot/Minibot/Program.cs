using System;
using Scarlet.IO.BeagleBone;
using Scarlet.Components.Motors;
using System.Threading;
using Scarlet.Utilities;
using OpenTK.Input;
using Scarlet.Components.Sensors;
using Scarlet.IO;

namespace Minibot
{
    class MainClass
    {
        static float MAX_SPEED = 200.0f;
        static bool ReceivingInput(GamePadState State)
        {
            return State.Triggers.Left <= Double.Epsilon && State.Triggers.Right <= Double.Epsilon;
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("Initializing Minibot Code");
            StateStore.Start("k den");
            BeagleBone.Initialize(SystemMode.DEFAULT, true);


            BBBPinManager.AddMappingsI2C(BBBPin.P9_19, BBBPin.P9_20); //Motors
            BBBPinManager.AddMappingsI2C(BBBPin.P9_17, BBBPin.P9_18); //Motors
            BBBPinManager.AddMappingUART(BBBPin.P9_24); //UART_1 TX GPS
            BBBPinManager.AddMappingUART(BBBPin.P9_26); //UART_1 RX GPS
            BBBPinManager.AddMappingsI2C(BBBPin.P9_21, BBBPin.P9_22); //Magnetometer Bus 2
            BBBPinManager.ApplyPinSettings(BBBPinManager.ApplicationMode.APPLY_IF_NONE);

            IUARTBus UARTBus = UARTBBB.UARTBus1;
            I2CBusBBB I2C1 = I2CBBB.I2CBus1;
            I2CBusBBB I2C2 = I2CBBB.I2CBus2;

            MTK3339 gps = new MTK3339(UARTBus);
            BNO055 magnetometer = new BNO055(I2C2);


            AdafruitMotor[] MinibotMotors = new AdafruitMotor[8];

            for (int i = 0; i < 4; i++)
            {
                var pwm = new AdafruitMotorPWM((AdafruitMotorPWM.Motors)i, I2C1);
                MinibotMotors[i] = new AdafruitMotor(pwm);
            }
            for (int i = 0; i < 4; i++)
            {
                var pwm = new AdafruitMotorPWM((AdafruitMotorPWM.Motors)i, I2C2);
                MinibotMotors[i + 4] = new AdafruitMotor(pwm);
            }

            //Tests the wheel motors
            /*
            for (int i = 0; i < 8; i++)
            {
                Console.WriteLine("Motor " + i);
                MinibotMotors[i].SetSpeed(10);
                Thread.Sleep(500);
                MinibotMotors[i].SetSpeed(0);
                Thread.Sleep(500);
            }
            */
            MinibotMotors[0].SetSpeed(0.0f);
            MinibotMotors[1].SetSpeed(0.0f);
            MinibotMotors[2].SetSpeed(0.0f);
            MinibotMotors[3].SetSpeed(0.0f);
            MinibotMotors[7].SetSpeed(0.0f);

            magnetometer.SetMode(BNO055.OperationMode.OPERATION_MODE_MAGONLY);

            Console.WriteLine("initialized");
            var orientation = 0.0f;
            while(true) 
            {
                GamePadState State = GamePad.GetState(0);
                if (State.Buttons.A == ButtonState.Pressed)
                {
                    do
                    {
                        State = GamePad.GetState(0);
                        Console.WriteLine("Reading");
                        if (State.IsConnected)
                        {                            
                            float rightSpeed = State.Triggers.Right;
                            float leftSpeed = State.Triggers.Left;
                            float speed = rightSpeed - leftSpeed;
                            Console.WriteLine($"Left: {State.ThumbSticks.Left.Y}, Right: {State.ThumbSticks.Right.Y}");
                            Console.WriteLine("Left Trigger: " + leftSpeed);
                            Console.WriteLine("Right Trigger: " + rightSpeed);
                            MinibotMotors[0].SetSpeed((100.0f) * speed);
                            MinibotMotors[1].SetSpeed((-100.0f) * speed);
                            MinibotMotors[2].SetSpeed((100.0f) * speed);
                            MinibotMotors[3].SetSpeed((100.0f) * speed);
                            MinibotMotors[7].SetSpeed((-75.0f) * (State.ThumbSticks.Left.X));
                        }
                        else
                        {
                            Console.WriteLine("NOT CONNECTED");
                        }
                    }
                    while (State.Buttons.Start != ButtonState.Pressed && State.Buttons.B != ButtonState.Pressed);
                }
                else 
                {
                    if(gps.HasFix()) 
                    {
                        var tuple = gps.GetCoords();
                        //Console.WriteLine($"({tuple.Item1}, {tuple.Item2})");
                        Thread.Sleep(150);
                    }
                    var xTesla = magnetometer.GetVector(BNO055.VectorType.VECTOR_MAGNETOMETER).Item1;
                    var yTesla = magnetometer.GetVector(BNO055.VectorType.VECTOR_MAGNETOMETER).Item2;
                    var zTesla = magnetometer.GetVector(BNO055.VectorType.VECTOR_MAGNETOMETER).Item3;
                    Console.WriteLine("xTesla: " + xTesla);
                    Console.WriteLine("yTesla: " + yTesla);
                    //Console.WriteLine(zTesla);
                    var arcTan = Math.Atan2(yTesla, xTesla);

                    if(yTesla > 0) {
                        orientation = (float)(90  - (arcTan * (180 / Math.PI)));
                    } else if (yTesla < 0) {
                        orientation = (float)(270 - (arcTan * (180 / Math.PI)));
                    } else if(Math.Abs(yTesla) < 1e-6  && xTesla < 0) {
                        orientation = 180.0f;
                    } else if(Math.Abs(yTesla) < 1e-6 && xTesla > 0) {
                        orientation = 0.0f;
                    }
                    Console.WriteLine(orientation);
                    Thread.Sleep(500);

                }
            }
        }
    }
}