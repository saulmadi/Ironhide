using System.Data.Entity;
using System.Threading.Tasks;
using Ironhide.Users.Domain;
using Ironhide.Users.Domain.Entities;

namespace Ironhide.App.Data
{
    public class AppDataContext : DbContext, IAppDataContext
    {
        public AppDataContext(string connectionString) : base(connectionString)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Sample> PasswordResetTokens { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserAbility> UserAbilities { get; set; }

        public async Task SaveChanges()
        {
            await SaveChangesAsync();
        }
    }
}