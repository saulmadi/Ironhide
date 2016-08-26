using System.Data.Entity.Infrastructure;

namespace Ironhide.App.Data
{
    public class IronhideSamplesDbContextFactory : IDbContextFactory<AppDataContext>
    {
        public AppDataContext Create()
        {
            return new AppDataContext(AppModule.Database.ConnectionString);
        }
    }
}