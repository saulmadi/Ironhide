using System.Data.Entity;
using Ironhide.Users.Domain.Entities;

namespace Ironhide.Users.Data
{
    public class UserDataContext : DbContext, IUserDataContext
    {
        public UserDataContext(string connectionString): base(connectionString)
        {            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserAbility> UserAbilities { get; set; }
    }
}