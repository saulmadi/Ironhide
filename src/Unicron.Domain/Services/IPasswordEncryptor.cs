using Unicron.Users.Domain.ValueObjects;

namespace Unicron.Users.Domain.Services
{
    public interface IPasswordEncryptor
    {
        EncryptedPassword Encrypt(string clearTextPassword);
    }
}