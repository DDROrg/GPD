using System;
using System.Runtime.Serialization;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml.Linq;

namespace GPD.WEB.Controllers
{
    using Facade.WebAppFacade;
    using ServiceEntities;
    using CNST = Utility.ConstantHelper;

    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class AccountController : Controller
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

            int userId = -1;
            var result = UserDetailsFacade.AuthenticateUser(model.Email, model.Password, out userId);

            switch (result)
            {
                case CNST.SignInStatus.Success:
                    if (userId == -1)
                    {
                        ModelState.AddModelError("", "Unhandled Exception Error... ");
                        return View(model);
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(userId.ToString(), model.RememberMe);
                        //var userProfile = new Facade.SignInFacade().GetUserRole(model.Email);
                        //SessionManager.GetInstance().SetPartnarName(userProfile.PartnerNames.FirstOrDefault());
                        return RedirectToLocal(returnUrl);
                    }
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
            return View("~/Views/Account/Register.cshtml");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            ForgotPasswordDTO model = new ForgotPasswordDTO();
            return View("~/Views/Account/ForgotPassword.cshtml", model);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordDTO model)
        {
            model.FPStatusMessage = "An email has been sent to your registered email with new code";
            model.FPStatus = true;
            return View("~/Views/Account/ForgotPassword.cshtml", model);
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ManageUser()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ManagePartner()
        {
            return View();
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