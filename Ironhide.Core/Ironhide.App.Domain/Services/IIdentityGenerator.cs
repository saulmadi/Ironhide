namespace Ironhide.App.Domain.Services
{
    public interface IIdentityGenerator<out T>
    {
        T Generate();
    }
}