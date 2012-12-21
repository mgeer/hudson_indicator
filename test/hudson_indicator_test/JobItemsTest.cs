using HudsonIndicator.HudsonDaemon.Hudson;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace hudson_indicator_test
{
    [TestClass]
    public class JobItemsTest
    {
        [TestMethod]
        public void it_should_parse_description_for_jobs_in_json()
        {
            var jobItems = ParseJobItems();
            Assert.AreEqual(null, jobItems.description);
        }

        [TestMethod]
        public void it_should_parse_job_items_array_for_jobs_in_json()
        {
            var jobItems = ParseJobItems();
            Assert.AreEqual(14, jobItems.jobs.Length);
        }

        [TestMethod]
        public void it_should_parse_name_and_url_for_job_item()
        {
            var jobItems = ParseJobItems();
            Assert.AreEqual("00_QUICK_TRIGGER", jobItems.jobs[0].name);
            Assert.AreEqual("http://db-testing-ps2122.db01.baidu.com:8235/hudson/job/00_QUICK_TRIGGER/", jobItems.jobs[0].url);
        }

        private static JobItems ParseJobItems()
        {
            return JobItems.Parse(BytesToText.Convert(Resource.jobs_in_json));
        }
    }
}
