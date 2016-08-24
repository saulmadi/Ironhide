using System.Threading.Tasks;
using AcklenAvenue.Commands;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.DomainEvents;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;

namespace Ironhide.Users.Domain.Application.CommandHandlers
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
            Sample sample = _repo.GetById<Sample>(command.Id);
            await _repo.Delete(sample);
            NotifyObservers(new SampleUpdated(sample.Id));
        }

        public event DomainEvent NotifyObservers;
    }
}