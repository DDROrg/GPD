using System.Data.Entity;

namespace GPD.Facade
{
    public class GpdDbContext : DbContext
    {
        public GpdDbContext() : base("GPD_CONNECTION")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<GpdDbContext, GPD.Facade.Migrations.Configuration>("GPD_CONNECTION"));
        }
    }
}