using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

        private SolidColorBrush lit = new SolidColorBrush(Colors.Gold);
        private SolidColorBrush unLit = new SolidColorBrush(Colors.Black);
        private SolidColorBrush backGround = new SolidColorBrush(Colors.LightGray);
        private SolidColorBrush bigBackGround = new SolidColorBrush(Colors.DarkGray);
        private SolidColorBrush altLit = new SolidColorBrush(Colors.DarkOrange);

        public GamePadView()
        {
            InitializeComponent();
            Task.Run(new Action(UpdateGamePadState));
        }
        private void UpdateGamePadState()
        {
            while(true)
            {
                GPState = GamePad.GetState(0);
                Dispatcher.InvokeAsync(UpdateGamePadView);
                Thread.Sleep(50);
            }
        }

        private void UpdateGamePadView()
        {
            // Skittle Buttons
            if (GPState.Buttons.A == ButtonState.Pressed) A.Foreground = lit;
            else A.Foreground = unLit;
            if (GPState.Buttons.B == ButtonState.Pressed) B.Foreground = lit;
            else B.Foreground = unLit;
            if (GPState.Buttons.X == ButtonState.Pressed) X.Foreground = lit;
            else X.Foreground = unLit;
            if (GPState.Buttons.Y == ButtonState.Pressed) Y.Foreground = altLit;
            else Y.Foreground = unLit;
            // D-Pad
            if (GPState.DPad.Up == ButtonState.Pressed) Up.Fill = lit;
            else Up.Fill = backGround;
            if (GPState.DPad.Down == ButtonState.Pressed) Down.Fill = lit;
            else Down.Fill = backGround;
            if (GPState.DPad.Left == ButtonState.Pressed) Left.Fill = lit;
            else Left.Fill = backGround;
            if (GPState.DPad.Right == ButtonState.Pressed) Right.Fill = lit;
            else Right.Fill = backGround;
            // Bumpers
            if (GPState.Buttons.LeftShoulder == ButtonState.Pressed) LBump.Fill = lit;
            else LBump.Fill = unLit;
            if (GPState.Buttons.RightShoulder == ButtonState.Pressed) RBump.Fill = lit;
            else RBump.Fill = unLit;
            // Center Buttons
            if (GPState.Buttons.Back == ButtonState.Pressed) Back.Fill = lit;
            else Back.Fill = unLit;
            if (GPState.Buttons.BigButton == ButtonState.Pressed) Big.Fill = lit;
            else Big.Fill = bigBackGround;
            if (GPState.Buttons.Start == ButtonState.Pressed) Start.Fill = lit;
            else Start.Fill = unLit;
            // Thumbsticks
            if (GPState.Buttons.LeftStick == ButtonState.Pressed) LeftThumb.Fill = lit;
            else LeftThumb.Fill = unLit;
            if (GPState.Buttons.RightStick == ButtonState.Pressed) RightThumb.Fill = lit;
            else RightThumb.Fill = unLit;
            TranslateTransform moveL = new TranslateTransform
                ((5 * GPState.ThumbSticks.Left.X) + 38, - (5 * GPState.ThumbSticks.Left.Y) + 46);
            LeftThumb.RenderTransform = moveL;
            TranslateTransform moveR = new TranslateTransform
                ((5 * GPState.ThumbSticks.Right.X) + 98, -(5 * GPState.ThumbSticks.Right.Y) + 46);
            RightThumb.RenderTransform = moveR;
            // Triggers
            LTrig.Width = (int)(5 - (4 * GPState.Triggers.Left));
            RTrig.Width = (int)(5 - (4 * GPState.Triggers.Right));
        }
    }
}
