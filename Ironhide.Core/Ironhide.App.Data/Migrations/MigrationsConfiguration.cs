using System.Data.Entity.Migrations;

namespace Ironhide.App.Data.Migrations
{
    sealed class MigrationsConfiguration : DbMigrationsConfiguration<AppDataContext>
    {
        public MigrationsConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
        }
    }
}