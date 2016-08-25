namespace Ironhide.Login.Domain
{
    public interface IKeyProvider
    {
        byte[] GetKey();
    }
}