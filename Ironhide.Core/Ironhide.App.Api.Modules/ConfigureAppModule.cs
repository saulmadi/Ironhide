using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AcklenAvenue.Commands;
using Autofac;
using AutoMapper;
using Ironhide.Api.Infrastructure;
using Ironhide.Api.Infrastructure.Authentication;
using Ironhide.Api.Infrastructure.Authentication.Roles;
using Ironhide.Api.Infrastructure.Properties;
using Ironhide.App.Api.Modules.Home;
using Ironhide.App.Data;
using Ironhide.App.Data.Repositories;
using Ironhide.App.Domain;
using Ironhide.App.Domain.Entities;
using Ironhide.App.Domain.Services;
using Newtonsoft.Json;
using AppModule = Ironhide.App.Data.AppModule;

namespace Ironhide.App.Api.Modules
{
    public class ConfigureAppModule : IBootstrapperTask<ContainerBuilder>
    {
        #region IBootstrapperTask<ContainerBuilder> Members

        public Action<ContainerBuilder> Task
        {
            get
            {
                return container =>
                       {
                           AutoRegisterDataAndDomainDependencies(container);
                           WireUpDatabaseStuff(container);
                           ConfigureAutomapperMappings(container);
                           RegisterUsersFeatures(container);
                       };
            }
        }

        static void AutoRegisterDataAndDomainDependencies(ContainerBuilder container)
        {
            container
                .RegisterAssemblyTypes(new[]
                                       {
                                           typeof (SampleRepository).Assembly,
                                           typeof (IEntity).Assembly
                                       })
                .Where(x => x.GetInterfaces().Any())
                .Where(x => typeof (ICommandHandler).IsAssignableFrom(x) != true)
                .AsImplementedInterfaces();
        }

        void ConfigureAutomapperMappings(ContainerBuilder container)
        {
            Mapper.Initialize(cfg =>
                              {
                                  cfg.CreateMap<Sample, SampleResponse>();
                              });

            container.RegisterInstance(Mapper.Configuration.CreateMapper()).As<IMapper>();
        }

        void RegisterUsersFeatures(ContainerBuilder container)
        {
            byte[] bytes = Resources.RolesFeatures;
            var reader = new StreamReader(new MemoryStream(bytes), Encoding.Default);


            var usersRoles = new JsonSerializer().Deserialize<IEnumerable<UsersRoles>>(new JsonTextReader(reader));


            container.RegisterType<MenuProvider>().As<IMenuProvider>().WithParameter("usersRoles", usersRoles);
        }

        void WireUpDatabaseStuff(ContainerBuilder container)
        {
            container.RegisterInstance(new AppDataContext(AppModule.Database.ConnectionString)).As<IAppDataContext>();
            container.RegisterType<SampleRepository>().As<ISampleRepository>();
        }

        #endregion
    }
}