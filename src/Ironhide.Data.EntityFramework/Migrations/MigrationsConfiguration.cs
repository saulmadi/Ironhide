using System.Data.Entity.Migrations;

namespace Ironhide.Users.Data.Migrations
{
    sealed class MigrationsConfiguration : DbMigrationsConfiguration<UserDataContext>
    {
        public MigrationsConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
        }
    }
}