using System.Collections.Generic;
using AcklenAvenue.Commands;
using AutoMapper;
using Ironhide.Api.Infrastructure;
using Ironhide.App.Domain.Application.Commands;
using Ironhide.App.Domain.Entities;
using Ironhide.App.Domain.Services;
using Nancy;
using Nancy.ModelBinding;

namespace Ironhide.App.Api.Modules.Home
{
    public class AppModule : NancyModule
    {
        public AppModule(ISampleRepository readOnlyRepository, IMapper mapper, ICommandDispatcher commandDispatcher,
            IUserSessionFactory userSessionFactory)
        {
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
                          var command = new AddSample(sampleRequest.Name);

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