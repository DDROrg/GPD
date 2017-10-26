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
        public void AddProject(string partnerName, int userId, string projectId, XDocument projectXmlData)
        {
            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                new SqlParameter("@P_PartnerName", partnerName),
                new SqlParameter("@P_UserId", userId),
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
        /// <param name="projectId"></param>
        /// <param name="projectXmlData"></param>
        public void UpdateProject(string projectId, XDocument projectXmlData)
        {
            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                new SqlParameter("@P_ProjectId", projectId),
                new SqlParameter("@P_XML", projectXmlData.ToString()),
                new SqlParameter("@P_Return_ErrorCode", SqlDbType.Int) { Direction = ParameterDirection.Output },
                new SqlParameter("@P_Return_Message", SqlDbType.VarChar, 1024) { Direction = ParameterDirection.Output }
            };

            Dictionary<string, object> retVal = base.ExecuteStoreProcedure("gpd_UpdateProject", parametersInList);

            if (retVal == null)
            {
                throw new Exception("Unhandled Exception");
            }
            else if (Convert.ToInt32(retVal["@P_Return_ErrorCode"]) != 0)
            {
                throw new Exception(retVal["@P_Return_Message"].ToString());
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //public DataSet GetProjectsList(string partnerName, int pageSize, int pageIndex, string fromDate, string toDate)
        //{
        //    // start row index logic
        //    int startRowIndex = (pageIndex == 1) ? 0 : ((pageIndex - 1) * pageSize);

        //    List<SqlParameter> parametersInList = new List<SqlParameter>()
        //    {
        //        new SqlParameter("@P_PartnerName", partnerName),
        //        new SqlParameter("@P_StartRowIndex", startRowIndex),
        //        new SqlParameter("@P_PageSize", pageSize)
        //    };

        //    return base.GetDSBasedOnStoreProcedure("gpd_GetProjectsListPaginated", parametersInList);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="partnerName"></param>
        ///// <param name="searchTerm"></param>
        ///// <param name="pIdentifier"></param>
        ///// <param name="pageSize"></param>
        ///// <param name="pageIndex"></param>
        ///// <returns></returns>
        //public DataSet GetProjectsListWithSearchTerm(string partnerName, string searchTerm, string pIdentifier, string fromDate, string toDate, int pageSize, int pageIndex)
        //{
        //    // start row index logic
        //    int startRowIndex = (pageIndex == 1) ? 0 : ((pageIndex - 1) * pageSize);

        //    List<SqlParameter> parametersInList = new List<SqlParameter>()
        //    {
        //        new SqlParameter("@P_PartnerName", partnerName),
        //        (searchTerm == null) ? new SqlParameter("@P_SearchKeyword", DBNull.Value) : new SqlParameter("@P_SearchKeyword", searchTerm),
        //        (pIdentifier == null) ? new SqlParameter("@P_ProjectIdentifier", DBNull.Value) : new SqlParameter("@P_ProjectIdentifier", pIdentifier),
        //        new SqlParameter("@P_StartRowIndex", startRowIndex),
        //        new SqlParameter("@P_PageSize", pageSize)
        //    };

        //    return base.GetDSBasedOnStoreProcedure("gpd_GetProjectsListBySearchTerm", parametersInList);
        //}

        /// <summary>
        /// Get Projects List
        /// </summary>
        /// <param name="partnerName"></param>
        /// <param name="userId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="searchTerm"></param>
        /// <param name="projectIdentifier"></param>
        /// <returns></returns>
        public DataSet GetProjectsList(string partnerName, int userId, string fromDate, string toDate,
            string searchTerm, string projectIdentifier, int pageSize, int pageIndex)
        {
            // start row index logic
            int startRowIndex = (pageIndex == 1) ? 0 : ((pageIndex - 1) * pageSize);

            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                new SqlParameter("@P_PartnerName", partnerName),
                new SqlParameter("@P_UserId", userId),
                new SqlParameter("@P_FromDate", fromDate),
                new SqlParameter("@P_ToDate", toDate),
                string.IsNullOrEmpty(searchTerm) ? new SqlParameter("@P_SearchTerm", DBNull.Value) : new SqlParameter("@P_SearchTerm", searchTerm),
                string.IsNullOrEmpty(projectIdentifier) ? new SqlParameter("@P_ProjectIdentifier", DBNull.Value) : new SqlParameter("@P_ProjectIdentifier", projectIdentifier),
                new SqlParameter("@P_StartRowIndex", startRowIndex),
                new SqlParameter("@P_PageSize", pageSize)
            };

            return base.GetDSBasedOnStoreProcedure("gpd_GetProjectsList", parametersInList);
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
        public int AddUserDetails(XDocument xDoc, string requestIpAddress, out int errorCode, out string errorMsg)
        {
            int userId = -1;

            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                new SqlParameter("@P_XML", xDoc.ToString()),
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
        public DataSet GetUserRole(int userId)
        {
            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                 new SqlParameter("@P_USER_ID", userId)
            };

            return base.GetDSBasedOnStoreProcedure("gpd_GetUserRoles", parametersInList);
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
    ORDER BY group_id;
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
            return base.GetDSBasedOnStoreProcedure("gpd_GetUsersList",
                new List<SqlParameter>()
                {
                    new SqlParameter("@P_SEARCH_VALUE", searchTerm ?? string.Empty)
                });
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
	SELECT distinct 
		u.user_id, 
		p.partner_id,
		p.name as PartnerName,
		g.group_id,
		g.name as GroupName
	FROM gpd_user_details u
	INNER JOIN gpd_partner_user_group_xref x
		ON u.user_id = x.user_id
			AND u.user_id = @P_USERID
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="partnerId"></param>
        /// <param name="groupId"></param>
        public void DeleteUserRole(int userId, string partnerId, int groupId)
        {
            StringBuilder sb = new StringBuilder("");
            #region SQL

            sb.AppendLine(@"
BEGIN
	DECLARE @M_PartnerId UNIQUEIDENTIFIER;

	SET @M_PartnerId = CONVERT(UNIQUEIDENTIFIER, @P_PartnerId);
	
	DELETE gpd_partner_user_group_xref
	WHERE user_id = @P_UserId AND partner_id = @M_PartnerId AND group_id = @P_GroupId;
END;
");
            #endregion

            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                 new SqlParameter("@P_UserId", userId),
                 new SqlParameter("@P_PartnerId", partnerId),
                 new SqlParameter("@P_GroupId", groupId)
            };
            base.ExecuteStatement(sb, parametersInList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="partnerId"></param>
        /// <param name="groupId"></param>
        public void AddUserRole(int userId, string partnerId, int groupId)
        {
            StringBuilder sb = new StringBuilder("");
            #region SQL

            sb.AppendLine(@"
BEGIN
	DECLARE @M_PartnerId UNIQUEIDENTIFIER;

	SET @M_PartnerId = CONVERT(UNIQUEIDENTIFIER, @P_PartnerId);

    IF NOT EXISTS ( SELECT * FROM gpd_partner_user_group_xref WHERE user_id = @P_UserId AND partner_id = @M_PartnerId AND group_id = @P_GroupId)
    BEGIN
	    INSERT INTO gpd_partner_user_group_xref (partner_id, user_id, group_id, description, active, create_date, update_date)
        VALUES (@M_PartnerId, @P_UserId, @P_GroupId, NULL, 1, getdate(), NULL);
    END
END;
");
            #endregion

            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                 new SqlParameter("@P_PartnerId", partnerId),
                 new SqlParameter("@P_UserId", userId),
                 new SqlParameter("@P_GroupId", groupId)
            };
            base.ExecuteStatement(sb, parametersInList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectListXmlData"></param>
        /// <param name="isActive"></param>
        public void ActivateProjectList(XDocument projectListXmlData, bool isActive)
        {
            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                new SqlParameter("@P_Is_Active", isActive),
                new SqlParameter("@P_XML", projectListXmlData.ToString()),
                new SqlParameter("@P_Return_ErrorCode", SqlDbType.Int) { Direction = ParameterDirection.Output },
                new SqlParameter("@P_Return_Message", SqlDbType.VarChar, 1024) { Direction = ParameterDirection.Output }
            };

            Dictionary<string, object> retVal = base.ExecuteStoreProcedure("gpd_ActivateProjectList", parametersInList);

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
        /// <param name="projectListXmlData"></param>
        public void DeleteProjectList(XDocument projectListXmlData, bool deleteFlag)
        {
            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                new SqlParameter("@P_XML", projectListXmlData.ToString()),
                new SqlParameter("@P_DeleteFlag", deleteFlag),
                new SqlParameter("@P_Return_ErrorCode", SqlDbType.Int) { Direction = ParameterDirection.Output },
                new SqlParameter("@P_Return_Message", SqlDbType.VarChar, 1024) { Direction = ParameterDirection.Output }
            };

            Dictionary<string, object> retVal = base.ExecuteStoreProcedure("gpd_DeleteProjectList", parametersInList);

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
        /// <param name="xDoc"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorMsg"></param>
        public void UpdateUserProfile(int userId, XDocument xDoc, out int errorCode, out string errorMsg)
        {
            errorCode = 0;
            errorMsg = string.Empty;

            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                new SqlParameter("@P_UserId", userId),
                new SqlParameter("@P_XML", xDoc.ToString()),
                new SqlParameter("@P_Return_ErrorCode", SqlDbType.Int) { Direction = ParameterDirection.Output },
                new SqlParameter("@P_Return_Message", SqlDbType.VarChar, 1024) { Direction = ParameterDirection.Output }
            };

            Dictionary<string, object> retVal = base.ExecuteStoreProcedure("gpd_UpdateUserProfile", parametersInList);

            if (retVal == null)
            {
                errorCode = -1;
                errorMsg = "Unhandled Exception";
            }
            else if (Convert.ToInt32(retVal["@P_Return_ErrorCode"]) != 0)
            {
                errorCode = Convert.ToInt32(retVal["@P_Return_ErrorCode"]);
                errorMsg = retVal["@P_Return_Message"].ToString();
            }
        }


        #region For Reporting

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partner"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public DataSet GetProjectCount(string partner)
        {
            StringBuilder sb = new StringBuilder("");
            #region SQL
            sb.AppendLine(@"
BEGIN
	DECLARE @M_PartnerName nvarchar(30);

	SET @M_PartnerName = @P_PartnerName;

	SELECT COUNT(P.project_id) AS P_COUNT
	FROM gpd_project P
	WHERE P.deleted = 0
		AND (@M_PartnerName = 'ALL' OR P.partner_name = @M_PartnerName);
END;
");
            #endregion 

            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                 new SqlParameter("@P_PartnerName", partner)
            };

            return base.GetDSBasedOnStatement(sb, parametersInList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        public DataSet GetUniqueUserCount(string partner)
        {
            StringBuilder sb = new StringBuilder("");
            #region SQL
            sb.AppendLine(@"
BEGIN
	DECLARE @M_PartnerName nvarchar(30);
	SET @M_PartnerName = @P_PartnerName;

	SELECT COUNT(DISTINCT U.user_id) AS U_COUNT
	FROM gpd_project P
	JOIN gpd_project_user_xref X
		ON P.project_id = X.project_id
		AND P.deleted = 0
		AND (@M_PartnerName = 'ALL' OR P.partner_name = @M_PartnerName)
	JOIN gpd_user_details U
		ON X.user_id = U.user_id
		AND U.active = 1;
END;
");
            #endregion 

            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                 new SqlParameter("@P_PartnerName", partner)
            };

            return base.GetDSBasedOnStatement(sb, parametersInList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        public DataSet GetBPMCount(string partner)
        {
            StringBuilder sb = new StringBuilder("");
            #region SQL
            sb.AppendLine(@"
BEGIN
	DECLARE @M_PartnerName nvarchar(30);
	SET @M_PartnerName = @P_PartnerName;

	SELECT COUNT(DISTINCT u.user_id) AS U_COUNT
	FROM gpd_user_details u 
	JOIN gpd_partner_user_group_xref x 
		ON u.user_id = x.user_id
		AND u.active = 1
		AND x.active = 1
	JOIN gpd_partner_details p
		ON x.partner_id = p.partner_id
		AND (@M_PartnerName = 'ALL' OR P.name = @M_PartnerName)
		AND p.active = 1
	WHERE p.name = @M_PartnerName;
END;
");
            #endregion 

            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                 new SqlParameter("@P_PartnerName", partner)
            };

            return base.GetDSBasedOnStatement(sb, parametersInList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet GetPartnerCount()
        {
            StringBuilder sb = new StringBuilder("");
            #region SQL
            sb.AppendLine(@"
BEGIN
	SELECT COUNT(P.partner_id) AS P_COUNT
	FROM gpd_partner_details P
	WHERE P.active = 1 AND P.name != 'ALL';
END
");
            #endregion 

            List<SqlParameter> parametersInList = new List<SqlParameter>() { };

            return base.GetDSBasedOnStatement(sb, parametersInList);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="partner"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public DataSet GetProjectChartData(string partner, string fromDate, string toDate)
        {
            StringBuilder sb = new StringBuilder("");
            #region SQL
            sb.AppendLine(@"
BEGIN
	DECLARE @M_PartnerName nvarchar(30),
		@M_FromDate DATETIME,
		@M_ToDate DATETIME;

	SET @M_PartnerName = @P_PartnerName;
	SET @M_FromDate = CONVERT(DATETIME, @P_FromDate, 102);
	SET @M_ToDate = DATEADD(day, 1, CONVERT(DATETIME, @P_ToDate, 102));

	SELECT  
		T1.application_type AS APP_TYPE, 
        T1.create_date AS C_DATE,
		COUNT(T1.project_id) AS P_COUNT
	FROM (
		SELECT P.project_id, 
			PS.application_type, 
			CAST(p.create_date AS DATE) AS create_date
		FROM gpd_project P
		JOIN gpd_project_session PS
			ON p.project_id = PS.project_id
			AND P.deleted = 0
			AND P.create_date BETWEEN @M_FromDate AND @M_ToDate
			AND (@M_PartnerName = 'ALL' OR P.partner_name = @M_PartnerName)
		) T1
	GROUP BY T1.application_type, T1.create_date
	ORDER BY APP_TYPE, C_DATE;
END;
");
            #endregion 

            List<SqlParameter> parametersInList = new List<SqlParameter>() {
                 new SqlParameter("@P_PartnerName", partner),
                 new SqlParameter("@P_FromDate", fromDate),
                 new SqlParameter("@P_ToDate", toDate),
            };
            
            return base.GetDSBasedOnStatement(sb, parametersInList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        public DataSet GetTopProductChartData(string partner)
        {
            StringBuilder sb = new StringBuilder("");
            #region SQL
            sb.AppendLine(@"
BEGIN
	DECLARE @M_PartnerName nvarchar(30);
	SET @M_PartnerName = @P_PartnerName;

	SELECT TOP 5 
		T1.product_name AS P_NAME, 
		COUNT(T1.project_id) AS P_COUNT
	FROM (
		SELECT P.project_id, 
			IM.product_name
		FROM gpd_project P
		JOIN gpd_project_item I
			ON p.project_id = I.project_id
			AND P.deleted = 0
			AND (@M_PartnerName = 'ALL' OR P.partner_name = @M_PartnerName)
		JOIN gpd_project_item_material IM
			ON I.project_item_id = IM.project_item_id
		) T1
	GROUP BY T1.product_name
	ORDER BY P_COUNT DESC;
END;
");
            #endregion 

            List<SqlParameter> parametersInList = new List<SqlParameter>() {
                 new SqlParameter("@P_PartnerName", partner)
            };

            return base.GetDSBasedOnStatement(sb, parametersInList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        public DataSet GetAppChartData(string partner)
        {
            StringBuilder sb = new StringBuilder("");
            #region SQL
            sb.AppendLine(@"
BEGIN
	DECLARE @M_PartnerName nvarchar(30);
	SET @M_PartnerName = @P_PartnerName;

	SELECT 
		T1.application_name AS A_NAME, 
		COUNT(T1.project_id) AS P_COUNT
	FROM (
		SELECT  DISTINCT
			P.project_id, 
			PS.application_name
		FROM gpd_project P
		JOIN gpd_project_session PS
			ON p.project_id = PS.project_id
			AND P.deleted = 0
			AND (@M_PartnerName = 'ALL' OR P.partner_name = @M_PartnerName)
		) T1
	GROUP BY T1.application_name
	ORDER BY P_COUNT DESC;
END;
");
            #endregion 

            List<SqlParameter> parametersInList = new List<SqlParameter>() {
                 new SqlParameter("@P_PartnerName", partner)
            };

            return base.GetDSBasedOnStatement(sb, parametersInList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        public DataSet GetTopCustomerChartData(string partner)
        {
            StringBuilder sb = new StringBuilder("");
            #region SQL
            sb.AppendLine(@"
BEGIN
	DECLARE @M_PartnerName nvarchar(30);
	SET @M_PartnerName = @P_PartnerName;

	SELECT TOP 5 
		T1.product_manufacturer AS M_NAME, 
		COUNT(T1.project_id) AS P_COUNT
	FROM (
		SELECT DISTINCT 
			P.project_id, 
			I.product_manufacturer
		FROM gpd_project P
		JOIN gpd_project_item I
			ON p.project_id = I.project_id
			AND P.deleted = 0
			AND (@M_PartnerName = 'ALL' OR P.partner_name = @M_PartnerName)
			AND ISNULL(I.product_manufacturer,'') != ''
		) T1
	GROUP BY T1.product_manufacturer
	ORDER BY P_COUNT DESC;
END;
");
            #endregion 

            List<SqlParameter> parametersInList = new List<SqlParameter>() {
                 new SqlParameter("@P_PartnerName", partner)
            };

            return base.GetDSBasedOnStatement(sb, parametersInList);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        public DataSet GetPctProjectWithManufacturer(string partner)
        {
            StringBuilder sb = new StringBuilder("");
            #region SQL
            sb.AppendLine(@"
BEGIN
	DECLARE @M_PartnerName nvarchar(30);
	SET @M_PartnerName = @P_PartnerName;

	SELECT 
	     SUM(IIF(T1.HAS_MFG = 'N', 0, 1)) AS HAS_MFG,
		 SUM(IIF(T1.HAS_MFG = 'N', 1, 0)) AS NO_MFG
	FROM (
		SELECT P.project_id,
			IIF ( ISNULL(P.organization_name,'') = '' , 'N', 'Y' ) AS HAS_MFG
		FROM gpd_project P
		WHERE P.deleted = 0 
			AND (@M_PartnerName = 'ALL' OR P.partner_name = @M_PartnerName)
		) T1;
END;
");
            #endregion 

            List<SqlParameter> parametersInList = new List<SqlParameter>() {
                 new SqlParameter("@P_PartnerName", partner)
            };

            return base.GetDSBasedOnStatement(sb, parametersInList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        public DataSet GetPctProjectWithProductTAG(string partner)
        {
            StringBuilder sb = new StringBuilder("");
            #region SQL
            sb.AppendLine(@"
BEGIN
	DECLARE @M_PartnerName nvarchar(30);

	SET @M_PartnerName = @P_PartnerName;
	
	SELECT SUM(IIF(T2.HAS_URL=0,1,0)) AS NO_URL,
		SUM(IIF(T2.HAS_URL=1,1,0)) AS HAS_URL
	FROM (
		SELECT T1.project_id, SUM(I_URL) AS HAS_URL
		FROM (
			SELECT DISTINCT
				P.project_id, 
				IIF(ISNULL(I.product_image_url,'') = '', 0, 1) AS I_URL
			FROM gpd_project P
			JOIN gpd_project_item I
				ON p.project_id = I.project_id
				AND P.deleted = 0
				AND (@M_PartnerName = 'ALL' OR P.partner_name = @M_PartnerName)
			) T1
		GROUP BY T1.project_id
	) T2;
END;
");
            #endregion 

            List<SqlParameter> parametersInList = new List<SqlParameter>() {
                 new SqlParameter("@P_PartnerName", partner)
            };

            return base.GetDSBasedOnStatement(sb, parametersInList);
        }
        //GetPctProjectWithProductTAG(partner, fromDate, toDate); 
        #endregion
    }
}