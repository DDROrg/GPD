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
    public class SignInFacade
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public SignInResponseDTO PasswordSignIn(string email, string password)
        {
            SignInResponseDTO retVal = new SignInResponseDTO() { SignInStatus = CNST.SignInStatus.Failure };
            try
            {
                // gete project data
                DataSet ds = new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).PasswordSignIn(email, password);
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
