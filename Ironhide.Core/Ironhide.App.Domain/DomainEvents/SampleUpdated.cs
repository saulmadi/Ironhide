using System;

namespace Ironhide.App.Domain.DomainEvents
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