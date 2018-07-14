using System;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace HuskyRobotics.UI
{
    /// <summary>
    /// Interaction logic for ConsoleView.xaml
    /// </summary>
    public partial class ConsoleView : ScrollViewer {
        private readonly ConsoleWriter writer;

        public TextWriter Writer => writer;

        public ConsoleView() {
            TextBox box = new TextBox {
                FontFamily = new FontFamily("consolas"),
                FontSize = 16,
                IsReadOnly = true,
                IsReadOnlyCaretVisible = false,
                BorderThickness = new System.Windows.Thickness(0),
                TextWrapping = System.Windows.TextWrapping.Wrap
            };

            AddChild(box);
            writer = new ConsoleWriter(box, this);
            InitializeComponent();
        }

		private class ConsoleWriter : TextWriter {
			private readonly TextBox view;
			private readonly ScrollViewer scroll;

			public ConsoleWriter(TextBox view, ScrollViewer scroll) {
				this.view = view;
				this.scroll = scroll;
			}

            public override Encoding Encoding => Encoding.UTF8;

            public override void Write(char value)
            {
                view.Dispatcher.InvokeAsync(() => {
                    view.AppendText(value.ToString());
                    UpdateView();
                });
            }

            public override void Write(char[] buffer, int index, int count)
            {
                view.Dispatcher.InvokeAsync(() => {
                    view.AppendText(new string(buffer, index, count));
                    UpdateView();
                });
            }

            //removes extra text (prevent memory leak)
            //and scrolls view to bottom (like a console)
            private void UpdateView() {
				int len = view.Text.Length;
                double scrollHeight = scroll.ViewportHeight;
                double viewHeight = view.ActualHeight;
                int totalLines = CountLines(view.Text);
                int lineHeight = (int)viewHeight / totalLines;
                int possibleVisibleLines = (int)(scrollHeight / lineHeight);
                if (viewHeight > scrollHeight) {
                    view.Text = GetFinalLines(view.Text, totalLines, Math.Min(totalLines, possibleVisibleLines));
                }
				scroll.ScrollToEnd();
			}

            private static string GetFinalLines(string text, int totalLines, int lineCount)
            {
                int ind = GetIndexAfter(text, totalLines - lineCount);
                return text.Substring(ind);
            }

            private static int GetIndexAfter(string str, int count)
            {
                if (str == string.Empty) return -1;
                int index = -1;
                while (count > 0 && (index = str.IndexOf(Environment.NewLine, index + Environment.NewLine.Length)) != -1)
                {
                    count--;
                }
                return index + Environment.NewLine.Length;
            }

            private static int CountLines(string str)
            {
                if (str == string.Empty) return 0;
                int index = -1;
                int count = 0;
                while (-1 != (index = str.IndexOf(Environment.NewLine, index + Environment.NewLine.Length))) {
					count++;
				}
                return count + 1;
            }
        }
	}
}
