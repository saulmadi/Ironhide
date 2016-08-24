using System;
using System.Collections.Generic;
using System.Linq;
using Nancy.Bootstrapper;

namespace Ironhide.Api.Infrastructure.RestExceptions
{
    public static class RestExceptionRepackager
    {
        public static RestExceptionRepackagingRegistrar Configure(Action<RestExceptionConfiguration> config)
        {
            var configurer = new RestExceptionConfiguration();

            IEnumerable<Type> repackagers =
                AppDomainAssemblyTypeScanner.TypesOf<IExceptionRepackager>(ScanMode.ExcludeNancy);

            repackagers.ToList().ForEach(
                x => configurer.WithRepackager((IExceptionRepackager) Activator.CreateInstance(x)));

            configurer.WithDefault(new InternalServerExceptionRepackager());

            config(configurer);

            return new RestExceptionRepackagingRegistrar(configurer);
        }
    }
}