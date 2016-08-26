using System.Data.Entity;
using System.Threading.Tasks;
using Ironhide.Login.Domain.Entities;

namespace Ironhide.Login.Data
{
    public class LoginDataContext : DbContext, ILoginDataContext
    {
        public LoginDataContext(string connectionString) : base(connectionString)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserAbility> UserAbilities { get; set; }

        public async Task SaveChanges()
        {
            await SaveChangesAsync();
        }
    }
}