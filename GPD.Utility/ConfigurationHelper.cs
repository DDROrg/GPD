using System.Configuration;

namespace GPD.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConfigurationHelper
    {
        public static string GPD_Connection { get { return GetDbConnection(ConfigurationManager.AppSettings["GPD_CONNECTION"]); } }

        private static string GetDbConnection(string dbName)
        {
            return ConfigurationManager.ConnectionStrings[dbName].ConnectionString;
        }

        public static int API_ProjectsListPageSize { get { return int.Parse(ConfigurationManager.AppSettings["API-PROJECTS-LIST-PAGE-SIZE"]); } }
        public static int API_ProjectsListPageMaxSize { get { return int.Parse(ConfigurationManager.AppSettings["API-PROJECTS-LIST-PAGE-MAX-SIZE"]); } }

        public static string DefaultPartnerImageUrl { get { return ConfigurationManager.AppSettings["DEFAULT-PARTNER-IMAGE-URL"]; } }

        public static string ProfileImageFolder { get { return ConfigurationManager.AppSettings["PROFILE-IMAGE-FOLDER"]; } }

        public static string MailEmaillFrom { get { return ConfigurationManager.AppSettings["MAIL-EMAIL-FROM"]; } }
        public static string MailUserName { get { return ConfigurationManager.AppSettings["MAIL-USER-NAME"]; } }
        public static string MailUserPassword { get { return ConfigurationManager.AppSettings["MAIL-USER-PASSWORD"]; } }
        public static string MailSubject { get { return ConfigurationManager.AppSettings["MAIL-SUBJECT"]; } }
    }
}
