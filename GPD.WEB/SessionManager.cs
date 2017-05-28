using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GPD.WEB
{
    /// <summary>
    /// 
    /// </summary>
    public static class SessionManager
    {
        public static string PartnarName
        {
            get { return HttpContext.Current.Session["SESSION_PARTNARNAME"] != null ? HttpContext.Current.Session["SESSION_PARTNARNAME"].ToString() : ""; }
            set { HttpContext.Current.Session.Add("SESSION_PARTNARNAME", value); }
        }

        public static void ClearSession()
        {
            HttpContext.Current.Session.Clear();
        }

    }
}