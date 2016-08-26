using System.Collections.Generic;
using System.Threading.Tasks;
using AcklenAvenue.Commands;
using BlingBag;
using Ironhide.App.Domain.Application.Commands;

namespace Ironhide.Api.Infrastructure.Configuration
{
    public class IronhideCommandDispatcher : ImmediateCommandDispatcher
    {
        readonly IBlingDispatcher _blingDispatcher;

        public IronhideCommandDispatcher(IEnumerable<ICommandHandler> commandHandlers,
            IEnumerable<ICommandValidator> commandValidators, ICommandDispatcherLogger logger,
            IBlingDispatcher blingDispatcher)
            : base(commandHandlers, commandValidators, logger)
        {
            _blingDispatcher = blingDispatcher;
        }

        protected override Task Handle(IUserSession userSession, object command, object handler)
        {
            var eventedCommandHandler = handler as IEventedCommandHandler;
            if (eventedCommandHandler != null)
            {
                eventedCommandHandler.NotifyObservers += @event => _blingDispatcher.Dispatch(@event);
            }
            return base.Handle(userSession, command, handler);
        }
    }
}