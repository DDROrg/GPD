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
    public class BaseFacade
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public BaseFacade() {
            using (var db = new GpdDbContext())
            {
                var objectContext = (db as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext;
            }
        }
    }
}
