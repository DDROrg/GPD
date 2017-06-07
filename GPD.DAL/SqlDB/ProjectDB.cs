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
    }
}