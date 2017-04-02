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
        /// <param name="projectXmlData"></param>
        /// <param name="sourceClient"></param>
        public void AddProject(XDocument projectXmlData, string sourceClient = "N/A")
        {
            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                 new SqlParameter("@P_XML", projectXmlData.ToString()),
                 new SqlParameter("@P_SOURCE_CLIENT", sourceClient),                 
                 new SqlParameter("@P_Return_ErrorCode", SqlDbType.Int) {Direction = ParameterDirection.Output },
                 new SqlParameter("@P_Return_Message", SqlDbType.VarChar, 1024) {Direction = ParameterDirection.Output }
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
        public DataSet GetProjects(string userId)
        {
            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                 new SqlParameter("@P_USER", userId)
            };

            return base.GetDSBasedOnStoreProcedure("gpd_GetProjects", parametersInList);
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