using System;
using AcklenAvenue.Commands;
using Ironhide.App.Domain.Application.CommandHandlers;
using Ironhide.App.Domain.Application.Commands;
using Ironhide.App.Domain.DomainEvents;
using Ironhide.App.Domain.Entities;
using Ironhide.App.Domain.Services;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Ironhide.App.Domain.Specs.CommandHandlers
{
    public class when_deleting_a_sample
    {
        static IEventedCommandHandler<IUserSession, DeleteSample> _handler;
        static ISampleRepository _sampleRepo;
        static Sample _sample;
        static object _eventRaised;
        static object _expectedEvent;
        static DeleteSample _command;

        Establish context =
            () =>
            {
                _sampleRepo = Mock.Of<ISampleRepository>();
                _handler = new SampleDeleter(_sampleRepo);
                _sample = new Sample(Guid.NewGuid(), "Test User");

                _command = new DeleteSample(_sample.Id);

                Mock.Get(_sampleRepo).Setup(x => x.GetById<Sample>(_sample.Id))
                    .ReturnsAsync(_sample);

                _handler.NotifyObservers += x => _eventRaised = x;
                _expectedEvent = new SampleUpdated(_sample.Id);
            };

        Because of =
            () =>
                _handler.Handle(new BasicUserSession(Guid.NewGuid().ToString()), _command);

        It should_throw_the_expected_event =
            () => _eventRaised.ShouldBeLike(_expectedEvent);

    }
}