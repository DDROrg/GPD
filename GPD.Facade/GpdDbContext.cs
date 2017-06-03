using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace GPD.Facade
{
    public class GpdDbContext : DbContext
    {
        public GpdDbContext()
            : base("GPD_CONNECTION")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<GpdDbContext, GPD.Facade.Migrations.Configuration>("GPD_CONNECTION"));
        }
    }
}