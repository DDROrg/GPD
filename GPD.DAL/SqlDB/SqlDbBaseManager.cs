﻿using System.Collections.Generic;

using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;

namespace GPD.DAL.SqlDB
{
    public class SqlDbBaseManager
    {
        #region Declaration
        //Declare the connection variable
        private string _db_connection;
        #endregion

        #region Constr
        /// <summary>
        /// DBBaseManager Construction method
        /// </summary>
        /// <param name="connectionString"></param>
        public SqlDbBaseManager(string connectionString)
        {
            //get the connection string
            this._db_connection = connectionString;
        }
        #endregion Constr

        internal DataSet GetDSBasedOnStatement(StringBuilder sql, List<SqlParameter> parametersList = null)
        {
            DataSet dataSet = new DataSet();

            using (SqlConnection conn = new SqlConnection(this._db_connection))
            {
                using (SqlCommand cmd = new SqlCommand(sql.ToString(), conn))
                {
                    cmd.CommandTimeout = 1000;
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        conn.Open();

                        if (parametersList != null && parametersList.Count > 0)
                            adapter.SelectCommand.Parameters.AddRange(parametersList.ToArray());

                        adapter.Fill(dataSet);
                    }
                }
                conn.Close();
            }
            return dataSet;
        }

        internal DataSet GetDSBasedOnStoreProcedure(string storedProdName, List<SqlParameter> parametersList)
        {
            DataSet dataSet = new DataSet();

            using (SqlConnection conn = new SqlConnection(this._db_connection))
            {
                using (SqlCommand cmd = new SqlCommand(storedProdName, conn))
                {
                    conn.Open();
                    cmd.CommandTimeout = 1000;
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        if (parametersList != null && parametersList.Count > 0)
                            adapter.SelectCommand.Parameters.AddRange(parametersList.ToArray());

                        adapter.Fill(dataSet);
                    }
                    conn.Close();
                }
            }

            return dataSet;
        }
        
        internal Dictionary<string, object> ExecuteStatement(StringBuilder sql, List<SqlParameter> parametersList)
        {
            Dictionary<string, object> retVal = null;
            using (SqlConnection conn = new SqlConnection(this._db_connection))
            {
                using (SqlCommand cmd = new SqlCommand(sql.ToString(), conn))
                {
                    conn.Open();
                    if (parametersList != null && parametersList.Count > 0)
                        cmd.Parameters.AddRange(parametersList.ToArray());
                    cmd.ExecuteNonQuery();

                    List<string> outParam = parametersList.Where(i => i.Direction != ParameterDirection.Input).Select(i => i.ParameterName).ToList();
                    if (outParam.Count() > 0)
                    {
                        retVal = new Dictionary<string, object>();
                        outParam.ForEach(i => {
                            retVal.Add(i, cmd.Parameters[i].Value);
                        });
                    }
                }

                conn.Close();
            }
            return retVal;
        }

        internal Dictionary<string, object> ExecuteStoreProcedure(string storedProdName, List<SqlParameter> parametersList)
        {
            Dictionary<string, object> retVal = null;
            using (SqlConnection conn = new SqlConnection(this._db_connection))
            {
                using (SqlCommand cmd = new SqlCommand(storedProdName, conn))
                {
                    conn.Open();
                    cmd.CommandTimeout = 1000;
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parametersList != null && parametersList.Count > 0)
                        cmd.Parameters.AddRange(parametersList.ToArray());
                    cmd.ExecuteNonQuery();

                    List<string> outParam = parametersList.Where(i => i.Direction != ParameterDirection.Input).Select(i => i.ParameterName).ToList();
                    if (outParam.Count() > 0)
                    {
                        retVal = new Dictionary<string, object>();
                        outParam.ForEach(i => {
                            retVal.Add(i, cmd.Parameters[i].Value);
                        });
                    }
                }
                conn.Close();
            }
            return retVal;
        }

        internal string GetSingleValueFromQuery(StringBuilder sql, List<SqlParameter> parametersList)
        {
            string retObj = null;

            using (SqlConnection conn = new SqlConnection(this._db_connection))
            {
                using (SqlCommand cmd = new SqlCommand(sql.ToString(), conn))
                {
                    cmd.CommandType = CommandType.Text;
                    conn.Open();

                    if (parametersList != null && parametersList.Count > 0)
                        cmd.Parameters.AddRange(parametersList.ToArray());

                    // ExecuteScalar
                    var dbReturn = cmd.ExecuteScalar();

                    if (dbReturn != System.DBNull.Value)
                        retObj = dbReturn.ToString();
                }

                conn.Close();
            }

            return retObj;
        }

        internal object GetValueFromQuery(StringBuilder sql, List<SqlParameter> parametersList)
        {
            object retObj = null;

            using (SqlConnection conn = new SqlConnection(this._db_connection))
            {
                using (SqlCommand cmd = new SqlCommand(sql.ToString(), conn))
                {
                    cmd.CommandType = CommandType.Text;
                    conn.Open();

                    if (parametersList != null && parametersList.Count > 0)
                        cmd.Parameters.AddRange(parametersList.ToArray());

                    // ExecuteScalar
                    retObj = cmd.ExecuteScalar();
                }

                conn.Close();
            }

            return retObj;
        }
    }
}
