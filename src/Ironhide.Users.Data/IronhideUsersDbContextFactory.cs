using System.Data.Entity.Infrastructure;

namespace Ironhide.Users.Data
{
    public class IronhideUsersDbContextFactory : IDbContextFactory<UserDataContext>
    {
        public UserDataContext Create()
        {
            return new UserDataContext(UserModule.Database.ConnectionString);
        }
    }
}