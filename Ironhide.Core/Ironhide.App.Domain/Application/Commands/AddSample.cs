﻿using System;

namespace Ironhide.App.Domain.Application.Commands
{
    public class AddSample
    {
        protected AddSample()
        {
        }

        public AddSample(string name)
        {
            Name = name;
        }
        public string Name { get; protected set; }
    }
}