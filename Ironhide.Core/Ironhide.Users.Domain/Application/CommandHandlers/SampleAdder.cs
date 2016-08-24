using System.Threading.Tasks;
using AcklenAvenue.Commands;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.DomainEvents;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;

namespace Ironhide.Users.Domain.Application.CommandHandlers
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
            Sample sample = new Sample(command.Id, command.Name);
            await _repo.Create(sample);
            NotifyObservers(new SampleUpdated(sample.Id));
        }

        public event DomainEvent NotifyObservers;
    }
}