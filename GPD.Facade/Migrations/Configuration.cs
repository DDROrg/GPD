namespace GPD.Facade.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GPD.Facade.GpdDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(GPD.Facade.GpdDbContext context)
        {
            //  This method will be called after migrating to the latest version.            
        }
    }
}
