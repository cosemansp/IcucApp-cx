﻿using System;
using System.Collections.Generic;
using IcucApp.Core;
using IcucApp.Core.Diagnostics;
using IcucApp.Core.UI;
using IcucApp.Presentation.ViewModels;
using IcucApp.Services.Facebook;
using IcucApp.Services.Syndication;
using IcucApp.Configuration;

namespace IcucApp.Presentation
{
    public interface INewsView : IView
    {
        void DataBind(NewsViewModel model);
    }

    public class NewsPresenter : IPresenter
    {
        private readonly INewsView _view;
		private readonly ICache _cache;
		private readonly IDataLoader _dataLoader;
        private readonly INavigator _navigator;
        private readonly IMapper<FacebookEntry, FeedData> _facebookMapper;
		private readonly IMapper<WordpressEntry, FeedData> _wordpressMapper;
        private readonly AppSettings _appSettings;

        private readonly ILog _log = LogManager.GetLogger(typeof(NewsPresenter).Name);
		int _segment;

        public NewsPresenter(INewsView view, 
		                     ICache cache,
		                     IDataLoader dataLoader,
                             INavigator navigator,
                             IMapper<FacebookEntry, FeedData> facebookMapper,
		                     IMapper<WordpressEntry, FeedData> wordpressMapper,
                             AppSettings appSettings)
        {
            _view = view;
			_cache = cache;
			_dataLoader = dataLoader;
			_navigator = navigator;
			_facebookMapper = facebookMapper;
			_wordpressMapper = wordpressMapper;
			_segment = 0;
            _appSettings = appSettings;

			_dataLoader.DataLoaded += (sender, e) => {
				if (_segment == 0) {
					var feed = _cache.GetFacebookFeed();
					if (feed != null && e.Context == "facebook") {
						DataBindView(feed);
					}
				}
				else {
					var feed = _cache.GetWebsiteFeed();
					if (feed != null && e.Context == "website") {
						DataBindView(feed);
					}
				}
			};
        }

        public void Initialize()
        {
        }

        public void OnViewShown()
        {
			if (_segment == 0) {
				LoadAndBindFacebookFeed();
			}
			if (_segment == 1) {
				LoadAndBindWebsiteNews();
			}
        }

		void LoadAndBindFacebookFeed ()
		{
			var feed = _cache.GetFacebookFeed();
			if (feed == null)
			{
				_view.DataBind(NewsViewModel.Loading);
				return;
			}
            if (feed.TimeStamp < DateTime.Now - _appSettings.TimeoutContent)
            {
                _dataLoader.ReloadFacebookFeed();
            }
			DataBindView(feed);
		}

		void LoadAndBindWebsiteNews ()
		{
			var feed = _cache.GetWebsiteFeed();
			if (feed == null)
			{
				_view.DataBind(NewsViewModel.Loading);
				return;
			}
            if (feed.TimeStamp < DateTime.Now - _appSettings.TimeoutContent)
            {
                _dataLoader.ReloadWebsiteFeed();
            }
			DataBindView(feed);
		}

        private void DataBindView(RequestContext<FacebookMessage> context)
        {
            var model = new NewsViewModel();
            if (context.Exception != null)
            {
                model.ErrorMessage = "Ophalen facebook nieuws is mislukt";
            }
            else if (context.Data != null && context.Data.entries != null) 
			{
                foreach (var feedEntry in context.Data.entries)
	            {
					var dataEntry = _facebookMapper.Map(feedEntry);
					dataEntry.ImageUrl = new Uri("http://graph.facebook.com/{0}/picture".FormatWith("441615792534282"));
					model.Entries.Add(dataEntry);
	            }
                model.LastUpdate = context.TimeStamp;
			}
            _view.DataBind(model);
        }

        private void DataBindView(RequestContext<WordpressMessage> context)
		{
			var model = new NewsViewModel();
            if (context.Exception != null)
            {
                model.ErrorMessage = "Ophalen icue nieuws is mislukt";
            }
            if (context.Data != null && context.Data.Entries != null) 
			{
                foreach (var websiteEntry in context.Data.Entries)
				{
					model.Entries.Add(_wordpressMapper.Map(websiteEntry));
				}
                model.LastUpdate = context.TimeStamp;
			}
			_view.DataBind(model);
		}

        public void OnViewUnloaded()
        {
        }

		public void OpenNewsFeed (int selectedSegment)
		{
			if (selectedSegment == 0) {
				LoadAndBindFacebookFeed();
			}

			if (selectedSegment == 1) {
				LoadAndBindWebsiteNews();
			}

			_segment = selectedSegment;
		}

        public void OnClickedFacebookFeed(string feedId)
        {
            _navigator.PushPresenter<FacebookDetailPresenter>(_view, feedId);
        }

        public void OnClickedWebsiteFeed(string feedId)
        {
            _navigator.PushPresenter<WebsiteDetailPresenter>(_view, feedId);
        }

        public void ReloadAll ()
        {
            _dataLoader.ReloadAllFeed();
            _view.DataBind(NewsViewModel.Loading);
        }

		public void Reload ()
		{
            if (_segment == 0)
            {
                _dataLoader.ReloadFacebookFeed();
            }
            else
            {
                _dataLoader.ReloadWebsiteFeed();
            }
		}
    }
}