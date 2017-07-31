using System;
using System.Data;
using System.Xml.Linq;
using System.Xml.XPath;

namespace GPD.Facade.WebAppFacade
{
    using DAL.SqlDB;    
    using Utility.CommonUtils;
    using CNST = Utility.ConstantHelper;

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
                // hash user password
                userDetails.XPathSelectElement("//*[local-name()='password']").Value =
                    ValueHashUtil.CreateHash(userDetails.XPathSelectElement("//*[local-name()='password']").Value);

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public static int AuthenticateUser(string userEmail, string userPassword)
        {
            int retVal = CNST.SignInStatus.Failure;

            try
            {
                // gete project data
                DataSet ds = new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).AuthenticateUser(userEmail);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    string passwordHash = ds.Tables[0].Rows[0]["password"].ToString();

                    if (ValueHashUtil.ValidateHash(userPassword, passwordHash))
                        retVal = CNST.SignInStatus.Success;
                }
            }
            catch (Exception ex)
            {
                log.Error("Unable to sign in id: " + userEmail ?? "n/a", ex);
            }

            return retVal;
        }
    }
}
