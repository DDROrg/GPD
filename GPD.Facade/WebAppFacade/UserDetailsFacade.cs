using System;
using System.Data;
using System.Xml.Linq;

namespace GPD.Facade.WebAppFacade
{
    using DAL.SqlDB;

    public class UserDetailsFacade
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static int AddUserDetails(XDocument userDetails, string requestIpAddress, out int errorCode, out string errorMsg)
        {
            int userId = -1;
            errorCode = -1;
            errorMsg = string.Empty;

            try
            {
                // add user details
                userId = new ProjectDB(Utility.ConfigurationHelper.GPD_Connection)
                    .AddUserDetails(userDetails, requestIpAddress, out errorCode, out errorMsg);
            }
            catch (Exception exc)
            {
                log.Error("Unable to Add New User" + exc.ToString());
            }

            return userId;
        }
    }
}
