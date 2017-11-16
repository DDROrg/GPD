using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Xml.XPath;

namespace GPD.Facade.WebAppFacade
{
    using DAL.SqlDB;
    using ServiceEntities;
    using ServiceEntities.BaseEntities;
    using ServiceEntities.ResponseEntities;
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

        public static int RegisterUser(UserDetailsTDO userDetails, string partnerName, List<KeyValuePair<string, string>> additionalData, 
            string requestIpAddress, out int dbErrorCode, out string dbErrorMsg)
        {
            dbErrorCode = -1;
            dbErrorMsg = "";

            if (string.IsNullOrWhiteSpace(userDetails.CompanyDetails.Name))
                userDetails.CompanyDetails = new CompanyDetailsDTO();

            if (string.IsNullOrWhiteSpace(userDetails.Password))
            {
                dbErrorCode = 0;
                dbErrorMsg = "Unable to register user at this time. Invalid Password.";
                return -1;
            }


            // hash user password
            userDetails.Password = ValueHashUtil.CreateHash(userDetails.Password);

            // get XML based on UserRegistrationDTO object
            XDocument xDoc = new XDocument();
            using (var writer = xDoc.CreateWriter())
            {
                var serializer = new DataContractSerializer(userDetails.GetType());
                serializer.WriteObject(writer, userDetails);
            }

            // additional user info
            if (additionalData != null && additionalData.Count > 0)
            {
                XNamespace xNamespace = xDoc.Root.Attribute("xmlns").Value;

                xDoc.Root.LastNode.AddAfterSelf(new XElement(xNamespace + "additional-data",
                    from T in additionalData
                    select new XElement(xNamespace + "item",
                        new XAttribute("type", T.Key),
                        T.Value
                    )));
            }

            // db call
            return new ProjectDB(ConfigurationHelper.GPD_Connection).AddUserDetails(xDoc, requestIpAddress, out dbErrorCode, out dbErrorMsg);
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
        /// <param name="userId"></param>
        public static SignInResponseDTO GetUserRole(int userId)
        {
            SignInResponseDTO retVal = null;

            try
            {
                DataSet ds = new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).GetUserRole(userId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    retVal = new SignInResponseDTO()
                    {
                        Email = ds.Tables[0].Rows[0]["email"].ToString(),
                        UserId = userId.ToString(),
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
                log.Error("Unable to get user profile for id: " + userId, ex);
            }

            return retVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="searchTerm"></param>
        /// <param name="orderByColIndex"></param>
        /// <param name="sortingOrder"></param>
        /// <param name="userGroupId"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="usersCount"></param>
        /// <returns></returns>
        public static List<ServiceEntities.ResponseEntities.UserDTO> GetUsers(DateTime fromDate, DateTime toDate, string searchTerm, 
            int orderByColIndex, Utility.EnumHelper.DBSortingOrder sortingOrder, int userGroupId, int startRowIndex, int pageSize, out int usersCount)
        {
            List<ServiceEntities.ResponseEntities.UserDTO> retObj = new List<ServiceEntities.ResponseEntities.UserDTO>();
            usersCount = 0;

            try
            {
                DataSet ds = new ProjectDB(ConfigurationHelper.GPD_Connection).GetUsers(fromDate, toDate, searchTerm,
                    orderByColIndex, sortingOrder.ToString(), userGroupId, startRowIndex, pageSize);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        retObj.Add(new ServiceEntities.ResponseEntities.UserDTO()
                        {
                            UserId = int.Parse(dr["user_id"].ToString()),
                            FirstName = dr["first_name"].ToString(),
                            LastName = dr["last_name"].ToString(),
                            Email = dr["email"].ToString(),
                            FirmName = DBNull.Value.Equals(dr["firmName"]) ? "" : dr["firmName"].ToString(),
                            IsActive = Convert.ToBoolean(dr["active"]),
                            CreatedOn = ((DateTime)dr["CREATE_DATE"]).ToString("o"),
                        });
                    }
                }

                if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count == 1)
                {
                    int.TryParse(ds.Tables[1].Rows[0][0].ToString(), out usersCount);
                }
            }
            catch (Exception ex)
            {
                log.Error("Unable to get users for search term: " + searchTerm, ex);
            }

            return retObj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        public static UserDetailsTDO GetUserFullProfile(int userId)
        {
            UserDetailsTDO retVal = null;

            try
            {
                DataSet ds = new UserDB(Utility.ConfigurationHelper.GPD_Connection).GetUserFullProfile(userId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    retVal = new UserDetailsTDO()
                    {
                        FirstName = ds.Tables[0].Rows[0]["first_name"].ToString(),
                        LastName = ds.Tables[0].Rows[0]["last_name"].ToString(),
                        Email = ds.Tables[0].Rows[0]["email"].ToString(),
                        JobTitle = ds.Tables[0].Rows[0]["job_title"].ToString(),
                        Phone = ds.Tables[0].Rows[0]["business_phone"].ToString(),
                    };

                    if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                    {
                        retVal.CompanyDetails = new CompanyDetailsDTO()
                        {
                            Id = (int)ds.Tables[1].Rows[0]["firm_id"],
                            Name = ds.Tables[1].Rows[0]["name"].ToString(),
                            WebSite = ds.Tables[1].Rows[0]["url"].ToString(),
                            Country = ds.Tables[1].Rows[0]["country"].ToString(),
                            Address = ds.Tables[1].Rows[0]["address_line_1"].ToString(),
                            Address2 = ds.Tables[1].Rows[0]["address_line_2"].ToString(),
                            City = ds.Tables[1].Rows[0]["city"].ToString(),
                            State = ds.Tables[1].Rows[0]["state_province"].ToString(),
                            PostalCode = ds.Tables[1].Rows[0]["zip_postal_code"].ToString(),
                            DefaultIndustry = ds.Tables[1].Rows[0]["DefaultIndustry"].ToString()
                        };
                    }
                };
            }
            catch (Exception ex)
            {
                log.Error("Unable to get user full profile for id: " + userId, ex);
            }

            return retVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userDetails"></param>
        public static bool UpdatetUserProfile(int userId, UserDetailsTDO userDetails, out string errorMsg)
        {
            try
            {
                // hash user password
                userDetails.Password = (string.IsNullOrEmpty(userDetails.Password)) ? null : ValueHashUtil.CreateHash(userDetails.Password);

                // get XML based on UserDetailsTDO object
                XDocument xDoc = new XDocument();
                using (var writer = xDoc.CreateWriter())
                {
                    var serializer = new System.Runtime.Serialization.DataContractSerializer(userDetails.GetType());
                    serializer.WriteObject(writer, userDetails);
                }

                // update user details
                int errorCode;
                new ProjectDB(ConfigurationHelper.GPD_Connection).UpdateUserProfile(userId, xDoc, out errorCode, out errorMsg);
                return (errorCode == 0);
            }
            catch (Exception exc)
            {
                log.Error("Unable to Update User Profile. ERROR: " + exc.ToString());
                errorMsg = "Unable to Update User Profile";
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="errorMsg"></param>
        public static bool ResetUserPassword(string userEmail, out string errorMsg)
        {
            bool retObj = false;
            errorMsg = string.Empty;

            try
            {
                string userPassword = Guid.NewGuid().ToString().Replace("-", "");
                userPassword = userPassword.Substring(0, 8);
                DataSet dataSet = new UserDB(ConfigurationHelper.GPD_Connection).UpdateUserPassword(userEmail, ValueHashUtil.CreateHash(userPassword));

                if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                    throw new Exception("No response from stored procedure.");

                // send email
                string emailContentFile = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\App_Data\\" + "reset-user-password-email.htm";
                string emailHtml = System.IO.File.ReadAllText(emailContentFile);

                emailHtml = emailHtml.Replace("{user-first-name}", dataSet.Tables[0].Rows[0]["first_name"].ToString());
                emailHtml = emailHtml.Replace("{user-last-name}", dataSet.Tables[0].Rows[0]["last_name"].ToString());
                emailHtml = emailHtml.Replace("{user-email-address}", dataSet.Tables[0].Rows[0]["email"].ToString());
                emailHtml = emailHtml.Replace("{user-password}", userPassword);

                // send emaill
                retObj = SendEmail(userEmail, emailHtml);
            }
            catch (Exception exc)
            {
                log.Error("Unable to reset user password. ERROR: " + exc.ToString());
                errorMsg = "Unable to Reset User Password.";
            }

            return retObj;
        }

        /// <summary>
        /// SendEmail
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="bodyContent"></param>
        public static bool SendEmail(string userEmail, string bodyContent)
        {
            MailMessage msg = new MailMessage();
            SmtpClient client = new SmtpClient();

            try
            {
                msg.From = new MailAddress(ConfigurationHelper.MailEmaillFrom);
                msg.To.Add(userEmail);
                msg.Subject = ConfigurationHelper.MailSubject;
                msg.Body = bodyContent;
                msg.IsBodyHtml = true;

                client.UseDefaultCredentials = true;
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential(ConfigurationHelper.MailUserName, ConfigurationHelper.MailUserPassword);
                client.Timeout = 20000;
                client.Send(msg);
            }
            catch (Exception exc)
            {
                log.Error(exc);
                return false;
            }
            finally
            {
                msg.Dispose();
            }

            return true;
        }
    }
}
