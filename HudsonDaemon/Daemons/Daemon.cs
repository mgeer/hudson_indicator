using HudsonDaemon.Hudson;

namespace HudsonDaemon.Daemons
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    internal class Daemon
    {
        private static void ExecuteCmd(Status status)
        {
            var process = new Process {
                StartInfo =
                    {
                        CreateNoWindow = true, 
                        UseShellExecute = false, 
                        RedirectStandardOutput = false, 
                        FileName = "USBCMDAP.exe", 
                        Arguments = string.Format(" 0 0 102 10 1 {0}", (int) status)
                    }
            };
            process.Start();
            process.WaitForExit();
        }

        public void UpdateStatus(IEnumerable<JobDetail> jobDetails)
        {
            Status status = new StatusCalculator().Calculate(jobDetails);
            try
            {
                ExecuteCmd(status);
            }
            catch (Exception exception)
            {
                WriteLog(exception.Message);
            }
        }

        private void WriteLog(string message)
        {
        }
    }
}

