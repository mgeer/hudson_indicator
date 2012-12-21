namespace HudsonDaemon.Daemons
{
    using Hudson;
    using System.Collections.Generic;
    using System.Linq;

    public class StatusCalculator
    {
        public Status Calculate(IEnumerable<JobDetail> jobDetails)
        {
            if (jobDetails.Any(detail => detail.color == "red"))
            {
                return Status.Red;
            }
            if (jobDetails.Any(detail => detail.color == "yellow"))
            {
                return Status.Yellow;
            }
            if (jobDetails.Any(detail => detail.color.EndsWith("anime")))
            {
                return Status.BlueAnime;
            }
            return Status.Green;
        }
    }
}

