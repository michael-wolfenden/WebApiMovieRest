using System;
using System.Web.Http.Filters;

namespace WebApiMovieRest.Infrastructure.Tests.Builders
{
    public class HttpActionExecutedContextBuilder
    {
        private Exception _exception;

        public HttpActionExecutedContextBuilder WithException(Exception exception)
        {
            _exception = exception;
            return this;
        }

        public HttpActionExecutedContext Build()
        {
            return new HttpActionExecutedContext(new HttpActionContextBuilder(), _exception);
        }

        public static implicit operator HttpActionExecutedContext(HttpActionExecutedContextBuilder builder)
        {
            return builder.Build();
        }
    }
}