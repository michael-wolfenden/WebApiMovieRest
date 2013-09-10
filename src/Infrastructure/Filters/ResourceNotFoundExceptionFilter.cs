using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using WebApiMovieRest.Infrastructure.Exceptions;

namespace WebApiMovieRest.Infrastructure.Filters
{
    public class ResourceNotFoundExceptionFilter: ExceptionFilterAttribute 
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var resourceNotFoundException = actionExecutedContext.Exception as ResourceNotFoundException;
            if (resourceNotFoundException != null)
            {
                actionExecutedContext.Response = actionExecutedContext
                    .Request
                    .CreateResponse(HttpStatusCode.NotFound, resourceNotFoundException.Message);
            }
        }
    }
}