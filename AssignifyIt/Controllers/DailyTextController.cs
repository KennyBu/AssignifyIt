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
            _query = new DailyTextManagerQuery(connectionString);
            _dailyTextManager = new DailyTextManager(_query);
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

    }
}
