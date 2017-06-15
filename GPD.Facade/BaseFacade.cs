namespace GPD.Facade
{
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
