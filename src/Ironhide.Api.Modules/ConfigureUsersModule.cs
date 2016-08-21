using System;
using System.Configuration;
using System.Linq;
using AcklenAvenue.Commands;
using Autofac;
using AutoMapper;
using Ironhide.Api.Infrastructure;
using Ironhide.Api.Modules.UserAccounts;
using Ironhide.Api.Modules.UserManagement;
using Ironhide.Users.Data;
using Ironhide.Users.Data.Repositories;
using Ironhide.Users.Domain;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;

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
                .Where(x => typeof(ICommandHandler).IsAssignableFrom(x) != true)
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

        void WireUpDatabaseStuff(ContainerBuilder container)
        {
            string connStrEnvVarName = ConfigurationManager.AppSettings["UserModuleConnectionStringName"];
            string connectionString = Environment.GetEnvironmentVariable(connStrEnvVarName);
            container.RegisterInstance(new UserDataContext(connectionString)).As<IUserDataContext>();

            container.RegisterType<UserRepository>().As<IUserRepository<User>>();
        }

        #endregion
    }
}