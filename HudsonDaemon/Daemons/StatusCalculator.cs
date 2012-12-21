using System.Collections.Generic;
using System.Linq;
using HudsonIndicator.HudsonDaemon.Hudson;

namespace HudsonIndicator.HudsonDaemon.Daemons
{
    public class StatusCalculator
    {
        public Status Calculate(IEnumerable<JobDetail> jobDetails)
        {
            if (jobDetails.Any(detail => detail.color == HudsonColor.Red))
            {
                return Status.Red;
            }
            if (jobDetails.Any(detail => detail.color == HudsonColor.Yellow))
            {
                return Status.Yellow;
            }
            if (jobDetails.Any(detail => detail.color.EndsWith(HudsonColor.Anime)))
            {
                return Status.BlueAnime;
            }
            return Status.Green;
        }
    }
}

