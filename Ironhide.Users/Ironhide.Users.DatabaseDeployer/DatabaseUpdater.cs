using System.Data.Entity;
using Ironhide.Users.Data;

namespace DatabaseDeployer
{
    public class DatabaseUpdater : CreateDatabaseIfNotExists<UserDataContext>
    {
        protected override void Seed(UserDataContext context)
        {
            base.Seed(context);
        }
    }
}