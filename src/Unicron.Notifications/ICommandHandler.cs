using Autofac.Extras.DynamicProxy2;

namespace Starscream.Notifications
{
    [Intercept("log-calls")]
    public interface ICommandHandler
    {
        event DomainEvent NotifyObservers;
    }

    public interface ICommandHandler<in T> : ICommandHandler
    {
        void Handle(IUserSession userIssuingCommand, T command);
    }
}