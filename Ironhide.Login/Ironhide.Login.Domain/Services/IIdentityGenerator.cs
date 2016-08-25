namespace Ironhide.Login.Domain.Services
{
    public interface IIdentityGenerator<out T>
    {
        T Generate();
    }
}