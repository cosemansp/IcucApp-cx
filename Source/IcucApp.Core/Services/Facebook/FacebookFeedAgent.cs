using System;
using System.Collections.Generic;
using IcucApp.Core.Diagnostics;
using IcucApp.Core.Net;
using RestSharp;

namespace IcucApp.Services.Facebook
{
    public interface IFacebookFeedAgent
    {
        FacebookMessage GetFeeds(string feedId);
    }

    /// <summary>
    /// http://graph.facebook.com/ICUCevents?fref=ts
    /// https://www.facebook.com/feeds/page.php?id=441615792534282&format=json
    /// </summary>
    public class FacebookFeedAgent : IFacebookFeedAgent
    {
        private readonly Uri _uri;
        private readonly ILog _log = LogManager.GetLogger(typeof(FacebookFeedAgent));

        public FacebookFeedAgent()
        {
            _uri = new Uri("https://www.facebook.com/feeds/page.php");
        }

        public FacebookMessage GetFeeds(string feedId)
        {
            try
            {
                NetworkActivityIndicator.ShowActivity();

                // Setup client
                var client = new RestClient(_uri.ToString());
                client.Timeout = 10 * 1000;

                // Build request
                var request = new RestRequest("", Method.GET);
                request.AddParameter("id", feedId);
                request.AddParameter("format", "json");

                var response = client.Execute<FacebookMessage>(request);

                // deserialize
                return response.Data;
            }
            catch (Exception ex)
            {
                _log.WarnFormat("Failed to Feeds: {1} - {0}", ex.GetType().Name, ex.Message);
                throw;
            }
			finally {
				NetworkActivityIndicator.HideActivity();
			}
        }
    }

    public class FacebookMessage
    {
        // ReSharper disable InconsistentNaming
        public string title { get; set; }
        public string link { get; set; }
        public List<FacebookEntry> entries { get; set; }
        // ReSharper restore InconsistentNaming
    }

    public class FacebookEntry
    {
        // ReSharper disable InconsistentNaming
        public string title { get; set; }
        public string id { get; set; }
        public DateTime published { get; set; }
        public DateTime updated { get; set; }
        public FacebookAuthor author { get; set; }
        public string content { get; set; }
        // ReSharper restore InconsistentNaming
    }

    public class FacebookAuthor
    {
        // ReSharper disable InconsistentNaming
        public string name { get; set; }
        // ReSharper restore InconsistentNaming
    }
}