using System;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace GPD.Facade.WebAppFacade
{
    using DAL.SqlDB;
    using ServiceEntities.BaseEntities;
    using Utility;
    using Utility.CommonUtils;
    using Utility.ConstantHelper;

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
        public static int AuthenticateUser(string userEmail, string userPassword, out int userid)
        {
            int retVal = SignInStatus.Failure;
            userid = -1;

            try
            {
                // gete project data
                DataSet ds = new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).AuthenticateUser(userEmail);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    string passwordHash = ds.Tables[0].Rows[0]["password"].ToString();

                    if (ValueHashUtil.ValidateHash(userPassword, passwordHash))
                    {
                        userid = (int)ds.Tables[0].Rows[0]["user_id"];
                        retVal = SignInStatus.Success;                        
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Unable to sign in id: " + userEmail ?? "n/a", ex);
            }

            return retVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public static SignInResponseDTO GetUserRole(string email)
        {
            SignInResponseDTO retVal = null;

            try
            {
                DataSet ds = new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).GetUserRole(email);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    retVal = new SignInResponseDTO()
                    {
                        Email = email.ToLower(),
                        UserId = ds.Tables[0].Rows[0]["user_id"].ToString(),
                        FirstName = ds.Tables[0].Rows[0]["first_name"].ToString(),
                        LastName = ds.Tables[0].Rows[0]["last_name"].ToString()
                    };

                    if (ds.Tables.Count == 1)
                        return retVal;

                    foreach (DataRow dataRow in ds.Tables[1].Rows)
                    {
                        retVal.Roles.Add(new UserRoleDTO()
                        {
                            GroupId = int.Parse(dataRow["group_id"].ToString()),
                            GroupName = dataRow["GroupName"].ToString(),
                            PartnerId = dataRow["partner_id"].ToString(),
                            PartnerName = dataRow["PartnerName"].ToString(),
                            PartnerImageUrl = DBNull.Value.Equals(dataRow["PartnerImageUrl"]) ? ConfigurationHelper.DefaultPartnerImageUrl : dataRow["PartnerImageUrl"].ToString()
                        });
                    }

                    if (retVal.Roles.Count > 0)
                    {
                        retVal.PartnerNames = retVal.Roles.Select(i => i.PartnerName).Distinct().ToList();
                        retVal.SelectedPartner = retVal.PartnerNames.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Unable to get user profile for id: " + email, ex);
            }

            return retVal;
        }
    }
}
