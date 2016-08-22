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
using Ironhide.Api.Modules.UserAccounts;
using Ironhide.Api.Modules.UserManagement;
using Ironhide.Users.Data;
using Ironhide.Users.Data.Repositories;
using Ironhide.Users.Domain;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;
using Newtonsoft.Json;

namespace Ironhide.Api.Modules
{
    public class ConfigureUsersModule : IBootstrapperTask<ContainerBuilder>
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
                                  cfg.CreateMap<User, AdminUserResponse>();
                                  cfg.CreateMap<UserAbility, UserAbilityRequest>().ReverseMap();
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
            container.RegisterInstance(new UserDataContext(UserModule.Database.ConnectionString)).As<IUserDataContext>();
            container.RegisterType<UserRepository>().As<IUserRepository<User>>();
        }

        #endregion
    }
}