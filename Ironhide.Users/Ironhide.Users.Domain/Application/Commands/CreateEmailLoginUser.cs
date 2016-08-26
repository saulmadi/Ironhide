using System.Collections.Generic;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.ValueObjects;

namespace Ironhide.Users.Domain.Application.Commands
{
    public class CreateEmailLoginUser
    {
        public CreateEmailLoginUser(string email, EncryptedPassword password, string name, string phoneNumber,
            IEnumerable<UserAbility> abilities)
        {
            Email = email;
            EncryptedPassword = password;
            Name = name;
            PhoneNumber = phoneNumber;
            this.Abilities = abilities;
        }

        public string Email { get; private set; }
        public EncryptedPassword EncryptedPassword { get; private set; }
        public string Name { get; private set; }
        public string PhoneNumber { get; private set; }
        public IEnumerable<UserAbility> Abilities { get; private set; }
    }
}