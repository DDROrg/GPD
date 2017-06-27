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
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataSet GetProjectsListWithSearchTerm(string partnerName, string searchTerm, int pageSize, int pageIndex)
        {
            // start row index logic
            int startRowIndex = (pageIndex == 1) ? 0 : ((pageIndex - 1) * pageSize);

            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                new SqlParameter("@P_PartnerName", partnerName),
                new SqlParameter("@P_SearchKeyword", searchTerm),
                new SqlParameter("@P_StartRowIndex", startRowIndex),
                new SqlParameter("@P_PageSize", pageSize)
            };

            return base.GetDSBasedOnStoreProcedure("gpd_GetProjectsListBySearchKeyword", parametersInList);
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
        /// <param name="password"></param>
        /// <returns></returns>
        public DataSet AuthenticateUser(string email)
        {
            StringBuilder sb = new StringBuilder();

            #region SQL
            sb.AppendLine(@"
BEGIN
	DECLARE @M_EMAIL nvarchar(150);

	SET @M_EMAIL = @P_EMAIL;

	SELECT distinct 
		u.user_id, u.password
	FROM gpd_user_details u
	WHERE LOWER(u.email) = LOWER(@M_EMAIL)
		AND u.active = 1;
END;
");
            #endregion 
            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                 new SqlParameter("@P_EMAIL", email)
            };

            return base.GetDSBasedOnStatement(sb, parametersInList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public DataSet GetUserRole(string email)
        {
            StringBuilder sb = new StringBuilder("");
            #region SQL
            sb.AppendLine(@"
BEGIN
	DECLARE @M_EMAIL NVARCHAR(150);

	SET @M_EMAIL = @P_EMAIL;

	SELECT distinct 
		u.user_id, 
		u.first_name, 
		u.last_name, 
		p.partner_id,
		p.name as PartnerName,
		g.group_id,
		g.name as GroupName
	FROM gpd_user_details u
	INNER JOIN gpd_partner_user_group_xref x
		ON u.user_id = x.user_id
			AND LOWER(u.email) = LOWER(@M_EMAIL)
			AND u.active = 1	
	INNER JOIN gpd_partner_details p
		ON x.partner_id = p.partner_id
	INNER JOIN gpd_user_group g
		ON x.group_id = g.group_id
	ORDER BY p.name;
END;
");
            #endregion 

            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                 new SqlParameter("@P_EMAIL", email)
            };

            return base.GetDSBasedOnStatement(sb, parametersInList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet GetPartners()
        {
            StringBuilder sb = new StringBuilder("");
            #region SQL
            sb.AppendLine(@"
BEGIN	
	SELECT 
		p.partner_id,
		p.name,
		p.site_url,
		p.short_description,
		p.description,
		p.active
	FROM gpd_partner_details p
    ORDER BY p.name;
END;
");
            #endregion 
            List<SqlParameter> parametersInList = new List<SqlParameter>() { };
            return base.GetDSBasedOnStatement(sb, parametersInList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet GetGroups()
        {
            StringBuilder sb = new StringBuilder("");
            #region SQL
            sb.AppendLine(@"
BEGIN	
	SELECT group_id, 
	    name,
	    description,
	    active
    FROM gpd_user_group
    ORDER BY name;
END;
");
            #endregion 
            List<SqlParameter> parametersInList = new List<SqlParameter>() { };
            return base.GetDSBasedOnStatement(sb, parametersInList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public DataSet GetUsers(string searchTerm)
        {
            StringBuilder sb = new StringBuilder("");
            #region SQL
            sb.AppendLine(@"
BEGIN
	DECLARE @M_SEARCH nvarchar(150);
	SET @M_SEARCH = @P_SEARCH;
	SELECT 
		u.user_id,
		u.last_name,
		u.first_name,
		u.full_name,
		u.email,
		u.company,
		u.job_title,
		u.business_phone,
		u.home_phone,
		u.mobile_phone,
		u.fax_number,
		u.address_line_1,
		u.address_line_2,
		u.city,
		u.state_province,
		u.zip_postal_code,
		u.country,
		u.active      
	FROM gpd_user_details U
	WHERE @M_SEARCH = '' 
		OR UPPER(u.last_name) LIKE @M_SEARCH 
		OR UPPER(u.first_name) LIKE @M_SEARCH 
		OR UPPER(u.email) LIKE @M_SEARCH;
END;
");
            #endregion 
            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                 new SqlParameter("@P_SEARCH", searchTerm)
            };

            return base.GetDSBasedOnStatement(sb, parametersInList);
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
        public void AddPartner(PartnerDTO partner)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isActive"></param>
        public void ActDactUser(int userId, bool isActive)
        {
            StringBuilder sb = new StringBuilder("");
            #region SQL
            sb.AppendLine(@"
BEGIN
	DECLARE @M_UserId int, @M_IsActive BIT;

	SET @M_UserId = @P_UserId;
	SET @M_IsActive = @P_IsActive;

	UPDATE gpd_user_details
	SET active = @M_IsActive,        
		update_date = GETDATE()
	WHERE user_id = @M_UserId;
END;
");
            #endregion 

            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                 new SqlParameter("@P_UserId", userId),
                 new SqlParameter("@P_IsActive", isActive)
            };
            base.ExecuteStatement(sb, parametersInList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataSet GetUserRoles(int userId)
        {
            StringBuilder sb = new StringBuilder("");
            #region SQL
            sb.AppendLine(@"
BEGIN
	DECLARE @M_USERID INT;

	SET @M_USERID = @P_USERID;

	SELECT distinct 
		u.user_id, 
		p.partner_id,
		p.name as PartnerName,
		g.group_id,
		g.name as GroupName
	FROM gpd_user_details u
	INNER JOIN gpd_partner_user_group_xref x
		ON u.user_id = x.user_id
			AND u.user_id = @M_USERID
	INNER JOIN gpd_partner_details p
		ON x.partner_id = p.partner_id
	INNER JOIN gpd_user_group g
		ON x.group_id = g.group_id;
END;
");
            #endregion 

            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                 new SqlParameter("@P_USERID", userId)
            };

            return base.GetDSBasedOnStatement(sb, parametersInList);
        }

    }
}