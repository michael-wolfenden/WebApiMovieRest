using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;

namespace WebApiMovieRest.Infrastructure.Tests.Builders
{
    public class HttpRequestBuilder
    {
        public HttpRequestMessage Build()
        {
            var request = new HttpRequestMessage {};

            var httpConfiguration = new HttpConfiguration();
            request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, httpConfiguration);

            return request;
        }

        public static implicit operator HttpRequestMessage(HttpRequestBuilder builder)
        {
            return builder.Build();
        } 
    }
}