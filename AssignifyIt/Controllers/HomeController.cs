using System.Configuration;
using System.Web.Mvc;
using AssignifyIt.Models;

namespace AssignifyIt.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to AssignifyIt! This is a test";

            return View();
        }

        public ActionResult About(string search)
        {
            var configValue = ConfigurationManager.AppSettings["kentest"];
            
            var model = new AboutViewModel
                {
                    Message = string.Format("The Value is: {0}", configValue)
                };
            
            return View(model);
        }
    }
}
