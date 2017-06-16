using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Xml.XPath;


namespace GPD.Facade
{
    using DAL.SqlDB;
    using ServiceEntities.BaseEntities;
    using CNST = GPD.Utility.ConstantHelper;

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
        public int AuthenticateUser(string email, string password)
        {
            int retVal = CNST.SignInStatus.Failure;
            try
            {
                // gete project data
                DataSet ds = new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).AuthenticateUser(email, password);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    retVal = CNST.SignInStatus.Success;
                }
            }
            catch (Exception ex)
            {
                log.Error("Unable to sign in id: " + email, ex);
            }
            return retVal;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public SignInResponseDTO GetUserRole(string email)
        {
            SignInResponseDTO retVal = new SignInResponseDTO();
            try
            {
                DataSet ds = new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).GetUserRole(email);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    retVal.UserId = dr["user_id"].ToString();
                    retVal.FirstName = dr["first_name"].ToString();
                    retVal.LastName = dr["last_name"].ToString();
                    retVal.GroupName = dr["GroupName"].ToString();
                    retVal.Email = email.ToLower();
                    foreach (DataRow dr2 in ds.Tables[0].Rows)
                    {
                        retVal.PartnerNames.Add(dr2["PartnerName"].ToString());
                    }
                    retVal.SelectedPartner = retVal.PartnerNames.FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                log.Error("Unable to get user profile for id: " + email, ex);
            }
            return retVal;
        }

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

        public List<UserDTO> GetUsers(string searchTerm)
        {
            List<UserDTO> retVal = new List<UserDTO>();
            searchTerm = string.IsNullOrWhiteSpace(searchTerm) ? string.Empty : "%" + searchTerm.Trim().ToUpper() + "%";
            try
            {
                DataSet ds = new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).GetUsers(searchTerm);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        UserDTO tempUserDto = new UserDTO();
                        tempUserDto.UserId = dr["user_id"].ToString();
                        tempUserDto.FirstName = dr["first_name"].ToString();
                        tempUserDto.LastName = dr["last_name"].ToString();
                        tempUserDto.Email = dr["email"].ToString();
                        tempUserDto.Company = dr["company"].ToString();
                        tempUserDto.JobTitle = dr["job_title"].ToString();
                        tempUserDto.BusinessPhone = dr["business_phone"].ToString();
                        tempUserDto.HomePhone = dr["home_phone"].ToString();
                        tempUserDto.MobilePhone = dr["mobile_phone"].ToString();
                        tempUserDto.FAX = dr["fax_number"].ToString();
                        tempUserDto.AddressLine1 = dr["address_line_1"].ToString();
                        tempUserDto.AddressLine2 = dr["address_line_2"].ToString();
                        tempUserDto.City = dr["city"].ToString();
                        tempUserDto.State = dr["state_province"].ToString();
                        tempUserDto.ZIP = dr["zip_postal_code"].ToString();
                        tempUserDto.Country = dr["country"].ToString();
                        tempUserDto.IsActive = Convert.ToBoolean(dr["active"]);
                        retVal.Add(tempUserDto);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Unable to get users for search term: " + searchTerm, ex);
            }
            return retVal;
        }
    }
}
