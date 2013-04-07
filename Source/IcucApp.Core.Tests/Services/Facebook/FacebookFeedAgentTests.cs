using IcucApp.Core.Services.Facebook;
using NUnit.Framework;

namespace IcucApp.Core.Tests.Services.Facebook
{
    [TestFixture]
    public class FacebookFeedAgentTests
    {
        [Test]
        public void TestWeCanGetFeeds()
        {
            const string url = "https://www.facebook.com/feeds/page.php";
            var agent = new FacebookFeedAgent(url);
            var message = agent.GetFeeds("441615792534282");
            Assert.Greater(message.entries.Count, 1);
        }
    }
}