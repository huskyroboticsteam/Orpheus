using System;
using System.Windows.Controls;
using System.Windows.Media;
using SharpDX.XInput;
using Buttons = SharpDX.XInput.GamepadButtonFlags;

namespace HuskyRobotics.UI {
	/// <summary>
	/// Interaction logic for GamePadView.xaml
	/// </summary>
	public partial class GamePadView : UserControl {
		public Controller Controller {
			get;
			set;
		}

		private readonly SolidColorBrush lit = new SolidColorBrush(Colors.Gold);
		private readonly SolidColorBrush unLit = new SolidColorBrush(Colors.Black);
		private readonly SolidColorBrush backGround = new SolidColorBrush(Colors.LightGray);
		private readonly SolidColorBrush bigBackGround = new SolidColorBrush(Colors.DarkGray);
		private readonly SolidColorBrush altLit = new SolidColorBrush(Colors.DarkOrange);
		private readonly SolidColorBrush lowBattery = new SolidColorBrush(Colors.Red);
		private readonly SolidColorBrush highBattery = new SolidColorBrush(Colors.Green);

		public GamePadView() {
			InitializeComponent();
			CompositionTarget.Rendering += UpdateGamePadView;
		}

		public void UpdateGamePadView(object sender, EventArgs args) {
			if (Controller != null && Controller.IsConnected) {
				Gamepad gamepad = Controller.GetState().Gamepad;
				GamepadButtonFlags buttons = gamepad.Buttons;

				// Skittle Buttons
				A.Foreground = IsPressed(buttons, Buttons.A) ? lit : unLit;
				B.Foreground = IsPressed(buttons, Buttons.B) ? lit : unLit;
				X.Foreground = IsPressed(buttons, Buttons.X) ? lit : unLit;
				Y.Foreground = IsPressed(buttons, Buttons.Y) ? altLit : unLit;

				// D-Pad
				Up.Fill = IsPressed(buttons, Buttons.DPadUp) ? lit : backGround;
				Down.Fill = IsPressed(buttons, Buttons.DPadDown) ? lit : backGround;
				Left.Fill = IsPressed(buttons, Buttons.DPadLeft) ? lit : backGround;
				Right.Fill = IsPressed(buttons, Buttons.DPadRight) ? lit : backGround;

				// Center Buttons
				Back.Fill = IsPressed(buttons, Buttons.Back) ? lit : unLit;
				Start.Fill = IsPressed(buttons, Buttons.Start) ? lit : unLit;

				// Bumpers
				LBump.Fill = IsPressed(buttons, Buttons.LeftShoulder) ? lit : unLit;
				RBump.Fill = IsPressed(buttons, Buttons.RightShoulder) ? lit : unLit;

				// Thumbsticks
				LeftThumb.Fill = IsPressed(buttons, Buttons.LeftThumb) ? lit : unLit;
				RightThumb.Fill = IsPressed(buttons, Buttons.RightThumb) ? lit : unLit;

				//use the middle big button as a battery indicator (gren for high/med, red for low)
				BatteryLevel battery = Controller.GetBatteryInformation(BatteryDeviceType.Gamepad).BatteryLevel;
				BatteryIndicator.Fill = battery == BatteryLevel.Full || battery == BatteryLevel.Full ? highBattery : lowBattery;

				// Triggers TODO adjust threshholds/range
				LTrig.Width = gamepad.LeftTrigger / 10;
				RTrig.Width = gamepad.RightTrigger / 10;

				//Thumbstick positions
				LeftThumb.RenderTransform = new TranslateTransform(gamepad.LeftThumbX, gamepad.LeftThumbY);
				//TranslateTransform moveL = new TranslateTransform
				//    ((5 * GPState.ThumbSticks.Left.X) + 38, - (5 * GPState.ThumbSticks.Left.Y) + 46);
				RightThumb.RenderTransform = new TranslateTransform(gamepad.RightThumbX, gamepad.RightThumbY);
				//TranslateTransform moveR = new TranslateTransform
				//    ((5 * GPState.ThumbSticks.Right.X) + 98, -(5 * GPState.ThumbSticks.Right.Y) + 46);
			}
		}

		private static bool IsPressed(GamepadButtonFlags flags, GamepadButtonFlags button) {
			return (flags & button) != 0;
		}
	}
}
