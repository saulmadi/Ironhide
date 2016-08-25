using System.Data.Entity.Migrations;

namespace Ironhide.Login.Data.Migrations
{
    sealed class MigrationsConfiguration : DbMigrationsConfiguration<LoginDataContext>
    {
        public MigrationsConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
        }
    }
}