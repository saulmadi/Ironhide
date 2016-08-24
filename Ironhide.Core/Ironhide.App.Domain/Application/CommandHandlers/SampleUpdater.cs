using System.Threading.Tasks;
using AcklenAvenue.Commands;
using Ironhide.App.Domain.Application.Commands;
using Ironhide.App.Domain.DomainEvents;
using Ironhide.App.Domain.Entities;
using Ironhide.App.Domain.Services;

namespace Ironhide.App.Domain.Application.CommandHandlers
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
            sample.ChangeName(command.Name);
            await _repo.Update(sample);
            NotifyObservers(new SampleUpdated(sample.Id));
        }

        public event DomainEvent NotifyObservers;
    }
}