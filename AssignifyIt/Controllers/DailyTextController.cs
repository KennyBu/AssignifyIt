using System.Web.Mvc;
using AssignifyIt.Managers;
using AssignifyIt.Models;

namespace AssignifyIt.Controllers
{
    public class DailyTextController : Controller
    {
        private readonly DailyTextManager _dailyTextManager;
        
        public DailyTextController()
        {
            _dailyTextManager = new DailyTextManager();
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
