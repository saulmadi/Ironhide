using System.Data.Entity;
using Ironhide.Users.Data;

namespace DatabaseDeployer
{
    public class DatabaseUpdater : CreateDatabaseIfNotExists<LoginDataContext>
    {
        protected override void Seed(LoginDataContext context)
        {
            base.Seed(context);
        }
    }
}