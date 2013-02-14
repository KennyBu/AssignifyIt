using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using AssignifyIt.Code;
using AssignifyIt.Managers;
using AssignifyIt.Models;

namespace AssignifyIt.Controllers
{
    public class FeedController : Controller
    {

        private readonly DailyTextManager _dailyTextManager;

        public FeedController()
        {
            _dailyTextManager = new DailyTextManager();
        }
        //
        // GET: /Feed/

        public ActionResult Index()
        {
            var texts = new List<DailyText>();
            var text = _dailyTextManager.GetTodaysText();
            texts.Add(text);
            
            var postItems = texts
                .Select(p => new SyndicationItem(p.DateLine, string.Concat(p.Header,"<br/>",p.Body), new Uri("http://assignit.apphb.com/Feed")));

            var feed = new SyndicationFeed("Daily Text", "Daily Text", new Uri("http://assignit.apphb.com/Feed"), postItems)
            {
                Copyright = new TextSyndicationContent("copyright never"),
                Language = "en-US"
            };

            return new FeedResult(new Rss20FeedFormatter(feed));
        }

    }
}
