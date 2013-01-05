using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using HudsonIndicator.HudsonDaemon.Hudson;
using Timer = System.Threading.Timer;

namespace HudsonIndicator.HudsonDaemon.Daemons
{
    internal class Schedule
    {
        private HudsonApiGraber hudsonApiGraber;
        private const int IntervalInSecond = 30;
        private Timer timer;

        public void Refresh(IEnumerable<JobItem> items, HudsonApiGraber graber)
        {
            hudsonApiGraber = graber;
            if (timer != null)
            {
                timer.Dispose();
            }
            timer = new Timer(UpdateDaemonStatus, items, 0, IntervalInSecond);
        }

        private void UpdateDaemonStatus(object state)
        {
            try
            {
                var enumerable = (IEnumerable<JobItem>)state;
                var jobDetails = from item in enumerable select hudsonApiGraber.GetJobDetail(item.url);
                new Daemon().UpdateStatus(jobDetails);
            }
            catch (Exception e)
            {
                timer.Dispose();
                MessageBox.Show(e.Message);
            }
            
        }
    }
}

