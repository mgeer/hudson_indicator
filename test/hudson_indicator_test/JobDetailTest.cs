using HudsonIndicator.HudsonDaemon.Hudson;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace hudson_indicator_test
{
    [TestClass]
    public class JobDetailTest
    {
        [TestMethod]
        public void it_should_parse_color_and_description_from_json()
        {
            var jobDetail = JobDetail.Parse(BytesToText.Convert(Resource.job_detail_in_json));
            Assert.AreEqual("blueColorForTest", jobDetail.color);
            Assert.AreEqual("00_QUICK_TRIGGER_DESCRIPTION_FOR_TEST", jobDetail.description);
        }
    }
}