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

        public int AddProject(XElement projectXmlData, string sourceClient = "n/a")
        {
            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                 new SqlParameter("@P_XML", 
                    new SqlXml(new MemoryStream(Encoding.UTF8.GetBytes(projectXmlData.ToString()
                    .Replace(@" xmlns:i=""http://www.w3.org/2001/XMLSchema-instance""", "")
                    .Replace(@" xmlns=""http://www.gpd.com""", "")
                    .Replace(@" i:nil=""true""", ""))))
                 ),
                 new SqlParameter("@P_SOURCE_CLIENT", sourceClient),
                 new SqlParameter("@P_Return_ErrorCode", SqlDbType.Int) {Direction = ParameterDirection.Output },
                 new SqlParameter("@P_Return_Message", SqlDbType.VarChar, 1024) {Direction = ParameterDirection.Output },
                 new SqlParameter("ReturnValue", SqlDbType.Int) {Direction = ParameterDirection.ReturnValue }
            };

            // execute AddProject stored procedure
            return Convert.ToInt32(base.ExecuteStoreProcedure("gpd_AddProject", parametersInList));
        }
    }
}