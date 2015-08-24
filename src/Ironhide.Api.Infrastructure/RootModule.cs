using Nancy;

namespace Ironhide.Api.Infrastructure
{
    public class RootModule : NancyModule
    {
        public RootModule()
        {
            Get["/"] = _ => "Ironhide API v" + GetType().Assembly.GetName().Version;
        }
    }
}