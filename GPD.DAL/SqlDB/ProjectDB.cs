using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;
using GPD.ServiceEntities.BaseEntities;

namespace GPD.DAL.SqlDB
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectDB : SqlDbBaseManager
    {
        #region Constr
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public ProjectDB(string connectionString) : base(connectionString) { }
        #endregion Constr

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partnerName"></param>
        /// <param name="projectXmlData"></param>
        public void AddProject(string partnerName, string projectId, XDocument projectXmlData)
        {
            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                new SqlParameter("@P_PartnerName", partnerName),
                new SqlParameter("@P_ProjectId", projectId),
                new SqlParameter("@P_XML", projectXmlData.ToString()),
                new SqlParameter("@P_Return_ErrorCode", SqlDbType.Int) { Direction = ParameterDirection.Output },
                new SqlParameter("@P_Return_Message", SqlDbType.VarChar, 1024) { Direction = ParameterDirection.Output }
            };

            Dictionary<string, object> retVal = base.ExecuteStoreProcedure("gpd_AddProject", parametersInList);

            if (retVal == null)
            {
                throw new Exception("Unhandled Exception");
            }
            else if (Convert.ToInt32(retVal["@P_Return_ErrorCode"]) != 0)
            {
                throw new Exception(retVal["@P_Return_Message"].ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public DataSet AuthenticateUser(string email, string password)
        {
            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                 new SqlParameter("@P_EMAIL", email),
                 new SqlParameter("@P_PASSWORD", password)
            };


            return base.GetDSBasedOnStoreProcedure("gpd_UserAuthenticate", parametersInList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataSet GetProjectsList(string partnerName, int pageSize, int pageIndex)
        {
            // start row index logic
            int startRowIndex = (pageIndex == 1) ? 0 : ((pageIndex - 1) * pageSize);

            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                new SqlParameter("@P_PartnerName", partnerName),
                new SqlParameter("@P_StartRowIndex", startRowIndex),
                new SqlParameter("@P_PageSize", pageSize)
            };

            return base.GetDSBasedOnStoreProcedure("gpd_GetProjectsListPaginated", parametersInList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataSet GetProjectById(string id)
        {
            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                 new SqlParameter("@P_PROJECTID", id)
            };

            return base.GetDSBasedOnStoreProcedure("gpd_GetProjectById", parametersInList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partnerName"></param>
        /// <param name="projectXmlData"></param>
        public int AddUserDetails(XDocument projectXmlData, string requestIpAddress, out int errorCode, out string errorMsg)
        {
            int userId = -1;

            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                new SqlParameter("@P_XML", projectXmlData.ToString()),
                new SqlParameter("@P_IpAddress", requestIpAddress),
                new SqlParameter("@P_Return_UserId", SqlDbType.Int) { Direction = ParameterDirection.Output },
                new SqlParameter("@P_Return_ErrorCode", SqlDbType.Int) { Direction = ParameterDirection.Output },
                new SqlParameter("@P_Return_Message", SqlDbType.VarChar, 1024) { Direction = ParameterDirection.Output }
            };

            Dictionary<string, object> retVal = base.ExecuteStoreProcedure("gpd_AddUserDetails", parametersInList);

            if (retVal == null)
                throw new Exception("Unhandled Exception");

            userId = (int)retVal["@P_Return_UserId"];
            errorCode = (int)retVal["@P_Return_ErrorCode"];
            errorMsg = retVal["@P_Return_Message"].ToString();

            return userId;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public DataSet GetUserRole(string email)
        {
            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                 new SqlParameter("@P_EMAIL", email)
            };

            return base.GetDSBasedOnStoreProcedure("gpd_GetUserRole", parametersInList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet GetPartners()
        {
            List<SqlParameter> parametersInList = new List<SqlParameter>() { };

            return base.GetDSBasedOnStoreProcedure("gpd_GetPartners", parametersInList);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public DataSet GetUsers(string searchTerm)
        {
            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                 new SqlParameter("@P_SEARCH", searchTerm)
            };

            return base.GetDSBasedOnStoreProcedure("gpd_GetUsers", parametersInList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partnerId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public void ActDactPartner(string partnerId, bool isActive)
        {
            StringBuilder sb = new StringBuilder("");
            #region SQL
            sb.AppendLine(@"
BEGIN
	DECLARE @M_PartnerId VARCHAR(50), @M_IsActive BIT;

	SET @M_PartnerId = @P_PartnerId;
	SET @M_IsActive = @P_IsActive;

	UPDATE gpd_partner_details
	SET active = @M_IsActive,        
		update_date = GETDATE()
	WHERE partner_id = @M_PartnerId;
END;
");
            #endregion 

            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                 new SqlParameter("@P_PartnerId", partnerId),
                 new SqlParameter("@P_IsActive", isActive)
            };
            base.ExecuteStatement(sb, parametersInList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partner"></param>
        public void SavePartner(PartnerDTO partner)
        {
            StringBuilder sb = new StringBuilder("");
            #region SQL
            if (!string.IsNullOrWhiteSpace(partner.partnerId))
            {
                #region Update
                sb.AppendLine(@"
BEGIN
	DECLARE @M_PartnerId VARCHAR(50),
	    @M_Name NVARCHAR(30),
		@M_URL NVARCHAR(300), 
		@M_ShortDescription NVARCHAR(150), 
		@M_Description NVARCHAR(1000), 
		@M_IsActive BIT;

	SET @M_PartnerId = @P_PartnerId;
	SET @M_Name = @P_Name;
	SET @M_URL = @P_URL;
	SET @M_ShortDescription = @P_ShortDescription;
	SET @M_Description = @P_Description;
	SET @M_IsActive = @P_IsActive;
	

	UPDATE gpd_partner_details
	SET name = @M_Name,
		site_url = @M_URL,
		short_description = @M_ShortDescription,
		description = @M_Description,
		active = @M_IsActive,
		update_date = GETDATE()		
	WHERE partner_id = @M_PartnerId;
END;
");
                #endregion
            }
            else
            {
                #region Insert
                sb.AppendLine(@"
BEGIN
	DECLARE @M_PartnerId VARCHAR(50),
	    @M_Name NVARCHAR(30),
		@M_URL NVARCHAR(300), 
		@M_ShortDescription NVARCHAR(150), 
		@M_Description NVARCHAR(1000), 
		@M_IsActive BIT;

	SET @M_PartnerId = @P_PartnerId;
	SET @M_Name = @P_Name;
	SET @M_URL = @P_URL;
	SET @M_ShortDescription = @P_ShortDescription;
	SET @M_Description = @P_Description;
	SET @M_IsActive = @P_IsActive;
	
    INSERT INTO gpd_partner_details (partner_id, name, site_url, short_description, description, active, xml_partner_metadata, create_date, update_date)
	VALUES(NEWID(), @M_Name,  @M_URL, @M_ShortDescription, @M_Description, 1, NULL, getdate(), NULL);
END;
");
                #endregion
            }
            #endregion 

            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                 new SqlParameter("@P_PartnerId", partner.partnerId),
                 new SqlParameter("@P_Name", partner.Name),
                 new SqlParameter("@P_URL", partner.URL),
                 new SqlParameter("@P_ShortDescription", partner.ShortDescription),
                 new SqlParameter("@P_Description", partner.Description),
                 new SqlParameter("@P_IsActive", partner.IsActive)
            };
            base.ExecuteStatement(sb, parametersInList);
        }
    }
}