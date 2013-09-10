using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using WebApiMovieRest.Infrastructure.Filters;
using WebApiMovieRest.Infrastructure.Tests.Builders;
using WebApiMovieRest.Infrastructure.Tests.Helpers;
using Xbehave;

namespace WebApiMovieRest.Infrastructure.Tests.Filters
{
    public class ValidationExceptionFilterTests
    {
        [Scenario]
        public void Should_create_a_400_response_when_validation_exception_raised(
            ValidationFailure validationFailure,
            ValidationException validationException,
            ValidationExceptionFilter exceptionFilter,
            HttpActionExecutedContext actionExecutedContext,
            ModelStateDictionary modelStateDictionary,
            HttpError httpError)
        {
            "Given a ValidationExceptionFilter"
                .Given(() => exceptionFilter = new ValidationExceptionFilter());

            "And a VailidationFailure with property name 'PropertyName' and error 'Error message'"
                .And(() => validationFailure = new ValidationFailure("PropertyName", "Error message"));

            "And a ValidationException constructed with the VailidationFailure"
                .And(() => validationException = new ValidationException(new[] { validationFailure }));

            "And a HttpActionExecutedContext containing the ValidationException"
                .And(() => actionExecutedContext = new HttpActionExecutedContextBuilder().WithException(validationException));

            "When the OnException event is raised with the HttpActionExecutedContext"
                .When(() => exceptionFilter.OnException(actionExecutedContext));

            "Then the response should have status code 400"
                .Then(() => actionExecutedContext.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest));

            "And have a HttpError as the content"
                .Then(() => (httpError = actionExecutedContext.Response.ContentAs<HttpError>()).Should().NotBeNull());

            "With a model error for key 'PropertyName'"
                .Then(() => httpError.ReconstructModelState().ContainsKey("PropertyName").Should().BeTrue());

            "And the model error should have an error message 'Error message'"
                .Then(() => httpError.ReconstructModelState().ErrorMessageForKey("PropertyName").Should().Be("Error message"));
        }
    }
}