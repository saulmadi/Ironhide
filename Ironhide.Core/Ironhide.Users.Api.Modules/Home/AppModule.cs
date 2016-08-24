using System;
using System.Collections.Generic;
using AcklenAvenue.Commands;
using AutoMapper;
using Ironhide.Api.Infrastructure;
using Ironhide.Api.Infrastructure.RestExceptions;
using Ironhide.Users.Domain;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Exceptions;
using Ironhide.Users.Domain.Services;
using Ironhide.Users.Domain.ValueObjects;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;

namespace Ironhide.Api.Modules.Home
{
    public class AppModule : NancyModule
    {
        public AppModule(IPasswordEncryptor passwordEncryptor, ISampleRepository readOnlyRepository,
            ISampleRepository sampleRepo, ITokenFactory tokenFactory, IMenuProvider menuProvider, IMapper mapper, ICommandDispatcher commandDispatcher,
            IUserSessionFactory userSessionFactory)
        {
            Get["/"] =
            _ =>
            {
                return "Ironhide";
            };

            Get["/sample", true] =
            async (a, ct) =>
            {
                var samples = await readOnlyRepository.GetAll<Sample>();

                return mapper.Map<IEnumerable<Sample>, IEnumerable<SampleResponse>>(samples);
            };

            Post["/sample", true] =
                async (a, ct) =>
                      {
                          var sampleRequest = this.Bind<SampleRequest>();
                          var command = new AddSample(sampleRequest.Id, sampleRequest.Name);

                          await commandDispatcher.Dispatch(userSessionFactory.Create(Context.CurrentUser), command);

                          return null;
                      };

            Put["/sample", true] =
                async (a, ct) =>
                      {
                          var sampleRequest = this.Bind<SampleRequest>();
                          var command = new UpdateSample(sampleRequest.Id, sampleRequest.Name);

                          await commandDispatcher.Dispatch(userSessionFactory.Create(Context.CurrentUser), command);

                          return null;
                      };
            Delete["/sample", true] =
                async (a, ct) =>
                {
                    var sampleRequest = this.Bind<SampleRequest>();
                    var command = new DeleteSample(sampleRequest.Id);

                    await commandDispatcher.Dispatch(userSessionFactory.Create(Context.CurrentUser), command);

                    return null;
                };

        }
    }
}