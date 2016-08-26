using System.Data.Entity;
using System.Threading.Tasks;
using Ironhide.App.Domain.Entities;
namespace Ironhide.App.Data
{
    public class AppDataContext : DbContext, IAppDataContext
    {
        public AppDataContext(string connectionString) : base(connectionString)
        {
        }

        public DbSet<Sample> Samples { get; set; }

        public async new Task SaveChanges()
        {
            await SaveChangesAsync();
        }
    }
}