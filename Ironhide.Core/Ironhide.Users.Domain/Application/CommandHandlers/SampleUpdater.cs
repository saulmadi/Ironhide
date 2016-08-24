using System.Threading.Tasks;
using AcklenAvenue.Commands;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.DomainEvents;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;

namespace Ironhide.Users.Domain.Application.CommandHandlers
{
    public class SampleUpdater : IEventedCommandHandler<IUserSession, UpdateSample>
    {
        readonly ISampleRepository _repo;


        public SampleUpdater(ISampleRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle(IUserSession userIssuingCommand, UpdateSample command)
        {
            Sample sample = await _repo.GetById<Sample>(command.Id);
            sample.Name = command.Name;
            await _repo.Update(sample);
            NotifyObservers(new SampleUpdated(sample.Id));
        }

        public event DomainEvent NotifyObservers;
    }
}