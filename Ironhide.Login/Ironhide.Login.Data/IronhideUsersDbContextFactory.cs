using System.Data.Entity.Infrastructure;

namespace Ironhide.Login.Data
{
    public class IronhideUsersDbContextFactory : IDbContextFactory<LoginDataContext>
    {
        public LoginDataContext Create()
        {
            return new LoginDataContext(LoginModule.Database.ConnectionString);
        }
    }
}