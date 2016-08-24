﻿using System;

namespace Ironhide.Users.Domain.Application.Commands
{
    public class UpdateSample
    {
        protected UpdateSample()
        {
        }

        public UpdateSample(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
    }
}