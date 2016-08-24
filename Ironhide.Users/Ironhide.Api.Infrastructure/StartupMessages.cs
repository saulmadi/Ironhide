using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;

namespace Ironhide.Api.Infrastructure
{
    public static class StartupMessages
    {
        public static string GetStartupMessageWithModules()
        {
            List<Type> nancyModuleTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof (NancyModule).IsAssignableFrom(x))
                .Where(x => x.Namespace != null && !x.Namespace.StartsWith("Nancy"))
                .ToList();

            IEnumerable<string> modules =
                nancyModuleTypes
                    .Select(x => string.Format("- {0}\r\n", x.Name))
                    .ToList();

            if (!modules.Any())
            {
                modules = new List<string> {"None"};
            }

            Version version = nancyModuleTypes.First().Assembly.GetName().Version;

            string message = string.Format("Ironhide API v{0}\r\n\r\nModules:\r\n{1}",
                version, string.Join("", modules));
            return message;
        }
    }
}