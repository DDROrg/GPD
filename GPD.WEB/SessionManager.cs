﻿using System;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace GPD.WEB
{
    using Facade.WebAppFacade;
    using ServiceEntities.BaseEntities;
    /// <summary>
    /// 
    /// </summary>
    public class SessionManager
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static SessionManager _instance;
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
            if (HttpContext.Current.Session[SESSION_USERPROFILE] == null) { loadProfile(); }
            return (HttpContext.Current.Session[SESSION_USERPROFILE] == null) ? null : (SignInResponseDTO)HttpContext.Current.Session[SESSION_USERPROFILE];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <param name="partner"></param>
        /// <returns></returns>
        public bool HasRolesForPartner(string role, string partner)
        {
            try
            {
                // get user profile
                SignInResponseDTO userProfile = GetUserProfile();

                if (userProfile != null)
                    return userProfile.Roles.Exists(i => i.PartnerName.Equals(partner) && i.GroupName.Equals(role));
            }
            catch (Exception exc)
            {
                log.Error(exc);
            }

            return false;
        }

        /// <summary>
        /// Is User has ADMIN role
        /// </summary>
        /// <returns>bool</returns>
        public bool AdminRole()
        {
            try
            {
                // get user profile
                SignInResponseDTO userProfile = GetUserProfile();

                if (userProfile != null)
                    return userProfile.Roles.Exists(T => T.GroupName.ToUpper().Contains("ADMIN"));
            }
            catch (Exception exc)
            {
                log.Error(exc);
            }

            return false;
        }

        /// <summary>
        /// Is User has GPDRole role
        /// </summary>
        /// <returns>bool</returns>
        public bool AdminGPDRole()
        {
            try
            {
                // get user profile
                SignInResponseDTO userProfile = GetUserProfile();

                if (userProfile != null)
                    return userProfile.Roles.Exists(T => T.GroupName.ToUpper().Contains("GPD ADMIN"));
            }
            catch (Exception exc)
            {
                log.Error(exc);
            }

            return false;
        }

        /// <summary>
        /// Is User assigned to any Roles in the list
        /// </summary>
        /// <returns>bool</returns>
        public bool AnyFromRoles(string[] rolesList)
        {
            try
            {
                // get user profile
                SignInResponseDTO userProfile = GetUserProfile();

                if (userProfile != null)
                    return userProfile.Roles.Any(T => rolesList.Contains(T.GroupName.ToUpper()));
            }
            catch (Exception exc)
            {
                log.Error(exc);
            }

            return false;
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
            if (FormsAuthentication.CookiesSupported && 
                HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                try
                {
                    string encryptedValue = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName].Value;

                    if (encryptedValue != null)
                    {
                        int userId = int.Parse(FormsAuthentication.Decrypt(encryptedValue).Name);
                        SignInResponseDTO userProfile = UserDetailsFacade.GetUserRole(userId);

                        if(userProfile != null)
                            HttpContext.Current.Session.Add(SESSION_USERPROFILE, userProfile);
                    }
                }
                catch (Exception exc)
                {
                    log.Error(exc);
                }
            }
        }

    }
}