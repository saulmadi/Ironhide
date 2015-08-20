using Nancy;

namespace Ironhide.Web.Api.Infrastructure
{
    public class RootModule : NancyModule
    {
        public RootModule()
        {
            Get["/"] = _ => "Ironhide";
        }
    }
}