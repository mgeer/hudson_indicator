using X3.Spider;
using System.Collections.Generic;

namespace HudsonIndicator.HudsonDaemon.Hudson
{
    public class HudsonApiGraber
    {
        public static JobDetail GetJobDetail(string url)
        {
            var url2 = new HudsonUrl(url);
            var spider = new Spider();
            var jobDetailInJson = spider.Grab(url2.GetJSONAPI());
            return JobDetail.Parse(jobDetailInJson);
        }

        public static IEnumerable<JobItem> GetJobs(IEnumerable<HudsonUrl> hudsonUrls)
        {
            var spider = new Spider();
            var allJobItems = new List<JobItem>();
            foreach (var hudsonUrl in hudsonUrls)
            {
                var jobsInJson = spider.Grab(hudsonUrl.GetJSONAPI());
                var items = JobItems.Parse(jobsInJson);
                var jobItems = items.jobs;
                allJobItems.AddRange(jobItems);
            }
            return allJobItems;
//            return (from hudsonUrl in hudsonUrls select JobItems.Parse(spider.Grab(hudsonUrl.GetJSONAPI())).jobs);
        }
    }
}

