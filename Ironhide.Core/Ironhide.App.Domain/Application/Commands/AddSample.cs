using System;

namespace Ironhide.App.Domain.Application.Commands
{
    public class AddSample
    {
        protected AddSample()
        {
        }

        public AddSample(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
    }
}