using System.Collections.Generic;
using BlingBag;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Starscream.Notifications.Specs.Testing
{
    public class when_dispatching_a_command
    {
        static ImmediateCommandDispatcher _dispatcher;
        static TestCommandHandler _testCommandHandler;
        static readonly TestCommand TestCommand = new TestCommand();
        static TestCommandValidator _testCommandValidator;

        Establish context =
            () =>
            {
                _testCommandHandler = new TestCommandHandler();
                _testCommandValidator = new TestCommandValidator();
                var blingInitializer = Mock.Of<IBlingInitializer<DomainEvent>>();
                var commandHandlers = new List<ICommandHandler>
                                      {
                                          _testCommandHandler,
                                          new AnotherTestCommandHandler()
                                      };
                var commandValidators = new List<ICommandValidator> {_testCommandValidator};

                _dispatcher = new ImmediateCommandDispatcher(blingInitializer, commandHandlers, commandValidators);
            };

        Because of =
            () => _dispatcher.Dispatch(null, TestCommand);

        It should_dispatch_the_expected_command =
            () => _testCommandHandler.CommandHandled.ShouldEqual(TestCommand);

        It should_validate_the_command =
            () => _testCommandValidator.CommandValidated.ShouldEqual(TestCommand);
    }
}