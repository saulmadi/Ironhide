namespace Starscream.Domain
{
    public interface IUserSessionFactory
    {
        UserSession Create(User executor);
    }
}