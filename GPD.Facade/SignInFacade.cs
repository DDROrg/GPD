using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Xml.XPath;


namespace GPD.Facade
{
    using DAL.SqlDB;
    using ServiceEntities.BaseEntities;
    using CNST = GPD.Utility.ConstantHelper;

    /// <summary>
    /// 
    /// </summary>
    public class SignInFacade : BaseFacade
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SignInFacade() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public int AuthenticateUser(string email, string password)
        {
            int retVal = CNST.SignInStatus.Failure;
            try
            {
                // gete project data
                DataSet ds = new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).AuthenticateUser(email, password);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    retVal = CNST.SignInStatus.Success;
                }
            }
            catch (Exception ex)
            {
                log.Error("Unable to sign in id: " + email, ex);
            }
            return retVal;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public SignInResponseDTO GetUserRole(string email)
        {
            SignInResponseDTO retVal = new SignInResponseDTO() { SignInStatus = CNST.SignInStatus.Failure };
            try
            {
                // gete project data
                DataSet ds = new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).GetUserRole(email);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    retVal.UserId = dr["user_id"].ToString();
                    retVal.FirstName = dr["first_name"].ToString();
                    retVal.LastName = dr["last_name"].ToString();
                    retVal.GroupName = dr["GroupName"].ToString();
                    retVal.SignInStatus = CNST.SignInStatus.Success;
                    foreach (DataRow dr2 in ds.Tables[0].Rows)
                    {
                        retVal.PartnerNames.Add(dr2["PartnerName"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Unable to sign in id: " + email, ex);
            }
            return retVal;
        }
    }
}
