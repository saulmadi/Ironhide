using System;

namespace Ironhide.Users.Domain.Entities
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

        public virtual Guid Id { get; protected set; }
        public virtual string Name { get; set; }
    }
}