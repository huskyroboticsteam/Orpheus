using System;
using OpenTK.Input;
namespace FlatRobot
{
    public class opentk
    {
        static bool ReceivingInput(GamePadState State)
        {
            return State.Triggers.Left <= Double.Epsilon && State.Triggers.Right <= Double.Epsilon;
        }

        public static void Main(string[] args) {

            GamePadState State;
            do
            {
                Console.WriteLine("Reading!");
                State = GamePad.GetState(0);
                if (State.IsConnected)
                {
                    Console.WriteLine($"Left: {State.Triggers.Left}, Right: {State.Triggers.Right}");
                    Console.WriteLine($"Left Joystick X: {State.ThumbSticks.Left.X}, Y: {State.ThumbSticks.Left.Y}, Length: {State.ThumbSticks.Left.Length}");
                }
                else
                {
                    Console.WriteLine("NOT CONNECTED");
                }





            } while (State.Buttons.Start != ButtonState.Pressed);

        }
        public opentk()
        {
            
        }
    }
}
