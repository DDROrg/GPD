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
            if (!ModelState.IsValid) { return View(model); }

            try
            {
                XDocument xDoc = new XDocument();
                
                // get XML based on UserRegistrationDTO object
                using (var writer = xDoc.CreateWriter())
                {
                    var serializer = new DataContractSerializer(model.GetType());
                    serializer.WriteObject(writer, model);
                }

                int errorCode;
                string errorMsg;

                string requestIpAddress = string.Empty;
                try { requestIpAddress = System.Web.HttpContext.Current.Request.UserHostAddress; }
                catch {}

                // add a user to repository
                int userId = UserDetailsFacade.AddUserDetails(xDoc, requestIpAddress, out errorCode, out errorMsg);

                if (userId != -1)
                    return RedirectToAction("Login");

                if (errorCode == 0 && !string.IsNullOrEmpty(errorMsg))
                    ModelState.AddModelError("", errorMsg);
                else
                    throw new Exception(string.Format("Add new UserDeatils() - Database ERROR: ErrorCode: {0}, ErrorMsg: {1}", errorCode, errorMsg));
            }
            catch (Exception exc)
            {
                log.Error(exc);
                ModelState.AddModelError("", "The server encountered an error processing registration. Please try again later.");
            }

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