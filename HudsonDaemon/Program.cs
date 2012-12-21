using System;
using System.Windows.Forms;
using HudsonIndicator.HudsonDaemon.UI;

namespace HudsonIndicator.HudsonDaemon
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}

