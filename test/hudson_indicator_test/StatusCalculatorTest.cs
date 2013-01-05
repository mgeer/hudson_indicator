using System.Linq;
using HudsonIndicator.HudsonDaemon.Daemons;
using HudsonIndicator.HudsonDaemon.Hudson;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace hudson_indicator_test
{
    [TestClass]
    public class StatusCalculatorTest
    {
        [TestMethod]
        public void it_should_be_red_if_any_job_is_red()
        {
            Assert.AreEqual(Status.Red, CalculateColor(HudsonColor.Red, HudsonColor.Blue));
            Assert.AreEqual(Status.Red, CalculateColor(HudsonColor.Anime, HudsonColor.Red, HudsonColor.Blue));
        }
        
        [TestMethod]
        public void it_should_be_yellow_if_any_job_is_yellow_without_red()
        {
            Assert.AreEqual(Status.Yellow, CalculateColor(HudsonColor.Yellow, HudsonColor.Blue));
            Assert.AreEqual(Status.Yellow, CalculateColor(HudsonColor.YellowAnime, HudsonColor.Yellow, HudsonColor.Blue));
        }

        [TestMethod]
        public void it_should_be_blueanime_if_any_is_in_anime_and_no_red_either_yellow()
        {
            Assert.AreEqual(Status.BlueAnime, CalculateColor(HudsonColor.Blue, HudsonColor.BlueAnime, HudsonColor.YellowAnime));
        }

        private static Status CalculateColor(params string[] colors)
        {
            var calculator = new StatusCalculator();
            var jobDetails = from color in colors select new JobDetail {color = color};
            var status = calculator.Calculate(jobDetails);
            return status;
        }
    }
}