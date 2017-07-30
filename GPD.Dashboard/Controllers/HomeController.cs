using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GPD.Dashboard.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Views/Home/Index.cshtml");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Dashboard()
        {
            return PartialView("~/Views/Home/_Dashboard.cshtml");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Project()
        {
            return PartialView("~/Views/Home/_Project.cshtml");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Manage()
        {
            return PartialView("~/Views/Home/_Manage.cshtml");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Report()
        {
            return PartialView("~/Views/Home/_Report.cshtml");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Map()
        {
            return PartialView("~/Views/Home/_Map.cshtml");
        }
    }
}