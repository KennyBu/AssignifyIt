using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssignifyIt.Managers;
using AssignifyIt.Queries.Assignments;

namespace AssignifyIt.Controllers
{
    [Authorize]
    public class AssignmentController : Controller
    {
        //
        // GET: /Assignment/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SubmitAssignment(string txtName)
        {
            var assignmentName = txtName;

            return Redirect("/Home");
        }
    }
}
