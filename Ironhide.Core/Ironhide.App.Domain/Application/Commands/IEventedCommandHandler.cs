using AcklenAvenue.Commands;

namespace Ironhide.App.Domain.Application.Commands
{
    public interface IEventedCommandHandler<in TUserSession, in TCommand> : ICommandHandler<TUserSession, TCommand>,
        IEventedCommandHandler
    {
    }

    public interface IEventedCommandHandler
    {
        event DomainEvent NotifyObservers;
    }
}