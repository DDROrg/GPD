using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace GPD.Facade.WebAppFacade
{
    using DAL.SqlDB;
    using ServiceEntities;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public static List<UserDTO> GetUsers(string searchTerm)
        {
            List<UserDTO> retVal = new List<UserDTO>();
            searchTerm = string.IsNullOrWhiteSpace(searchTerm) ? string.Empty : "%" + searchTerm.Trim() + "%";

            try
            {
                DataSet ds = new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).GetUsers(searchTerm);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        retVal.Add(new UserDTO()
                        {
                            UserId = int.Parse(dr["user_id"].ToString()),
                            FirstName = dr["first_name"].ToString(),
                            LastName = dr["last_name"].ToString(),
                            Email = dr["email"].ToString(),
                            FirmId = DBNull.Value.Equals(dr["firm_id"]) ? -1 : int.Parse(dr["firm_id"].ToString()),
                            FirmName = DBNull.Value.Equals(dr["firmName"]) ? "" : dr["firmName"].ToString(),
                            IsActive = Convert.ToBoolean(dr["active"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Unable to get users for search term: " + searchTerm, ex);
            }

            return retVal;
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
                return (errorCode == -1 || errorCode == 0);
            }
            catch (Exception exc)
            {
                log.Error("Unable to Update User Profile. ERROR: " + exc.ToString());
                errorMsg = "Unable to Update User Profile";
                return false;
            }
        }
    }
}
