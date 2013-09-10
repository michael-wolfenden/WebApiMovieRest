using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using FluentValidation;
using WebApiMovieRest.Infrastructure.Extensions;

namespace WebApiMovieRest.Infrastructure.Filters
{
    public class ValidationExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var validationException = actionExecutedContext.Exception as ValidationException;
            if (validationException != null)
            {
                //NOTE [wolfenden on 08-17-2013] should this be a 422 exception ?

                actionExecutedContext.Response = actionExecutedContext
                    .Request
                    .CreateErrorResponse(HttpStatusCode.BadRequest, validationException.ToModelState());
            }
        }
    }
}