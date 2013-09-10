using System.Web.Http.Controllers;

namespace WebApiMovieRest.Infrastructure.Tests.Builders
{
    public class HttpActionContextBuilder
    {
        public HttpActionContext Build()
        {
            var actionContext = new HttpActionContext
            {
                ControllerContext = new HttpControllerContext
                {
                    Request = new HttpRequestBuilder()
                }
            };

            return actionContext;
        }

        public static implicit operator HttpActionContext(HttpActionContextBuilder builder)
        {
            return builder.Build();
        }         
    }
}