using System;
using System.Collections.Generic;
using IcucApp.Core.Diagnostics;
using IcucApp.Core.Net;
using IcucApp.Core;
using RestSharp;

namespace IcucApp.Services.Syndication
{
    public interface IWordpressFeedAgent
    {
        WordpressMessage GetFeeds(string category);
		WordpressEntry GetSingleFeed(string path);
    }

    public class WordpressFeedAgent : IWordpressFeedAgent
    {
        private readonly string _baseUrl;
        private readonly ILog _log = LogManager.GetLogger(typeof(WordpressFeedAgent));

        public WordpressFeedAgent(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public WordpressMessage GetFeeds(string category)
        {
            try
            {
                NetworkActivityIndicator.ShowActivity();

                // Setup client
                var client = new RestClient(_baseUrl);
                client.Timeout = 10 * 1000;

                // Build request
				var request = new RestRequest(category, Method.GET);
                request.AddParameter("feed", "json");
                request.AddParameter("date_format", "c");
                //if (!category.IsNullOrEmpty()) 
                //    request.AddParameter("cat", category);
                var response = client.Execute<List<WordpressEntry>>(request);

                // error handling
                if (!response.ErrorMessage.IsNullOrEmpty())
                    throw new InvalidOperationException(response.ErrorMessage);

                // deserialize
                return new WordpressMessage(response.Data);
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

		public WordpressEntry GetSingleFeed(string path)
		{
			try
			{
				NetworkActivityIndicator.ShowActivity();
				
				// Setup client
				var client = new RestClient(_baseUrl);
				client.Timeout = 10 * 1000;
				
				// Build request
				var request = new RestRequest(path, Method.GET);
				request.AddParameter("feed", "json");
				request.AddParameter("date_format", "c");
				request.AddParameter("withoutcomments", "1");
				var response = client.Execute<List<WordpressEntry>>(request);
				
				// error handling
				if (!response.ErrorMessage.IsNullOrEmpty())
					throw new InvalidOperationException(response.ErrorMessage);
				
				// deserialize
				return response.Data[0];
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

	public class WordpressMessage 
	{
		public WordpressMessage(List<WordpressEntry> entries){
			Entries = entries;
		}

		public List<WordpressEntry> Entries {get;set;}
	}

    public class WordpressEntry
    {
        // ReSharper disable InconsistentNaming
        public string id { get; set; }
        public string permalink { get; set; }
        public string title { get; set; }
        public string excerpt { get; set; }
		public string content { get; set; }
        public DateTime date { get; set; }
        public string image { get; set; }
        // ReSharper restore InconsistentNaming
    }
}