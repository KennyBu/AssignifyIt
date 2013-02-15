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

        public ActionResult About()
        {
            var configValue = ConfigurationManager.AppSettings["kentest"];
            var connectionString = ConfigurationManager.ConnectionStrings["AssignifyItDatabase"].ConnectionString;

            var manager = new AssigneeManager(new AssignmentManagerQuery(connectionString));
            var list = manager.GetAssignees();

            var model = new AboutViewModel
                {
                    Message = string.Format("The Value is: {0}", configValue),
                    Assignees = MapAssigneesToViewModel(list)
                };
            
            return View(model);
        }

        private List<AssigneeViewModel> MapAssigneesToViewModel(IEnumerable<Assignee> assignees)
        {
            return assignees.Select(assignee => new AssigneeViewModel
                {
                    Name = assignee.Name, Email = assignee.Email
                }).ToList();
        }

        public JsonResult GetAssignees()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["AssignifyItDatabase"].ConnectionString;
            var manager = new AssigneeManager(new AssignmentManagerQuery(connectionString));
            var list = manager.GetAssignees();

            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}
