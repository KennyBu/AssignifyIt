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
        private readonly IAssigneeManager _assigneeManager;

        public HomeController()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["AssignifyItDatabase"].ConnectionString;
            var elasticSearchUrl = ConfigurationManager.AppSettings["SEARCHBOX_URL"];

            _assigneeManager = new AssigneeManager(new AssignmentManagerQuery(connectionString), new ElasticSearchManager(elasticSearchUrl));
            _assigneeManager.IndexAssignees();
        }
        
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to AssignifyIt! This is a test";

            return View();
        }

        public ActionResult About(string search)
        {
            var configValue = ConfigurationManager.AppSettings["kentest"];
            var connectionString = ConfigurationManager.ConnectionStrings["AssignifyItDatabase"].ConnectionString;

            _assigneeManager.Reindex();

            /*
            var manager = new AssigneeManager(new AssignmentManagerQuery(connectionString));

            var list = string.IsNullOrWhiteSpace(search)
                           ? manager.GetAssignees().ToList()
                           : manager.GetAssignees(search);
            */
            var model = new AboutViewModel
                {
                    Message = string.Format("The Value is: {0}", configValue),
                    Assignees = new List<AssigneeViewModel>()
                    //Assignees = MapAssigneesToViewModel(list)
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
            var list = _assigneeManager.GetAssignees(searchText);

            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}
