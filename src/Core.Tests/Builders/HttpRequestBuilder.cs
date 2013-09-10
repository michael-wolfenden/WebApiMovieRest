using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;

namespace WebApiMovieRest.Core.Tests.Builders
{
    public class HttpRequestBuilder
    {
        private Action<HttpConfiguration> _configureRoutes = null;

        public HttpRequestBuilder ConfigureRoutes(Action<HttpConfiguration> configureRoutes)
        {
            _configureRoutes = configureRoutes;
            return this;
        }

        public HttpRequestMessage Build()
        {
            var request = new HttpRequestMessage();

            var httpConfiguration = new HttpConfiguration();
            request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, httpConfiguration);

            if (_configureRoutes != null)
            {
                _configureRoutes(httpConfiguration);

                // RequestUri must be a valid registered route for any routing methods (such as Url.Link) to work
                request.RequestUri = new Uri("http://localhost/api/v1/movies");
                request.Properties[HttpPropertyKeys.HttpRouteDataKey] = httpConfiguration.Routes.GetRouteData(request);
            }

            return request;
        }

        public static implicit operator HttpRequestMessage(HttpRequestBuilder builder)
        {
            return builder.Build();
        } 
    }
}