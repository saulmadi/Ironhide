﻿using System;

namespace Ironhide.Users.Domain.DomainEvents
{
    public class UserProfileUpdated
    {
        public UserProfileUpdated(Guid id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }

        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public string Email { get; protected set; }
    }
}