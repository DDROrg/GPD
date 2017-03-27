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
    }
}
