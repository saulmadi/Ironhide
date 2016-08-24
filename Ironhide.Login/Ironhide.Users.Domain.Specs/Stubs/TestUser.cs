using System;
using Ironhide.Users.Domain.Entities;

namespace Ironhide.Users.Domain.Specs.Stubs
{
    public class TestUser : UserEmailLogin
    {
        public TestUser(Guid userId, string name, string password)
        {
            Id = userId;
            Name = name;
            EncryptedPassword = password;
        }
    }
}