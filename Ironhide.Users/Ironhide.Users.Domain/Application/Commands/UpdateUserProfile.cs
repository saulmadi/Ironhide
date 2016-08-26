using System;

namespace Ironhide.Users.Domain.Application.Commands
{
    public class UpdateUserProfile
    {
        public UpdateUserProfile(Guid id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }

        protected UpdateUserProfile()
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}