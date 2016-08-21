using System.Data.Entity;
using Ironhide.Users.Domain.Entities;

namespace Ironhide.Users.Data
{
    public interface IUserDataContext
    {
        DbSet<User> Users { get; set; }
        DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<UserAbility> UserAbilities { get; set; } 
    }
}