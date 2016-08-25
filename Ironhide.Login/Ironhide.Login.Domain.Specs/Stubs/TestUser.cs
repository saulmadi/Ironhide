using System;
using Ironhide.Login.Domain.Entities;

namespace Ironhide.Login.Domain.Specs.Stubs
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