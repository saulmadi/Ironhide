using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using AcklenAvenue.Commands;
using Autofac;
using BlingBag;
using Ironhide.Api.Infrastructure.Authentication;
using Ironhide.Api.Infrastructure.Authentication.Roles;
using Ironhide.Api.Infrastructure.Properties;
using Ironhide.Users.Data.Repositories;
using Ironhide.Users.Domain;
using Ironhide.Users.Domain.Application.Commands;
using log4net;
using log4net.Config;
using Newtonsoft.Json;

namespace Ironhide.Api.Infrastructure.Configuration
{
    public class ConfigureCommonDependencies : IBootstrapperTask<ContainerBuilder>
    {
        #region IBootstrapperTask<ContainerBuilder> Members

        public Action<ContainerBuilder> Task
        {
            get
            {
                return container =>
                       {
                           AutoRegisterDataAndDomain(container);
                           container.RegisterType<BaseUrlProvider>().As<IBaseUrlProvider>();
                           container.RegisterType<ApiUserMapper>().As<IApiUserMapper<string>>();
                           container.RegisterInstance(LogManager.GetLogger("Logger")).As<ILog>();
                           container.RegisterType<UserSessionFactory>().As<IUserSessionFactory>();
                           XmlConfigurator.Configure();

                           ConfigureCommandAndEventHandlers(container);

                           AutoRegisterAllCommandHandlers(container);
                           RegisterUsersFeutures(container);
                       };
            }
        }

        void RegisterUsersFeutures(ContainerBuilder container)
        {
            byte[] bytes = Resources.RolesFeatures;
            var reader = new StreamReader(new MemoryStream(bytes), Encoding.Default);


            var usersRoles = new JsonSerializer().Deserialize<IEnumerable<UsersRoles>>(new JsonTextReader(reader));


            container.RegisterType<MenuProvider>().As<IMenuProvider>().WithParameter("usersRoles", usersRoles);
        }

        void AutoRegisterAllCommandHandlers(ContainerBuilder container)
        {
            container.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .Where(
                    x =>
                        typeof (ICommandHandler).IsAssignableFrom(x) ||
                        typeof (IEventedCommandHandler).IsAssignableFrom(x))
                .AsImplementedInterfaces();
        }

        #endregion

        static void ConfigureCommandAndEventHandlers(ContainerBuilder container)
        {
            container.RegisterType<IronhideBlingDispatcher>().As<IBlingDispatcher>();

            container.RegisterType<IronhideCommandDispatcher>().Named<ICommandDispatcher>("CommandDispatcher");
            container.RegisterDecorator<ICommandDispatcher>(
                (c, inner) => new UnitOfWorkCommandDispatcher(inner), "CommandDispatcher");

            container.RegisterType<CommandDispatcherLogger>().As<ICommandDispatcherLogger>().As<IBlingLogger>();
        }

        
    }
}