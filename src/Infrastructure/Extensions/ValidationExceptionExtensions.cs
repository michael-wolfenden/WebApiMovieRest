using System.Web.Http.ModelBinding;
using FluentValidation;

namespace WebApiMovieRest.Infrastructure.Extensions
{
    public static class ValidationExceptionExtensions
    {
        public static ModelStateDictionary ToModelState(this ValidationException validationException)
        {
            var modelState = new ModelStateDictionary();
            foreach (var validationFailure in validationException.Errors)
            {
                modelState.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);
            }
            return modelState;
        }
    }
}