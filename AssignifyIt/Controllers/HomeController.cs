using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using AssignifyIt.Managers;
using AssignifyIt.Models;
using AssignifyIt.Queries.Assignments;

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
            var connectionString = ConfigurationManager.ConnectionStrings["AssignifyItDatabase"].ConnectionString;

            var manager = new AssigneeManager(new AssignmentManagerQuery(connectionString));

            var list = string.IsNullOrWhiteSpace(search)
                           ? manager.GetAssignees().ToList()
                           : manager.GetAssignees(search);

            var model = new AboutViewModel
                {
                    Message = string.Format("The Value is: {0}", configValue),
                    Assignees = MapAssigneesToViewModel(list)
                };
            
            return View(model);
        }

        private List<AssigneeViewModel> MapAssigneesToViewModel(IEnumerable<Assignee> assignees)
        {
            var list = assignees != null ? assignees.Select(assignee => new AssigneeViewModel { Name = assignee.Name, Email = assignee.Email }).ToList() 
                : new List<AssigneeViewModel>();

            return list;
        }

        public JsonResult GetAssignees(string searchText)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["AssignifyItDatabase"].ConnectionString;
            var manager = new AssigneeManager(new AssignmentManagerQuery(connectionString));
            var list = manager.GetAssignees(searchText);

            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}
