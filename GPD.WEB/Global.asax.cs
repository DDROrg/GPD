using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace GPD.WEB
{
    /// <summary>
    /// 
    /// </summary>
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            log4net.Config.XmlConfigurator.Configure();
        }

        
        //protected void FormsAuthentication_OnAuthenticate(Object sender, FormsAuthenticationEventArgs e)
        //{
        //    if (FormsAuthentication.CookiesSupported == true)
        //    {
        //        if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
        //        {
        //            try
        //            {
        //                string userEmail = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
        //                var result = new Facade.SignInFacade().GetUserRole(userEmail);
        //                SessionManager.PartnarName = result.PartnerNames.FirstOrDefault();
        //                e.User = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity(userEmail, "Forms"), result.GroupName.Split(';'));
        //            }
        //            catch (Exception ex)
        //            {
        //                log.Error("FormsAuthentication_OnAuthenticate :", ex);
        //            }
        //        }
        //    }
        //}
    }
}
