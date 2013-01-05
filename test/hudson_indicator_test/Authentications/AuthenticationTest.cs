using HudsonIndicator.HudsonDaemon.Hudson;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace hudson_indicator_test.Authentications
{
    [TestClass]
    public class AuthenticationTest
    {
        [TestMethod]
        public void it_should_get_jobs_with_authentication()
        {
            var hudsonUrl = new HudsonUrl("http://classified.jenkins.baidu.com/view/DA-RANK/");
            var hudsonApiGraber = new HudsonApiGraber(new CredentialSpider("zuoyulong", "123456"));
            var jobItems = hudsonApiGraber.GetJobs(new[] {hudsonUrl});
            Assert.AreEqual(12, jobItems.Count());
        }

        [TestMethod]
        public void it_should_get_jobs_without_authentication()
        {
            var hudsonUrl = new HudsonUrl("http://classified.jenkins.baidu.com/view/DA-RANK/");
            var hudsonApiGraber = new HudsonApiGraber(new CommonSpider());
            var jobItems = hudsonApiGraber.GetJobs(new[] {hudsonUrl});
            Assert.AreEqual(2, jobItems.Count());
        }

    }
}