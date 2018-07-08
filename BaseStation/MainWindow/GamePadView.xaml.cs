using System;
using System.Windows.Controls;
using System.Windows.Media;
using OpenTK.Input;

namespace HuskyRobotics.UI
{
    /// <summary>
    /// Interaction logic for GamePadView.xaml
    /// </summary>
    public partial class GamePadView : UserControl
    {
        private GamePadState _gPState;
        public GamePadState GPState {
            get => _gPState;
            set {
                _gPState = value;
            }
        }

        private readonly SolidColorBrush lit = new SolidColorBrush(Colors.Gold);
        private readonly SolidColorBrush unLit = new SolidColorBrush(Colors.Black);
        private readonly SolidColorBrush backGround = new SolidColorBrush(Colors.LightGray);
        private readonly SolidColorBrush bigBackGround = new SolidColorBrush(Colors.DarkGray);
        private readonly SolidColorBrush altLit = new SolidColorBrush(Colors.DarkOrange);

        public GamePadView()
        {
            InitializeComponent();
            CompositionTarget.Rendering += UpdateGamePadView;
        }

        public void UpdateGamePadView(object sender, EventArgs args)
        {
            // Skittle Buttons
            A.Foreground = (GPState.Buttons.A == ButtonState.Pressed) ? lit : unLit;
            B.Foreground = (GPState.Buttons.B == ButtonState.Pressed) ? lit : unLit;
            X.Foreground = (GPState.Buttons.X == ButtonState.Pressed) ? lit : unLit;
            Y.Foreground = (GPState.Buttons.Y == ButtonState.Pressed) ? altLit : unLit;
            
            // D-Pad
            Up.Fill = (GPState.DPad.Up == ButtonState.Pressed) ? lit : backGround;
            Down.Fill = (GPState.DPad.Down == ButtonState.Pressed) ? lit : backGround;
            Left.Fill = (GPState.DPad.Left == ButtonState.Pressed) ? lit : backGround;
            Right.Fill = (GPState.DPad.Right == ButtonState.Pressed) ? lit : backGround;

            // Center Buttons
            Back.Fill = (GPState.Buttons.Back == ButtonState.Pressed) ? lit : unLit;
            Big.Fill = (GPState.Buttons.BigButton == ButtonState.Pressed) ? lit : unLit;
            Start.Fill = (GPState.Buttons.Start == ButtonState.Pressed) ? lit : unLit;

            // Bumpers
            LBump.Fill = (GPState.Buttons.LeftShoulder == ButtonState.Pressed) ? lit : unLit;
            RBump.Fill = (GPState.Buttons.RightShoulder == ButtonState.Pressed) ? lit : unLit;

            // Thumbsticks
            LeftThumb.Fill = (GPState.Buttons.LeftStick == ButtonState.Pressed) ? lit : unLit;
            RightThumb.Fill = (GPState.Buttons.RightStick == ButtonState.Pressed) ? lit : unLit;

            // Triggers
            LTrig.Width = (int)(5 - (4 * GPState.Triggers.Left));
            RTrig.Width = (int)(5 - (4 * GPState.Triggers.Right));

            //Thumbstick positions
            TranslateTransform moveL = new TranslateTransform
                ((5 * GPState.ThumbSticks.Left.X) + 38, - (5 * GPState.ThumbSticks.Left.Y) + 46);
            LeftThumb.RenderTransform = moveL;
            TranslateTransform moveR = new TranslateTransform
                ((5 * GPState.ThumbSticks.Right.X) + 98, -(5 * GPState.ThumbSticks.Right.Y) + 46);
            RightThumb.RenderTransform = moveR;
        }
    }
}
