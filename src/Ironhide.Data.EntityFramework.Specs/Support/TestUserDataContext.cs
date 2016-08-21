using System.Data.Entity;
using Ironhide.Users.Domain.Entities;

namespace Ironhide.Users.Data.Specs.Support
{
    public class TestUserDataContext : IUserDataContext
    {
        public TestUserDataContext()
        {
            Users = new TestDbSet<User>();
            PasswordResetTokens = new TestDbSet<PasswordResetToken>();
            Roles = new TestDbSet<Role>();
            UserAbilities = new TestDbSet<UserAbility>();
        }

        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserAbility> UserAbilities { get; set; }
        public DbSet<User> Users { get; set; }

        public void Seed(params User[] seed)
        {
            Users.AddRange(seed);
        }

        public void Seed(params PasswordResetToken[] seed)
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