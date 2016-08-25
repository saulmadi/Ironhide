using Ironhide.Login.Domain.ValueObjects;

namespace Ironhide.Login.Domain.Services
{
    public interface IPasswordEncryptor
    {
        EncryptedPassword Encrypt(string clearTextPassword);
    }
}