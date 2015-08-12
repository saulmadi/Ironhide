namespace Starscream.Notifications.Specs.Testing
{
    public class TestCommandValidator : ICommandValidator<TestCommand>
    {
        public TestCommand CommandValidated { get; private set; }

        public void Validate(IUserSession userSession, TestCommand command)
        {
            CommandValidated = command;
        }
    }
}