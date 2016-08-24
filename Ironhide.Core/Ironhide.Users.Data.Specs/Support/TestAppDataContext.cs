using System.Data.Entity;
using System.Threading.Tasks;
using Ironhide.Users.Domain;
using Ironhide.Users.Domain.Entities;

namespace Ironhide.App.Data.Specs.Support
{
    public class TestAppDataContext : IAppDataContext
    {
        public TestAppDataContext()
        {
            Users = new TestDbSet<User>();
            PasswordResetTokens = new TestDbSet<Sample>();
            Roles = new TestDbSet<Role>();
            UserAbilities = new TestDbSet<UserAbility>();
        }

        public int Saved { get; private set; }

        public DbSet<Sample> PasswordResetTokens { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserAbility> UserAbilities { get; set; }

        public async Task SaveChanges()
        {
            Saved ++;
        }

        public DbSet<User> Users { get; set; }

        public void Seed(params User[] seed)
        {
            Users.AddRange(seed);
        }

        public void Seed(params Sample[] seed)
        {
            PasswordResetTokens.AddRange(seed);
        }

        public void Seed(params Role[] seed)
        {
            Roles.AddRange(seed);
        }

        public void Seed(params UserAbility[] seed)
        {
            UserAbilities.AddRange(seed);
        }
    }
}