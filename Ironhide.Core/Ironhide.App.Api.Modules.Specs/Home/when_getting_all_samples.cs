using System.Collections.Generic;
using System.Linq;
using AcklenAvenue.Commands;
using AutoMapper;
using FizzWare.NBuilder;
using FluentAssertions;
using Ironhide.Api.Infrastructure;
using Ironhide.App.Api.Modules.Home;
using Ironhide.App.Domain.Entities;
using Ironhide.App.Domain.Services;
using Machine.Specifications;
using Moq;
using Nancy.Testing;
using It = Machine.Specifications.It;

namespace Ironhide.App.Api.Modules.Specs.Home
{
    public class when_getting_all_samples
    {
        static Browser _browser;
        static ISampleRepository _readOnlyRepository;
        static ICommandDispatcher _commandDispatcher;
        static IMapper _mapper;
        static BrowserResponse _result;
        static IEnumerable<SampleResponse> _expectedResponse;

        Establish context =
            () =>
            {
                _readOnlyRepository = Mock.Of<ISampleRepository>();
                _mapper = Mock.Of<IMapper>();
                _commandDispatcher = Mock.Of<ICommandDispatcher>();

                var samples = Builder<Sample>.CreateListOfSize(2).Build();

                _expectedResponse = Builder<SampleResponse>.CreateListOfSize(2).Build();

                Mock.Get(_readOnlyRepository).Setup(x => x.GetAll<Sample>()).ReturnsAsync(samples);
                Mock.Get(_mapper)
                    .Setup(
                        x =>
                            x.Map<IEnumerable<Sample>, IEnumerable<SampleResponse>>(
                                Moq.It.Is<IEnumerable<Sample>>(source => source.All(sample => samples.Contains(sample)))))
                    .Returns(_expectedResponse);

                _browser = new Browser(with =>
                                       {
                                           with.Module<AppModule>();
                                           with.Dependencies(_readOnlyRepository);
                                           with.Dependencies(_mapper);
                                           with.Dependencies(_commandDispatcher);
                                           with.Dependencies(Mock.Of<IUserSessionFactory>());
                                       });
            };

        Because of =
            () => _result = _browser.Get("/sample",
                with =>
                {
                    with.HttpRequest();
                    with.Header("Accept", "application/json");
                });

        It should_get_all_samples =
            () =>
            {
                var resultBody = _result.Body<IEnumerable<SampleResponse>>();
                resultBody.ShouldBeEquivalentTo(_expectedResponse);
            };
    }
}