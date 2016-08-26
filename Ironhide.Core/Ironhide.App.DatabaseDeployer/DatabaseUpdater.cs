using System.Data.Entity;
using Ironhide.App.Data;

namespace DatabaseDeployer
{
    public class DatabaseUpdater : CreateDatabaseIfNotExists<AppDataContext>
    {
        protected override void Seed(AppDataContext context)
        {
            base.Seed(context);
        }
    }
}