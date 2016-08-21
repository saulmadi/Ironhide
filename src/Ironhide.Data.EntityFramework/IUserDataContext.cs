using System.Data.Entity;
using System.Threading.Tasks;
using Ironhide.Users.Domain.Entities;

namespace Ironhide.Users.Data
{
    public interface IUserDataContext
    {
        DbSet<User> Users { get; set; }
        DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<UserAbility> UserAbilities { get; set; }
        Task SaveChanges();
    }
}