using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GPD.DAL.SqlDB
{
    public class UserDB : SqlDbBaseManager
    {
        #region Constr
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public UserDB(string connectionString) : base(connectionString) { }
        #endregion Constr

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firmNameTerm"></param>
        /// <returns>DataSet</returns>
        public DataSet GetFirmsListBasedOnTerm(string firmNameTerm)
        {
            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                new SqlParameter("@P_FIRM_NAME_TERM", firmNameTerm)
            };

            return base.GetDSBasedOnStoreProcedure("gpd_GetFirmsListBasedOnTerm", parametersInList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firmId"></param>
        /// <returns>DataSet</returns>
        public DataSet GetFirmProfile(int firmId)
        {
            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                new SqlParameter("@P_FIRM_ID", firmId)
            };

            return base.GetDSBasedOnStoreProcedure("gpd_GetFirmProfile", parametersInList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>DataSet</returns>
        public DataSet GetUserFullProfile(int userId)
        {
            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                new SqlParameter("@P_USER_ID", userId)
            };

            return base.GetDSBasedOnStoreProcedure("gpd_GetUserFullProfile", parametersInList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="userPassword"></param>
        /// <returns>DataSet</returns>
        public DataSet UpdateUserPassword(string userEmail, string userPassword)
        {
            List<SqlParameter> parametersInList = new List<SqlParameter>()
            {
                new SqlParameter("@P_USER_EMAIL", userEmail),
                new SqlParameter("@P_USER_PASSWORD", userPassword),
            };

            // call stored procedure
            return base.GetDSBasedOnStoreProcedure("gpd_UpdateUserPassword", parametersInList);
        }
    }
}
