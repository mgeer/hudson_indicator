using System;
using System.Collections.Generic;
using System.Diagnostics;
using HudsonIndicator.HudsonDaemon.Hudson;
using HudsonIndicator.HudsonDaemon.Log;
using Timer = System.Threading.Timer;
using System.Linq;

namespace HudsonIndicator.HudsonDaemon.Daemons
{
    internal class Schedule
    {
        private readonly ILogger logger;
        private HudsonApiGraber hudsonApiGraber;
        private const int IntervalInSecond = 10 * 1000;
        private Timer timer;

        public Schedule(ILogger logger)
        {
            this.logger = logger;
        }

        public void Refresh(IEnumerable<JobItem> items, HudsonApiGraber graber)
        {
            hudsonApiGraber = graber;
            ResetTimer(items);
        }

        private void ResetTimer(IEnumerable<JobItem> items)
        {
            if (timer != null)
            {
                timer.Dispose();
            }
            timer = new Timer(UpdateDaemonStatus, items, 0, IntervalInSecond);
        }

        private void UpdateDaemonStatus(object state)
        {
            var jobItems = (IEnumerable<JobItem>) state;
            //debug
            Debug.WriteLine("job count: " + jobItems.Count());
            foreach (var jobItem in jobItems)
            {
                Debug.WriteLine(jobItem.url);
            }
            //end
            try
            {
                var jobDetails = new List<JobDetail>();
                foreach (var jobItem in jobItems)
                {
                    logger.Show("Trying to get job detail for: " + jobItem.url);
                    jobDetails.Add(hudsonApiGraber.GetJobDetail(jobItem.url));
                }
                new Daemon().UpdateStatus(jobDetails);
            }
            catch (Exception e)
            {
                logger.Show(e.Message);
            }
        }
    }
}

