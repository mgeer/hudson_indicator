using X3.Spider;
using System.Collections.Generic;

namespace HudsonIndicator.HudsonDaemon.Hudson
{
    public interface ISpider
    {
        string Spide(string url);
    }

    public class CommonSpider : ISpider
    {
        public string Spide(string url)
        {
            var spider = new Spider();
            return spider.Grab(url);
        }
    }

    public class CredentialSpider : ISpider
    {
        private readonly string userName;
        private readonly string password;

        public CredentialSpider(string userName, string password)
        {
            this.userName = userName;
            this.password = password;
        }

        public string Spide(string url)
        {
            var spider = new Spider();
            return spider.GrapWithCredentials(url, userName, password);
        }
    }

    public class HudsonApiGraber
    {
        private readonly ISpider spider;

        public HudsonApiGraber(ISpider spider)
        {
            this.spider = spider;
        }

        public JobDetail GetJobDetail(string url)
        {
            var url2 = new HudsonUrl(url);
            var jobDetailInJson = spider.Spide(url2.GetJSONAPI());
            return JobDetail.Parse(jobDetailInJson);
        }

        public IEnumerable<JobItem> GetJobs(IEnumerable<HudsonUrl> hudsonUrls)
        {
            var allJobItems = new List<JobItem>();
            foreach (var hudsonUrl in hudsonUrls)
            {
                var jobsInJson = spider.Spide(hudsonUrl.GetJSONAPI());
                var items = JobItems.Parse(jobsInJson);
                var jobItems = items.jobs;
                allJobItems.AddRange(jobItems);
            }
            return allJobItems;
        }
    }
}

