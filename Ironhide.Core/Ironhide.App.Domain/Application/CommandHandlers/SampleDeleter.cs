using System.Threading.Tasks;
using AcklenAvenue.Commands;
using Ironhide.App.Domain.Application.Commands;
using Ironhide.App.Domain.DomainEvents;
using Ironhide.App.Domain.Entities;
using Ironhide.App.Domain.Services;

namespace Ironhide.App.Domain.Application.CommandHandlers
{
    public class SampleDeleter : IEventedCommandHandler<IUserSession, DeleteSample>
    {
        readonly ISampleRepository _repo;


        public SampleDeleter(ISampleRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle(IUserSession userIssuingCommand, DeleteSample command)
        {
            Sample sample = await _repo.GetById<Sample>(command.Id);
            await _repo.Delete(sample.Id);
            NotifyObservers(new SampleUpdated(sample.Id));
        }

        public event DomainEvent NotifyObservers;
    }
}