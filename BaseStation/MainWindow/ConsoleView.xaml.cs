using System;
using System.IO;
using System.Text;
using System.Windows.Controls;

namespace HuskyRobotics.UI
{
    /// <summary>
    /// Interaction logic for ConsoleView.xaml
    /// </summary>
    public partial class ConsoleView : ScrollViewer
    {
        public ConsoleView()
        {
            TextBlock child = new TextBlock();
            AddChild(child);
            child.FontFamily = new System.Windows.Media.FontFamily("consolas");
            child.FontSize = 16;
            Console.SetOut(new ConsoleWriter(child, this, Console.Out));
            InitializeComponent();
        }

        private class ConsoleWriter : TextWriter
        {
            private readonly TextBlock view;
            private readonly ScrollViewer scroll;
            private readonly TextWriter oldOut;

            public ConsoleWriter(TextBlock view, ScrollViewer scroll, TextWriter oldOut)
            {
                this.view = view;
                this.scroll = scroll;
                this.oldOut = oldOut;
                view.TextWrapping = System.Windows.TextWrapping.Wrap;
            }

            public override Encoding Encoding => Encoding.UTF8;

            public override void Write(char value)
            {
                oldOut.Write(value);
                view.Dispatcher.BeginInvoke(new Action(() =>
                {
                    view.Text += value;
                    UpdateView();
                }));
            }

            public override void Write(char[] buffer, int index, int count)
            {
                oldOut.Write(buffer, index, count);
                view.Dispatcher.BeginInvoke(new Action(() =>
                {
                    view.Text += new string(buffer, index, count);
                    UpdateView();
                }));
            }

            //removes extra text (prevent memory leak)
            //and scrolls view to bottom (like a console)
            private void UpdateView()
            {
                int len = view.Text.Length;
                double scrollHeight = scroll.ViewportHeight;
                double viewHeight = view.ActualHeight;
                int totalLines = CountLines(view.Text);
                int lineHeight = (int)viewHeight / totalLines;
                int possibleVisibleLines = (int)(scrollHeight / lineHeight);
                if (viewHeight > scrollHeight * 2) //if stored text is partially offscreen, the 2 is there to buffer the text so there isn't so much garbage produced
                {
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
                while (-1 != (index = str.IndexOf(Environment.NewLine, index + Environment.NewLine.Length)))
                {
                    count++;
                }
                return count + 1;
            }
        }
    }
}
