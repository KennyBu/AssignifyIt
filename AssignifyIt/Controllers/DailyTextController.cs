using System;
using System.Configuration;
using System.Web.Mvc;
using AssignifyIt.Managers;
using AssignifyIt.Models;
using AssignifyIt.Queries.DailyTexts;

namespace AssignifyIt.Controllers
{
    public class DailyTextController : Controller
    {
        private readonly DailyTextManagerQuery _query;
        private readonly DailyTextManager _dailyTextManager;
        
        public DailyTextController()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["AssignifyItDatabase"].ConnectionString;
            var redisUrl = ConfigurationManager.AppSettings["REDISCLOUD_URL"];
            var redisManager = new RedisManager(redisUrl);
            _query = new DailyTextManagerQuery(connectionString);
            _dailyTextManager = new DailyTextManager(_query, redisManager);
        }
        
        //
        // GET: /DailyText/

        public ActionResult Index()
        {
            var model = _dailyTextManager.GetTodaysText();
            var viewModel = new DailyTextViewModel
                {
                    DateLine = model.DateLine,
                    Header = model.Header,
                    Body = model.Body
                };
            
            return View(viewModel);
        }

        public ActionResult Article(Guid id)
        {
            var model = _dailyTextManager.GetText(id);
            var viewModel = new DailyTextViewModel
            {
                DateLine = model.DateLine,
                Header = model.Header,
                Body = model.Body
            };

            return View("Index",viewModel);
        }

       [HttpGet]
        public JsonResult Json()
        {
            var model = _dailyTextManager.GetTodaysText();
            var viewModel = new DailyTextViewModel
            {
                DateLine = model.DateLine,
                Header = model.Header,
                Body = model.Body
            };

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }
    }
}
