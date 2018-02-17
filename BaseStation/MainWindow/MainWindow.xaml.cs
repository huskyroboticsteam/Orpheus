using System.Windows;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;

namespace HuskyRobotics.UI {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
			WindowState = WindowState.Maximized;
			DataContext = this;
		}

		private void PuTTY_Button_Click(object sender, RoutedEventArgs e) {
			string puttyPath = @"C:\Program Files (x86)\PuTTY\putty.exe";
			if (File.Exists(puttyPath)) {
				var process = new Process();
				process.StartInfo.FileName = puttyPath;
				process.StartInfo.Arguments = "-ssh root@192.168.0.50";
				process.Start();
			} else {
				//remove button if PuTTY is not found
				DockPanel panel = this.FindName("dock") as DockPanel;
				panel.Children.Remove(sender as UIElement);
				//display error message
				MessageBox.Show("Could not find PuTTY. You will need to install putty, or launch it manually\n" +
						"Looking at: " + puttyPath);
			}
		}
    }
}
