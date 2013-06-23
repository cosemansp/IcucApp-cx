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
            var cacheKey = "facebookFeeds";
            if (context.Exception == null)
            {
                // all ok, set to cache and persist
                CacheStore.Set(cacheKey, context, true);
            }
            else if (GetFacebookFeed() == null)
            {
                // only store in cache when no data was available
                CacheStore.Set(cacheKey, context, false);
            }
        }

        public void SetWebsite(RequestContext<WordpressMessage> context) {
            var cacheKey = "websiteFeeds";
            if (context.Exception == null)
            {
                // all ok, set to cache and persist
                CacheStore.Set(cacheKey, context, true);
            }
            else if (GetWebsiteFeed() == null)
            {
                // only store in cache when no data was available
                CacheStore.Set(cacheKey, context, false);
            }
        }

        public void SetLineup(string category, RequestContext<WordpressMessage> context) {
            var cacheKey = "lineupFeeds-" + category;
            if (context.Exception == null)
            {
                CacheStore.Set(cacheKey, context, true);
            }
            else if (GetLineupFeed(category) == null)
            {
                // only store in cache when no data was available
                CacheStore.Set(cacheKey, context, false);
            }
        }

        public void SetInfo(RequestContext<WordpressEntry> context) {
            var cacheKey = "infoFeed";
            if (context.Exception == null)
            {
                // all ok, set to cache and persist
                CacheStore.Set(cacheKey, context, true);
            }
            else if (GetInfoFeed() == null)
            {
                // only store in cache when no data was available
                CacheStore.Set(cacheKey, context, false);
            }
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

