using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

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

        public int AddProject(XElement project)
        {
            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                 new SqlParameter("@P_XML", project),
                 new SqlParameter("@P_ID", SqlDbType.Int) {Direction = ParameterDirection.ReturnValue }
            };
            return Convert.ToInt32(base.ExecuteStoreProcedure("uspAddProject", parametersInList));
        }
    }
}
