namespace Starscream.Notifications.Specs.Testing
{
    public class TestCommandHandler : ICommandHandler<TestCommand>
    {
        public TestCommand CommandHandled { get; private set; }

        public void Handle(IUserSession userIssuingCommand, TestCommand command)
        {
            CommandHandled = command;
            if (NotifyObservers != null) NotifyObservers(new TestEvent(command));
        }

        public event DomainEvent NotifyObservers;
    }
}