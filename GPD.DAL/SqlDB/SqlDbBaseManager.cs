using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

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

        internal DataSet GetDSBasedOnStatement(StringBuilder sql, List<SqlParameter> parametersList)
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

        internal void ExecuteStatement(StringBuilder sql, List<SqlParameter> parametersList)
        {
            using (SqlConnection conn = new SqlConnection(this._db_connection))
            {
                using (SqlCommand cmd = new SqlCommand(sql.ToString(), conn))
                {
                    conn.Open();
                    if (parametersList != null && parametersList.Count > 0)
                        cmd.Parameters.AddRange(parametersList.ToArray());
                    cmd.ExecuteNonQuery();
                }

                conn.Close();
            }
        }

        internal void ExecuteStoreProcedure(string storedProdName, List<SqlParameter> parametersList)
        {
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
                }
                conn.Close();
            }
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
