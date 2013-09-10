using System.Linq;
using System.Web.Http.ModelBinding;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using WebApiMovieRest.Infrastructure.Extensions;
using WebApiMovieRest.Infrastructure.Tests.Helpers;
using Xbehave;

namespace WebApiMovieRest.Infrastructure.Tests.Extensions
{
    public class ValidationExceptionExtensionTests
    {
        [Scenario]
        public void Name(
            ValidationFailure validationFailure,
            ValidationException validationException,
            ModelStateDictionary modelState)
        {
            "Given a VailidationFailure with property name 'PropertyName' and error 'Error message'"
                .Given(() => validationFailure = new ValidationFailure("PropertyName", "Error message"));

            "And a ValidationException constructed with the VailidationFailure"
                .And(() => validationException = new ValidationException(new[] { validationFailure }));

            "When converting the ValidationException to a ModelStateDictionary"
                .When(() => modelState = validationException.ToModelState());

            "Then the ModelStateDictionary should contain a model error for key 'PropertyName'"
                .Then(() => modelState.ContainsKey("PropertyName").Should().BeTrue());

            "And the model error should have an error message 'Error message'"
                .Then(() => modelState.ErrorMessageForKey("PropertyName").Should().Be("Error message"));
        } 
    }
}