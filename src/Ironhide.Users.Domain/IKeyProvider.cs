namespace Ironhide.Users.Domain
{
    public interface IKeyProvider
    {
        byte[] GetKey();
    }
}