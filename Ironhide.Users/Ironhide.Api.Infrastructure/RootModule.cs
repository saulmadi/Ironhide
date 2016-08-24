using Nancy;

namespace Ironhide.Api.Infrastructure
{
    public class RootModule : NancyModule
    {
        public RootModule()
        {
            Get["/"] =
                _ => StartupMessages.GetStartupMessageWithModules().Replace("\r\n", "<br>");
        }
    }
}