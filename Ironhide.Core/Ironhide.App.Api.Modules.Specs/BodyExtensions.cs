using Nancy.Testing;

namespace Ironhide.App.Api.Modules.Specs
{
    public static class BodyExtensions
    {
        public static T Body<T>(this BrowserResponse response)
        {
            if (response == null)
            {
                throw new AssertException("BrowserResponse was null.");
            }
            var body = response.Body.DeserializeJson<T>();
            if (body == null)
            {
                throw new AssertException("Body was null.");
            }
            return body;
        }
    }
}