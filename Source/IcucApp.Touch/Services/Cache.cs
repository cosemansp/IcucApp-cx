using System;
using IcucApp.Services.Facebook;
using IcucApp.Core;
using IcucApp.Services.Syndication;

namespace IcucApp
{
    public interface ICache {
        void SetFacebook(RequestContext<FacebookMessage> context);
        void SetWebsite(RequestContext<WordpressMessage> context);
        void SetLineup(string category, RequestContext<WordpressMessage> context);
        void SetInfo(RequestContext<WordpressEntry> context);
    
        RequestContext<FacebookMessage> GetFacebookFeed();
        RequestContext<WordpressMessage> GetWebsiteFeed();
        RequestContext<WordpressMessage> GetLineupFeed(string category);
        RequestContext<WordpressEntry> GetInfoFeed();
    }

    public class Cache : ICache
    {
        public void SetFacebook(RequestContext<FacebookMessage> context) {
            CacheStore.Set("facebookFeeds", context);
        }

        public void SetWebsite(RequestContext<WordpressMessage> context) {
            CacheStore.Set("websiteFeeds", context);
        }

        public void SetLineup(string category, RequestContext<WordpressMessage> context) {
            CacheStore.Set("lineupFeeds-" + category, context);
        }

        public void SetInfo(RequestContext<WordpressEntry> context) {
            CacheStore.Set("infoFeed", context);
        }

        public RequestContext<FacebookMessage> GetFacebookFeed() {
            return CacheStore.Get<RequestContext<FacebookMessage>>("facebookFeeds");
        }

        public RequestContext<WordpressMessage> GetWebsiteFeed() {
            return CacheStore.Get<RequestContext<WordpressMessage>>("websiteFeeds");
        }

        public RequestContext<WordpressMessage> GetLineupFeed(string category) {
            return CacheStore.Get<RequestContext<WordpressMessage>>("lineupFeeds-" + category);
        }

        public RequestContext<WordpressEntry> GetInfoFeed() {
            return CacheStore.Get<RequestContext<WordpressEntry>>("infoFeed");
        }
    }
}

