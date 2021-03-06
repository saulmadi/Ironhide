using System;
using System.Threading.Tasks;
using AcklenAvenue.Commands;
using Ironhide.App.Domain.Application.Commands;
using Ironhide.App.Domain.DomainEvents;
using Ironhide.App.Domain.Entities;
using Ironhide.App.Domain.Services;

namespace Ironhide.App.Domain.Application.CommandHandlers
{
    public class SampleAdder : IEventedCommandHandler<IUserSession, AddSample>
    {
        readonly ISampleRepository _repo;

        public SampleAdder(ISampleRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle(IUserSession userIssuingCommand, AddSample command)
        {
            Sample sample = new Sample(Guid.NewGuid(), command.Name);
            var sampleCreated = await _repo.Create(sample);
            NotifyObservers(new SampleUpdated(sampleCreated.Id));
        }

        public event DomainEvent NotifyObservers;
    }
}