using System;

namespace Ironhide.Users.Domain.DomainEvents
{
    public class SampleUpdated
    {
        public SampleUpdated(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
    }
}