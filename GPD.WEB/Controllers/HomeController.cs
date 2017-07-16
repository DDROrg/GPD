using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GPD.WEB.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class HomeController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.Title = "GPD - Home";
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Project()
        {
            ViewBag.Title = "GPD - Project";
            return View("~/Views/Home/Project.cshtml");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ProjectList()
        {
            return PartialView("~/Views/Home/_ProjectList.cshtml");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ProjectEdit()
        {
            return PartialView("~/Views/Home/_ProjectEdit.cshtml");
        }
    }
}
