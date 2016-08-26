using System;
using System.Collections.Generic;
using AcklenAvenue.Commands;
using Ironhide.App.Domain.Application.Commands;
using Ironhide.App.Domain.Services;
using Ironhide.App.Domain.Validators;
using Ironhide.Common;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Ironhide.App.Domain.Specs.Validation
{
    public class when_validating_an_invalid_sample_add_command
    {
        static ICommandValidator<IUserSession, AddSample> _validator;
        static Exception _exception;

        Establish context =
            () =>
            {
                _validator = new SampleValidator(Mock.Of<ISampleRepository>(),
                    Mock.Of<ITimeProvider>());

            };

        Because of =
            () => _exception = Catch.Exception(() => _validator.Validate(new VisitorSession(),
                new AddSample("Name")).Await());

        It should_return_expected_failures =
            () => _exception.ShouldBeOfExactType(typeof(ArgumentException));
    }
}