using System;
using Ironhide.Users.Domain.ValueObjects;

namespace Ironhide.Users.Domain.Entities
{
    public class UserEmailLogin : User
    {
        protected UserEmailLogin()
        {
        }

        public UserEmailLogin(Guid id, string name, string emailAddress, EncryptedPassword encryptedPassword):base(id, name,emailAddress)
        {
            
           
            EncryptedPassword = encryptedPassword.Password;
        }

        public UserEmailLogin(Guid id, string name, string emailAddress, EncryptedPassword encryptedPassword, string phoneNumber) :this(id, name, emailAddress, encryptedPassword)
        {
            PhoneNumber = phoneNumber;            
        }

        public virtual string PhoneNumber { get; protected set; }
        public virtual string EncryptedPassword { get; protected set; }

        public virtual void ChangePassword(EncryptedPassword encryptedPassword)
        {
            EncryptedPassword = encryptedPassword.Password;
        }
    }
}