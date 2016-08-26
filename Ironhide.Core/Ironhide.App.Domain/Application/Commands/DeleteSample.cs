using System;

namespace Ironhide.App.Domain.Application.Commands
{
    public class DeleteSample
    {
        protected DeleteSample()
        {
        }

        public DeleteSample(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; protected set; }
    }
}