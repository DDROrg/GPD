﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Xml.XPath;


namespace GPD.Facade
{
    using DAL.SqlDB;
    using ServiceEntities;
    using ServiceEntities.BaseEntities;
    using CNST = Utility.ConstantHelper;

    /// <summary>
    /// 
    /// </summary>
    public class SignInFacade : BaseFacade
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SignInFacade() : base() { }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public SignInResponseDTO GetUserRole(string email)
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
                        retVal.Roles.Add(new UserRoleDTO() {
                            GroupId = int.Parse(dataRow["group_id"].ToString()),
                            GroupName = dataRow["GroupName"].ToString(),
                            PartnerId = dataRow["partner_id"].ToString(),
                            PartnerName = dataRow["PartnerName"].ToString()
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

        public object AuthenticateUser(string email, string password)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<PartnerDTO> GetPartners()
        {
            List<PartnerDTO> retVal = new List<PartnerDTO>();
            try
            {
                DataSet ds = new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).GetPartners();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        PartnerDTO tempPartnerDTO = new PartnerDTO();
                        tempPartnerDTO.partnerId = dr["partner_id"].ToString();
                        tempPartnerDTO.Name = dr["name"].ToString();
                        tempPartnerDTO.URL = dr["site_url"].ToString();
                        tempPartnerDTO.ShortDescription = dr["short_description"].ToString();
                        tempPartnerDTO.Description = dr["description"].ToString();
                        tempPartnerDTO.IsActive = Convert.ToBoolean(dr["active"]);
                        retVal.Add(tempPartnerDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Unable to get partners", ex);
            }
            return retVal;
        }
        
        public List<GroupDTO> GetGroups()
        {
            List<GroupDTO> retVal = new List<GroupDTO>();
            try
            {
                DataSet ds = new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).GetGroups();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        GroupDTO tempGroupDTO = new GroupDTO();
                        tempGroupDTO.GroupId = Convert.ToInt32(dr["group_id"].ToString());
                        tempGroupDTO.Name = dr["name"].ToString();
                        tempGroupDTO.Description = dr["description"].ToString();
                        tempGroupDTO.IsActive = Convert.ToBoolean(dr["active"]);
                        retVal.Add(tempGroupDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Unable to get partners", ex);
            }
            return retVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public List<UserDTO> GetUsers(string searchTerm)
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
                            //ManufactureId = DBNull.Value.Equals(dr["manufacture_id"]) ? -1 : int.Parse(dr["manufacture_id"].ToString()),
                            //JobTitle = dr["job_title"].ToString(),
                            //BusinessPhone = dr["business_phone"].ToString(),
                            //HomePhone = dr["home_phone"].ToString(),
                            //MobilePhone = dr["mobile_phone"].ToString(),
                            //FAX = dr["fax_number"].ToString(),
                            //AddressLine1 = dr["address_line_1"].ToString(),
                            //AddressLine2 = dr["address_line_2"].ToString(),
                            //City = dr["city"].ToString(),
                            //State = dr["state_province"].ToString(),
                            //ZIP = dr["zip_postal_code"].ToString(),
                            //Country = dr["country"].ToString(),
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
        /// <param name="partnerId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public string ActDactPartner(string partnerId, bool isActive)
        {
            string retVal = "";

            try
            {
                new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).ActDactPartner(partnerId, isActive);
                retVal = "SUCCESS";
            }
            catch (Exception ex)
            {
                log.Error("Unable to Actctivate/Dactivate Partner for partnerId: " + partnerId, ex);
                retVal = "ERROR";
            }
            return retVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        public string AddPartner(PartnerDTO partner)
        {
            string retVal = "";

            try
            {
                new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).AddPartner(partner);
                retVal = "SUCCESS";
            }
            catch (Exception ex)
            {
                log.Error("Unable to Save Partner for partnerId: " + partner.partnerId, ex);
                retVal = "ERROR";
            }
            return retVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public string ActDactUser(int userId, bool isActive)
        {
            string retVal = "";

            try
            {
                new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).ActDactUser(userId, isActive);
                retVal = "SUCCESS";
            }
            catch (Exception ex)
            {
                log.Error("Unable to Actctivate/Dactivate user for userId: " + userId, ex);
                retVal = "ERROR";
            }
            return retVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<UserRoleDTO> GetUserRoles(int userId)
        {
            List<UserRoleDTO> retVal = new List<UserRoleDTO>();
            try
            {
                DataSet ds = new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).GetUserRoles(userId);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        UserRoleDTO tempUserRole = new UserRoleDTO();
                        tempUserRole.UserId = userId;
                        tempUserRole.GroupId = Convert.ToInt32(dr["group_id"].ToString());
                        tempUserRole.GroupName = dr["GroupName"].ToString();
                        tempUserRole.PartnerId = dr["partner_id"].ToString();
                        tempUserRole.PartnerName = dr["PartnerName"].ToString();
                        retVal.Add(tempUserRole);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Unable to Actctivate/Dactivate user for userId: " + userId, ex);
            }
            return retVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="partnerId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public string DeleteUserRole(int userId, string partnerId, int groupId)
        {
            string retVal = "";

            try
            {
                new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).DeleteUserRole(userId, partnerId, groupId);
                retVal = "SUCCESS";
            }
            catch (Exception ex)
            {
                log.Error("Unable to delete UserRole for userId: " + userId + " partnerId: " + partnerId + " groupId: " + groupId, ex);
                retVal = "ERROR";
            }
            return retVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="partnerId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public string AddUserRole(int userId, string partnerId, int groupId)
        {
            string retVal = "";
            try
            {
                new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).AddUserRole(userId, partnerId, groupId);
                retVal = "SUCCESS";
            }
            catch (Exception ex)
            {
                log.Error("Unable to delete UserRole for userId: " + userId + " partnerId: " + partnerId + " groupId: " + groupId, ex);
                retVal = "ERROR";
            }
            return retVal;
        }



        public List<CompanyDetailsDTO> GetCompanies(string searchTerm)
        {
            List<CompanyDetailsDTO> retVal = new List<CompanyDetailsDTO>();
            retVal.Add(new CompanyDetailsDTO() {
                Name= "ABC Company",
                WebSite = "www.abc.com",
                Address = "ABC Address Line 1",
                Address2 = "ABC Address Line 2",
                City = "ABC City",
                State= "ABC State",
                Country = "ABC Country",
                PostalCode="783458",
                DefaultIndustry="Publishing"
            });
            retVal.Add(new CompanyDetailsDTO()
            {
                Name = "XYZ Company",
                WebSite = "www.xyz.com",
                Address = "XYZ Address Line 1",
                City = "XYZ City",
                State = "XYZ State",
                Country = "XYZ Country",
                PostalCode = "783458",
                DefaultIndustry = "Publishing"
            });

            return retVal;
        }
    }
}
