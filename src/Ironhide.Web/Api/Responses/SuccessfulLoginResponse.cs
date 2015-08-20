using System;

namespace Ironhide.Web.Api.Responses
{
    public class SuccessfulLoginResponse<T>
    {
        public SuccessfulLoginResponse()
        {

        }

        public SuccessfulLoginResponse(T token)
        {
            Token = token;
        }

        public T Token { get; set; }
    }
}