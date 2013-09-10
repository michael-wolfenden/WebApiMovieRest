using System.Net;
using System.Web.Http.Filters;
using FluentAssertions;
using WebApiMovieRest.Infrastructure.Exceptions;
using WebApiMovieRest.Infrastructure.Filters;
using WebApiMovieRest.Infrastructure.Tests.Builders;
using WebApiMovieRest.Infrastructure.Tests.Helpers;
using Xbehave;

namespace WebApiMovieRest.Infrastructure.Tests.Filters
{
    public class ResourceNotFoundExceptionFilterTests
    {
        [Scenario]
        public void Should_create_a_404_response_when_resource_not_found_exception_raised( 
            ResourceNotFoundExceptionFilter exceptionFilter,
            HttpActionExecutedContext actionExecutedContext)
        {
            "Given a ResourceNotFoundExceptionFilter"
                .Given(() => exceptionFilter = new ResourceNotFoundExceptionFilter());

            "And a HttpActionExecutedContext containing a ResourceNotFoundException with message 'resource not found'"
                .And(() => actionExecutedContext = new HttpActionExecutedContextBuilder()
                                                            .WithException(new ResourceNotFoundException("resource not found")));

            "When the OnException event is raised with the HttpActionExecutedContext"
                .When(() => exceptionFilter.OnException(actionExecutedContext));

            "Then the response should have status code 404"
                .Then(() => actionExecutedContext.Response.StatusCode.Should().Be(HttpStatusCode.NotFound));

            "And have content 'resource not found'"
                .Then(() => actionExecutedContext.Response.ContentAs<string>().Should().Be("resource not found"));
        } 
    }
}