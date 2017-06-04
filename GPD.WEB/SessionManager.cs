using System;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace GPD.WEB
{
    using ServiceEntities.BaseEntities;
    public class SessionManager
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static SessionManager _instance;
        private const string SESSION_PARTNARNAME = "SESSION_PARTNARNAME";
        private const string SESSION_USERNAME = "SESSION_USERNAME";
        private const string SESSION_USERPROFILE = "SESSION_USERPROFILE";

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
        public SignInResponseDTO GetUserProfile()
        {
            if (HttpContext.Current.Session[SESSION_PARTNARNAME] == null) { loadProfile(); }
            return (HttpContext.Current.Session[SESSION_PARTNARNAME] == null) ? null : (SignInResponseDTO)HttpContext.Current.Session[SESSION_PARTNARNAME];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userProfile"></param>
        public void SetUserProfile(SignInResponseDTO userProfile)
        {
            HttpContext.Current.Session.Add(SESSION_PARTNARNAME, userProfile);
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
                            SignInResponseDTO result = new Facade.SignInFacade().GetUserRole(userEmail);
                            SetUserProfile(result);
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