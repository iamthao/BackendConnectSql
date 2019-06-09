using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuickspatchWeb.Controllers
{
    public class WelcomeController : Controller
    {
        // GET: Wellcome
        public ActionResult Index()
        {
            return View();
        }
    }
}