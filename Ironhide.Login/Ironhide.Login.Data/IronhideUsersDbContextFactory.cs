using System.Data.Entity.Infrastructure;

namespace Ironhide.Users.Data
{
    public class IronhideUsersDbContextFactory : IDbContextFactory<LoginDataContext>
    {
        public LoginDataContext Create()
        {
            return new LoginDataContext(LoginModule.Database.ConnectionString);
        }
    }
}