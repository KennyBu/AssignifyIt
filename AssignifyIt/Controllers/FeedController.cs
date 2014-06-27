using System;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using AssignifyIt.Code;
using AssignifyIt.Managers;
using AssignifyIt.Queries.DailyTexts;

namespace AssignifyIt.Controllers
{
    public class FeedController : Controller
    {
        private readonly DailyTextManagerQuery _query;
        private readonly DailyTextManager _dailyTextManager;

        public FeedController()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["AssignifyItDatabase"].ConnectionString;
            _query = new DailyTextManagerQuery(connectionString);
            _dailyTextManager = new DailyTextManager(_query);
        }
        //
        // GET: /Feed/

        public ActionResult Index()
        {
            var texts = _dailyTextManager.GetDailyTextList();
            
            var postItems = texts
                .Select(p => new SyndicationItem(p.DateLine, GetContent(string.Concat(p.Header, "<br/>", p.Body)), new Uri(string.Format("http://assignit.apphb.com/DailyText/Article/{0}",p.Id))));

            var feed = new SyndicationFeed("Daily Text", "The Daily Text", new Uri("http://assignit.apphb.com/Feed"), postItems)
            {
                Copyright = new TextSyndicationContent("Copyright 2013"),
                Language = "en-US"
            };

            return new FeedResult(new Rss20FeedFormatter(feed));
        }

        private string GetContent(string inContent)
        {
            return new TextSyndicationContent(inContent, TextSyndicationContentKind.Html).Text;
        }

    }
}
