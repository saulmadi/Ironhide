using AcklenAvenue.Commands;
using AcklenAvenue.Testing.Moq.ExpectedObjects;
using AutoMapper;
using FizzWare.NBuilder;
using Ironhide.Api.Infrastructure;
using Ironhide.App.Api.Modules.Home;
using Ironhide.App.Domain;
using Ironhide.App.Domain.Application.Commands;
using Ironhide.App.Domain.Services;
using Machine.Specifications;
using Moq;
using Nancy;
using Nancy.Testing;
using It = Machine.Specifications.It;

namespace Ironhide.App.Api.Modules.Specs.Home
{
    public class when_successfully_updating_a_sample
    {
        static ISampleRepository _readOnlyRepository;
        static IMapper _mapper;
        static ICommandDispatcher _commandDispatcher;
        static Browser _browser;
        static SampleRequest _request;
        static BrowserResponse _result;
        static IUserSessionFactory _userFactory;
        static TestIdentity _userIdentity;
        static IUserSession _userSession;

        Establish context =
            () =>
            {
                _readOnlyRepository = Mock.Of<ISampleRepository>();
                _mapper = Mock.Of<IMapper>();
                _commandDispatcher = Mock.Of<ICommandDispatcher>();
                _request = Builder<SampleRequest>.CreateNew().Build();
                _userFactory = Mock.Of<IUserSessionFactory>();

                Mock.Get(_userFactory)
                    .Setup(x => x.Create(_userIdentity))
                    .Returns(_userSession = new BasicUserSession("user"));

                _browser = new Browser(with =>
                                       {
                                           with.Module<AppModule>();
                                           with.Dependencies(_readOnlyRepository);
                                           with.Dependencies(_mapper);
                                           with.Dependencies(_commandDispatcher);
                                           with.Dependencies(_userFactory);
                                       });
            };

        Because of =
            () => _result = _browser.Put("/sample",
                with =>
                {
                    with.HttpRequest();
                    with.Header("Accept", "application/json");
                    with.JsonBody(_request);
                });

        It should_be_okay = () => _result.StatusCode.ShouldEqual(HttpStatusCode.OK);

        It should_dispatch_the_command =
            () => Mock.Get(_commandDispatcher)
                .Verify(
                    x => x.Dispatch(_userSession, WithExpected.Object(new UpdateSample(_request.Id, _request.Name), AllowAnonymous.No)));
    }
}