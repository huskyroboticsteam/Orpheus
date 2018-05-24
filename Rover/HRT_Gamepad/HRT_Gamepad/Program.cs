using System;

using System.Text;

using Scarlet.Utilities;

using Scarlet.IO.BeagleBone;

using Scarlet.IO;

using System.Net.Sockets;
using System.Net;
using Scarlet.Components.Motors;
using Scarlet.Controllers;
using Scarlet.IO.BeagleBone;
using Scarlet.Components.Motors;
using System.Threading;
using System.Net;
using Scarlet.Utilities;
using OpenTK.Input;
using Scarlet.Components.Sensors;
using Scarlet.IO;
using Scarlet.Communications;

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
            Console.WriteLine("initializeds");
            var orientation = 0.0f;
            CANBusBBB canName = CANBBB.CANBus0;

            /*
            var client = new UdpClient();
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("192.168.0.20"), 9000);
            client.Connect(ep);*/
            bool usingJoystick = false;
            Client.Start("192.168.0.20", 1025, 1026, "Client 1");
            if (usingJoystick)
            {
                while (true)
                // for (int i = 0; i < 4; i++)
                {
                    //JoystickState state = new JoystickState();
                    var state = Joystick.GetState(0);
                    if (state.IsConnected)
                    {
                        float x = state.GetAxis(0);
                        float y = state.GetAxis(1);

                        // Print the current state of the joystick
                        //Console.WriteLine(state);
                        Console.WriteLine("X: " + x + "Y: " + -y);

                        string stringData = x + "," + -y;

                        //byte [] sendBytes = Encoding.ASCII.GetBytes(stringData);
                        //client.Send(sendBytes, sendBytes.Length);

                        Packet MyPack = new Packet(0x30, false);
                        MyPack.AppendData(UtilData.ToBytes(stringData));
                        MyPack.IsUDP = false;
                        Client.Send(MyPack);
                    }
                }
            } 
            else
            { 
                int count = 0;
                while (true)
                {

                    GamePadState State = GamePad.GetState(0);
                    if (State.Buttons.A == ButtonState.Pressed)
                    {
                        do
                        {
                            //Thread.Sleep(500);
                            count++;
                            State = GamePad.GetState(0);
                            //Console.WriteLine("Reading");
                            if (State.IsConnected)
                            {
                                float rightSpeed = State.Triggers.Right;
                                float leftSpeed = State.Triggers.Left;
                                float speed = rightSpeed - leftSpeed;

                                float leftJoy = State.ThumbSticks.Left.Y;
                                float rightJoy = State.ThumbSticks.Right.X + 0.003967406f;


                                //Console.WriteLine("Left Trigger: " + leftSpeed);
                                //Console.WriteLine("Right Trigger: " + rightSpeed);
                                float turn = rightSpeed - leftSpeed;

                                if (count == 100)
                                {
                                    Console.WriteLine($"Speed: {rightJoy}");
                                    Console.WriteLine($"Turn: {turn}");
                                    Console.WriteLine();
                                    count = 0;
                                }

                                string stringData = turn + "," + rightJoy;

                                //byte [] sendBytes = Encoding.ASCII.GetBytes(stringData);
                                //client.Send(sendBytes, sendBytes.Length);


                                Console.WriteLine(stringData);


                                Packet MyPack = new Packet(0x30, false);
                                MyPack.AppendData(UtilData.ToBytes(stringData));
                                MyPack.IsUDP = false;
                                Client.Send(MyPack);




                                /*
                                canName.Write(5, UtilData.ToBytes((int)0.15*turn*100000.0)); 
                             
                                canName.Write(1, UtilData.ToBytes((int)rightJoy * 100000.0));
                                canName.Write(2, UtilData.ToBytes((int)rightJoy * 100000.0));
                                canName.Write(3, UtilData.ToBytes((int)rightJoy * 100000.0));
                                canName.Write(4, UtilData.ToBytes((int)rightJoy * 100000.0));
                                */
                            }
                            else
                            {
                                Console.WriteLine("NOT CONNECTED");
                            }
                        }
                        while (State.Buttons.Start != ButtonState.Pressed && State.Buttons.B != ButtonState.Pressed);
                    }
                }
            }
        }
    }
}