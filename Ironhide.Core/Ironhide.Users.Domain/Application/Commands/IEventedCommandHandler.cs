using AcklenAvenue.Commands;

namespace Ironhide.Users.Domain.Application.Commands
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