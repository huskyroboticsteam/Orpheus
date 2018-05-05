using System;
using System.IO;
using System.Text;
using System.Windows.Controls;

namespace HuskyRobotics.UI
{
    /// <summary>
    /// Interaction logic for ConsoleView.xaml
    /// </summary>
    public partial class ConsoleView : ScrollViewer {
		public ConsoleView() {
			TextBlock child = new TextBlock();
			VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
			AddChild(child);
			child.FontFamily = new System.Windows.Media.FontFamily("consolas");
			child.FontSize = 16;
			Console.SetOut(new ConsoleWriter(child, this, Console.Out));
			InitializeComponent();
        }

		private class ConsoleWriter : TextWriter {
			private const int BUFFER_SIZE = 500;

			private readonly TextBlock view;
			private readonly ScrollViewer scroll;
            private readonly TextWriter oldOut;

			public ConsoleWriter(TextBlock view, ScrollViewer scroll, TextWriter oldOut) {
				this.view = view;
				this.scroll = scroll;
                this.oldOut = oldOut;
			}

 			public override Encoding Encoding => Encoding.UTF8;

			public override void Write(char value) {
                oldOut.Write(value);
				view.Dispatcher.BeginInvoke(new Action(() => {
					view.Text += value;
					UpdateView();
				}));
			}

			public override void Write(string value)
            {
                oldOut.Write(value);
                view.Dispatcher.BeginInvoke(new Action(() => {
					view.Text += value;
					UpdateView();
				}));
			}

			//removes extra text (prevent memory leak)
			//and scrolls view to bottom (like a console)
			private void UpdateView() {
				int len = view.Text.Length;
				int removeSize = Math.Max(0, Math.Min(len - BUFFER_SIZE, BUFFER_SIZE));
				view.Text =
					view.Text.Substring(removeSize, len - removeSize);
				scroll.ScrollToEnd();
			}
		}
	}
}
