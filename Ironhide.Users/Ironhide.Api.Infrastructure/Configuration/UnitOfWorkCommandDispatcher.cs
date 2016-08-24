using System.Threading.Tasks;
using AcklenAvenue.Commands;

namespace Ironhide.Api.Infrastructure.Configuration
{
    public class UnitOfWorkCommandDispatcher : ICommandDispatcher
    {
        readonly ICommandDispatcher _decoratedCommandDispatcher;

        public UnitOfWorkCommandDispatcher(ICommandDispatcher decoratedCommandDispatcher)
        {
            _decoratedCommandDispatcher = decoratedCommandDispatcher;
        }

        public async Task Dispatch(IUserSession userSession, object command)
        {
            //using (var tx = _session.BeginTransaction())
            //{
            await _decoratedCommandDispatcher.Dispatch(userSession, command);
            //tx.Commit();
            //}
        }
    }
}