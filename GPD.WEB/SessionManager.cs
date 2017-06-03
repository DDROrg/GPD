using System;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace GPD.WEB
{
    public class SessionManager
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static SessionManager _instance;
        private const string SESSION_PARTNARNAME = "SESSION_PARTNARNAME";
        private const string SESSION_USERNAME = "SESSION_USERNAME";

        #region Constr
        private SessionManager() { }
        #endregion Constr

        /// <summary>
        /// GetInstance
        /// </summary>
        /// <returns></returns>
        public static SessionManager GetInstance()
        {
            if (_instance == null) { _instance = new SessionManager(); }
            return _instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetPartnarName()
        {
            if (HttpContext.Current.Session[SESSION_PARTNARNAME] == null) { loadProfile(); }
            return (HttpContext.Current.Session[SESSION_PARTNARNAME] == null) ? string.Empty : HttpContext.Current.Session[SESSION_PARTNARNAME].ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partnarName"></param>
        public void SetPartnarName(string partnarName)
        {
            HttpContext.Current.Session.Add(SESSION_PARTNARNAME, partnarName);
        }

        /// <summary>
        /// GetUserName
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            if (HttpContext.Current.Session[SESSION_USERNAME] == null) { loadProfile(); }
            return HttpContext.Current.Session[SESSION_USERNAME].ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        public void SetUserName(string userName)
        {
            HttpContext.Current.Session.Add(SESSION_USERNAME, userName);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearSession()
        {
            HttpContext.Current.Session.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void loadProfile()
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        string encryptedName = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName].Value;

                        if (encryptedName != null)
                        {
                            string userEmail = FormsAuthentication.Decrypt(encryptedName).Name;
                            var result = new Facade.SignInFacade().GetUserRole(userEmail);
                            SetPartnarName(result.PartnerNames.FirstOrDefault());
                            SetUserName(result.FirstName + " " + result.LastName);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("FormsAuthentication_OnAuthenticate :", ex);
                    }
                }
            }
        }

    }
}