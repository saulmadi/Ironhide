namespace Unicron.Users.Domain.Services
{
    public interface IIdentityGenerator<out T>
    {
        T Generate();
    }
}