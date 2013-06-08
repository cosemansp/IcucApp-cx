using System;
using System.Threading.Tasks;
using IcucApp.Services.Facebook;
using IcucApp.Services.Syndication;
using System.Collections.Generic;
using IcucApp.Core;

namespace IcucApp
{

    public static class ExceptionExtensions
    {
        public static string GetMessage(this AggregateException exception)
        {
            var exceptionMessage = string.Empty;
            if (exception != null && exception.InnerExceptions.Count > 0)
                exceptionMessage = exception.InnerExceptions[0].Message;
            return exceptionMessage;
        }

        public static string ToLongString(this Exception exception)
        {
            if (exception == null)
                return "null exception";

            return string.Format("{0}: {1}\n\t{2}",
                                 exception.GetType().Name,
                                 exception.Message.IsNullOrEmpty() ? "-" : exception.Message,
                                 exception.StackTrace);
        } 
    }

	public interface IDataLoader
	{
		void Start();
        void ReloadFacebookFeed();
        void ReloadWebsiteFeed();
        void ReloadWebsiteLineup();
        void ReloadWebsiteInfo();

        event EventHandler DataLoaded;
	}

	public class DataLoader : IDataLoader
	{
		IFacebookFeedAgent _facebookFeedAgent;
		IWordpressFeedAgent _wordpressFeedAgent;
        IDispatcher _dispatcher;
        ICache _cache;

        public event EventHandler DataLoaded;

		public DataLoader (IFacebookFeedAgent facebookFeedAgent, 
                           IWordpressFeedAgent wordpressFeedAgent,
                           IDispatcher dispatcher,
                           ICache cache)
		{
            _dispatcher = dispatcher;
            _cache = cache;
			_wordpressFeedAgent = wordpressFeedAgent;
            _facebookFeedAgent = facebookFeedAgent;
		}

		public void Start() {
            // load facebook news
			var task1 = Task.Factory.StartNew (() => LoadFacebookNews());
			
            // store facebook and load website news
            var task2 = task1.ContinueWith(t => {
                HandleFacebookResult(t);
				return LoadWebsiteNews();
			});

            // store website and load info
            var task3 = task2.ContinueWith(t => {
                HandleWebsiteResult(t);
                return LoadWebsiteLineup();
            });

            // store lineup and load info
			var task4 = task3.ContinueWith(t => {
                HandleLineupResult(t);
				return LoadInfo();
			});

            // store info
			task4.ContinueWith(t => {
                HandleInfoResult(t);
				return LoadInfo();
			});
		}

        private void SignalDataLoaded() {
            _dispatcher.DispatchOnMainThread( () => {
                if (DataLoaded != null) {
                    DataLoaded(this, EventArgs.Empty);
                }
            });
        }

        // facebook 

        public void ReloadFacebookFeed() {
            Task.Factory
                .StartNew<FacebookMessage>(() => 
                {
                    return LoadFacebookNews();
                })
                .ContinueWith(t => 
                {
                    HandleFacebookResult(t);
                });
        }

        private void HandleFacebookResult(Task<FacebookMessage> task) {
            RequestContext<FacebookMessage> context; 
            if (task.IsFaulted) {
                context = new RequestContext<FacebookMessage>(task.Exception.InnerExceptions[0]);
            }
            else {
                context = new RequestContext<FacebookMessage>(task.Result);
            }
            _cache.SetFacebook(context);
            SignalDataLoaded();
        }

		public FacebookMessage LoadFacebookNews() {
			return _facebookFeedAgent.GetFeeds("441615792534282" /* icuc feedId */);
		}

        // website feed

        public void ReloadWebsiteFeed() {
            Task.Factory
                .StartNew<WordpressMessage>(() => 
                {
                    return LoadWebsiteNews();
                })
                .ContinueWith(t => 
                {
                    HandleWebsiteResult(t);
                });
        }

        private void HandleWebsiteResult(Task<WordpressMessage> task) {
            RequestContext<WordpressMessage> context; 
            if (task.IsFaulted) {
                context = new RequestContext<WordpressMessage>(task.Exception.InnerExceptions[0]);
            }
            else {
                context = new RequestContext<WordpressMessage>(task.Result);
            }
            _cache.SetWebsite(context);
            SignalDataLoaded();
        }

		public WordpressMessage LoadWebsiteNews() {
			return _wordpressFeedAgent.GetFeeds("2013/category/app-news");
		}

        // website lineup

        public void ReloadWebsiteLineup() {
            Task.Factory
                .StartNew<WordpressMessage>(() => 
                                            {
                    return LoadWebsiteLineup();
                })
                .ContinueWith(t => 
                {
                    HandleLineupResult(t);
                });
        }

        private void HandleLineupResult(Task<WordpressMessage> task) {
            RequestContext<WordpressMessage> context; 
            if (task.IsFaulted) {
                context = new RequestContext<WordpressMessage>(task.Exception.InnerExceptions[0]);
            }
            else {
                context = new RequestContext<WordpressMessage>(task.Result);
            }
            _cache.SetLineup(context);
            SignalDataLoaded();
        }

        public WordpressMessage LoadWebsiteLineup() {
            return _wordpressFeedAgent.GetFeeds("2013/category/app-lineup");
        }

        // website info

        public void ReloadWebsiteInfo() {
            Task.Factory
                .StartNew<WordpressEntry>(() => 
                {
                    return LoadInfo();
                })
                .ContinueWith(t => 
                {
                    HandleInfoResult(t);
                });
        }

        private void HandleInfoResult(Task<WordpressEntry> task) {
            RequestContext<WordpressEntry> context; 
            if (task.IsFaulted) {
                context = new RequestContext<WordpressEntry>(task.Exception.InnerExceptions[0]);
            }
            else {
                context = new RequestContext<WordpressEntry>(task.Result);
            }
            _cache.SetInfo(context);
            SignalDataLoaded();
        }

		public WordpressEntry LoadInfo() {
			return _wordpressFeedAgent.GetSingleFeed("2013/app-info");
		}

	}

	public class RequestContext<T> {
		public Exception Exception { get; set; }
		public DateTime TimeStamp { get; set; }
		public T Data { get; set; }

		public RequestContext(T data) {
			Data = data;
            TimeStamp = DateTime.Now;
		}

        public RequestContext(Exception exception) {
            Exception = exception;
            TimeStamp = DateTime.Now;
        }
	}
}

