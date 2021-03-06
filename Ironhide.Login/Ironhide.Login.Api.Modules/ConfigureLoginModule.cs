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
using Ironhide.Login.Data;
using Ironhide.Login.Data.Repositories;
using Ironhide.Login.Domain;
using Ironhide.Login.Domain.Services;
using Newtonsoft.Json;

namespace Ironhide.Login.Api.Modules
{
    public class ConfigureLoginModule : IBootstrapperTask<ContainerBuilder>
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
                                           typeof (UserRepository).Assembly,
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
                                  // Example ===> cfg.CreateMap<User, AdminUserResponse>();
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
            container.RegisterInstance(new LoginDataContext(LoginModule.Database.ConnectionString)).As<ILoginDataContext>();
            container.RegisterType<UserRepository>().As<IUserRepository>();
        }

        #endregion
    }
}