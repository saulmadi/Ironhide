using System;
using AcklenAvenue.Commands;
using Autofac;
using BlingBag;
using Ironhide.Api.Infrastructure.Authentication;
using Ironhide.Login.Domain.Application.Commands;
using log4net;
using log4net.Config;

namespace Ironhide.Api.Infrastructure.Configuration
{
    public class ConfigureApiDependencies : IBootstrapperTask<ContainerBuilder>
    {
        #region IBootstrapperTask<ContainerBuilder> Members

        public Action<ContainerBuilder> Task
        {
            get
            {
                return container =>
                       {
                           XmlConfigurator.Configure();
                           container.RegisterType<BaseUrlProvider>().As<IBaseUrlProvider>();
                           container.RegisterType<ApiUserMapper>().As<IApiUserMapper<string>>();
                           container.RegisterInstance(LogManager.GetLogger("Logger")).As<ILog>();
                           container.RegisterType<UserSessionFactory>().As<IUserSessionFactory>();
                           ConfigureCommandAndEventHandlers(container);
                           AutoRegisterAllCommandHandlers(container);
                       };
            }
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