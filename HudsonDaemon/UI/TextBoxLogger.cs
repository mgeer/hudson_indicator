using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HudsonIndicator.HudsonDaemon.Log;
using System.Linq;

namespace HudsonIndicator.HudsonDaemon.UI
{
    public class TextBoxLogger : ILogger
    {
        private readonly TextBox textBox;

        public TextBoxLogger(TextBox textBox)
        {
            this.textBox = textBox;
        }

        public void Show(string message)
        {
            if (textBox.InvokeRequired)
            {
                var d = new SetTextCallback(SetText);
                textBox.Invoke(d, message);
            }
            else
            {
                SetText(message);
            }
            
        }

        private void SetText(string text)
        {
            var hundredLinesText = GetLast100Lines(textBox.Lines);
            textBox.Text = hundredLinesText + Environment.NewLine + text;
            textBox.SelectionStart = textBox.Text.Length;
            textBox.ScrollToCaret();
        }

        private string GetLast100Lines(string[] lines)
        {
            const int limit = 100;
            var countToSkip = lines.Length - limit;
            IEnumerable<string> linesTaken = lines;
            if (countToSkip > 0)
            {
                linesTaken = lines.Skip(countToSkip).Take(lines.Length - countToSkip);
            }
            return String.Join(Environment.NewLine, linesTaken);
        }
    }

    internal delegate void SetTextCallback(string text);
}