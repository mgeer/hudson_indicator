using System.Linq;

namespace HudsonDaemon.Daemons
{
    using Hudson;
    using System.Collections.Generic;
    using System.Threading;

    internal class Schedule
    {
        private const int INTERVAL_IN_SECOND = 30;
        private Timer timer;

        public void Refresh(IEnumerable<JobItem> items)
        {
            if (this.timer != null)
            {
                this.timer.Dispose();
            }
            this.timer = new Timer(new TimerCallback(Schedule.UpdateDaemonStatus), items, 0, 0x7530);
        }

        private static void UpdateDaemonStatus(object state)
        {
            var enumerable = (IEnumerable<JobItem>) state;
            var jobDetails = from item in enumerable select HudsonApiGraber.GetJobDetail(item.url);
            new Daemon().UpdateStatus(jobDetails);
        }
    }
}

