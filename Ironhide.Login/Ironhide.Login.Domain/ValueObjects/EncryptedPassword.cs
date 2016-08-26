namespace Ironhide.Login.Domain.ValueObjects
{
    public class EncryptedPassword
    {
        public EncryptedPassword(string encryptedPassword)
        {
            Password = encryptedPassword;
        }

        public string Password { get; set; }
    }
}