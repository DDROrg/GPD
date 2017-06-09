using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Security;


namespace GPD.WEB.Controllers
{
    using ServiceEntities;
    using CNST = Utility.ConstantHelper;

    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class AccountController : Controller
    {
        #region Private Action
        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginDTO model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = new Facade.SignInFacade().AuthenticateUser(model.Email, model.Password);

            switch (result)
            {
                case CNST.SignInStatus.Success:
                    FormsAuthentication.SetAuthCookie(model.Email, model.RememberMe);
                    //var userProfile = new Facade.SignInFacade().GetUserRole(model.Email);
                    //SessionManager.GetInstance().SetPartnarName(userProfile.PartnerNames.FirstOrDefault());
                    return RedirectToLocal(returnUrl);
                case CNST.SignInStatus.LockedOut:
                    return View("Lockout");
                case CNST.SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case CNST.SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserRegistrationDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //TODO: Facade

            //if success 
            //return RedirectToAction("Login")
            //else
            //ModelState.AddModelError("", "Invalid");
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            SessionManager.GetInstance().ClearSession();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Private Method
        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        } 
        #endregion
    }
}