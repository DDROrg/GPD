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

        /// <summary>
        /// Get dataset of users based on search criteria
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="searchTerm"></param>
        /// <param name="orderByColIndex"></param>
        /// <param name="sortingOrder"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public DataSet GetUsers(DateTime fromDate, DateTime toDate, string searchTerm,
            int orderByColIndex, string sortingOrder, int userGroupId, int startRowIndex, int pageSize)
        {
            return base.GetDSBasedOnStoreProcedure("gpd_GetUsersList_V2",
                new List<SqlParameter>()
                {
                    new SqlParameter("@P_FromDate", fromDate),
                    new SqlParameter("@P_ToDate", toDate),
                    (string.IsNullOrWhiteSpace(searchTerm)) ?
                    new SqlParameter("@P_SearchTerm", DBNull.Value) : new SqlParameter("@P_SearchTerm", searchTerm),
                    new SqlParameter("@P_OrderByColIndex", orderByColIndex),
                    new SqlParameter("@P_SortingOrder", sortingOrder),
                    (userGroupId <= 0) ?
                    new SqlParameter("@P_UserGroupId", DBNull.Value) : new SqlParameter("@P_UserGroupId", userGroupId),
                    new SqlParameter("@P_StartRowIndex", startRowIndex),
                    new SqlParameter("@P_PageSize", pageSize)
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="apiKeyId"></param>
        /// <param name="partnerName"></param>
        /// <returns>DataSet</returns>
        public DataSet GetPartnerListAccessTo(int userId, string apiKeyId, string partnerName)
        {
            return base.GetDSBasedOnStoreProcedure("gpd_GetPartnerListAccessTo",
                new List<SqlParameter>()
                {
                    new SqlParameter("@P_UserId", userId),
                    (string.IsNullOrEmpty(apiKeyId) ? new SqlParameter("@P_ApiKeyId", DBNull.Value) : new SqlParameter("@P_ApiKeyId", apiKeyId)),
                    new SqlParameter("@P_PartnerName", partnerName ?? string.Empty)
                });

        }
    }
}
