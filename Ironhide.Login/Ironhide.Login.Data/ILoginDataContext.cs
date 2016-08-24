using System.Data.Entity;
using System.Threading.Tasks;
using Ironhide.Login.Domain.Entities;
using Ironhide.Users.Domain;
using Ironhide.Users.Domain.Entities;

namespace Ironhide.Users.Data
{
    public interface ILoginDataContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<UserAbility> UserAbilities { get; set; }
        Task SaveChanges();
    }
}