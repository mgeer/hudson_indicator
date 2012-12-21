using System.Linq;
using System.Collections.Generic;
using System.Threading;
using HudsonIndicator.HudsonDaemon.Hudson;

namespace HudsonIndicator.HudsonDaemon.Daemons
{
    internal class Schedule
    {
        private const int IntervalInSecond = 30;
        private Timer timer;

        public void Refresh(IEnumerable<JobItem> items)
        {
            if (timer != null)
            {
                timer.Dispose();
            }
            timer = new Timer(UpdateDaemonStatus, items, 0, IntervalInSecond);
        }

        private static void UpdateDaemonStatus(object state)
        {
            var enumerable = (IEnumerable<JobItem>) state;
            var jobDetails = from item in enumerable select HudsonApiGraber.GetJobDetail(item.url);
            new Daemon().UpdateStatus(jobDetails);
        }
    }
}

