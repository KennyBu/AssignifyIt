using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            return View();
        }
    }
}
