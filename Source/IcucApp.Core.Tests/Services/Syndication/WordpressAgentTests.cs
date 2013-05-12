using System;
using IcucApp.Services.Facebook;
using IcucApp.Services.Syndication;
using NUnit.Framework;

namespace IcucApp.Core.Tests.Services.Syndication
{
    [TestFixture]
    public class WordpressAgentTests
    {
        [Test]
        public void TestWeCanGetFeeds()
        {
            // http://www.katriendanschutter.be/feed/?cat=lineup

            var agent = new WordpressFeedAgent("http://www.katriendanschutter.be");
            var entries = agent.GetFeeds("lineup");
            Assert.GreaterOrEqual(entries.Entries.Count, 0);
        }
    }
}