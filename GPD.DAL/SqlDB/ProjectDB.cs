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
using System.Diagnostics;

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

        public int AddProject(XDocument projectXmlData, string sourceClient = "n/a")
        {
            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                 new SqlParameter("@P_XML", 
                    new SqlXml(projectXmlData.CreateReader())
                 ),
                 new SqlParameter("@P_SOURCE_CLIENT", sourceClient),
                 new SqlParameter("@P_Return_ErrorCode", SqlDbType.Int) {Direction = ParameterDirection.Output },
                 new SqlParameter("@P_Return_Message", SqlDbType.VarChar, 1024) {Direction = ParameterDirection.Output }
                 //new SqlParameter("ReturnValue", SqlDbType.Int) {Direction = ParameterDirection.ReturnValue }
            };

            // execute AddProject stored procedure
            //return Convert.ToInt32(base.ExecuteStoreProcedure("gpd_AddProject", parametersInList));

            Stopwatch sw = new Stopwatch();
            sw.Start();

            DataSet dataSet = base.GetDSBasedOnStoreProcedure("gpd_AddProject", parametersInList);

            //Stop the Timer
            sw.Stop();
            //Console.WriteLine("Running Time: " + sw.Elapsed);


            return -1;
        }
    }
}