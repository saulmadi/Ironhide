using System;
using System.Web;

namespace Ironhide.Api.Infrastructure
{
    public class BaseUrlProvider : IBaseUrlProvider
    {
        public string GetBaseUrl()
        {
            Uri url = HttpContext.Current.Request.Url;
            string baseUrl = string.Format("{0}://{1}:{2}", url.Scheme, url.Host, url.Port);
            return baseUrl;
        }
    }
}