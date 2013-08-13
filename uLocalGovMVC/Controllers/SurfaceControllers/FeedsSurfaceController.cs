using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Core;
using System.Web.Mvc;
using System.ServiceModel.Syndication;
using uLocalGovMVC.mvc;

namespace uLocalGovMVC.Controllers.SurfaceControllers
{
    public class FeedsSurfaceController : SurfaceController
    {
        public ActionResult NewsRssFeed()
        {
            var latestNews = Umbraco.TypedContentAtXPath("//ulgNewsItem").Where(x => x.IsVisible()).OrderByDescending(x => x.GetPropertyValue<DateTime>("articleDate")).Take(20);
  
          
            var lastUpdate = (from news in latestNews
                              select news.UpdateDate).Max();
            SyndicationFeed feed =
                     new SyndicationFeed("Local Government Starter Kit News RSS Feed Example",
                                         "Latest News for Local Government Starter Kit site",
                                         new Uri("http://www.uLocalGov.co.uk"),
                                         "uLocalCovNews",
                                         lastUpdate);


            List<SyndicationItem> items = new List<SyndicationItem>();
            foreach (var newsItem in latestNews)
            {
                string itemTitle = string.Format("{0}", newsItem.GetPropertyValue("title"));
                   Uri itemURI = new Uri(newsItem.UrlWithDomain());
                string itemContent = string.Format("<p>{0:dddd, MM yyyy} - {1}</p>", newsItem.GetPropertyValue<DateTime>("articleDate"), newsItem.GetPropertyValue("newsSummary"));
                SyndicationItem item =
                    new SyndicationItem(itemTitle,
                                        itemContent,
                                        itemURI,
                                        newsItem.Id.ToString(),
                                        newsItem.UpdateDate);

                items.Add(item);
            }
            feed.Items = items;
            return new RssActionResult(feed);
        }
    }

    }
