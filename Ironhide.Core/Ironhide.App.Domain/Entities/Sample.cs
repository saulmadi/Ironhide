using System;

namespace Ironhide.App.Domain.Entities
{
    public class Sample : IEntity
    {
        protected Sample()
        {
        }

        public Sample(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public virtual void ChangeName(string newName)
        {
            Name = newName;
        }

        public virtual Guid Id { get; protected set; }
        public virtual string Name { get; protected set; }
    }
}