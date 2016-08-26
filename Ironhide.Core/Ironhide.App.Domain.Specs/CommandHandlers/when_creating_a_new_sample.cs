using System;
using AcklenAvenue.Commands;
using FizzWare.NBuilder;
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
    public class when_creating_a_new_sample
    {
        static AddSample _command;
        static ISampleRepository _sampleRepo;
        static IEventedCommandHandler<IUserSession, AddSample> _handler;
        static SampleUpdated _expectedEvent;
        static object _eventRaised;
        static Sample _sampleCreated;

        Establish context =
            () =>
            {
                _command = new AddSample("Test name");

                _sampleCreated = Builder<Sample>.CreateNew()
                    .With(user => user.Name, _command.Name)
                    .Build();

                _sampleRepo = Mock.Of<ISampleRepository>();

                Mock.Get(_sampleRepo)
                    .Setup(repository => repository.Create(Moq.It.IsAny<Sample>()))
                    .ReturnsAsync(_sampleCreated);

                _handler = new SampleAdder(_sampleRepo);

                _expectedEvent = new SampleUpdated(_sampleCreated.Id);
                _handler.NotifyObservers += x => _eventRaised = x;
            };

        Because of =
            () => _handler.Handle(Mock.Of<IUserSession>(), _command);

        It should_create_the_new_sample =
            () => Mock.Get(_sampleRepo).Verify(
                x =>
                    x.Create(Moq.It.Is<Sample>(u =>
                        u.Name == _command.Name
                        )));


        It should_throw_the_expected_event =
            () => _eventRaised.ShouldBeLike(_expectedEvent);
    }
}