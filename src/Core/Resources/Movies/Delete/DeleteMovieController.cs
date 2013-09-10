using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiMovieRest.Infrastructure;

namespace WebApiMovieRest.Core.Resources.Movies.Delete
{
    [RoutePrefix("api/v1")]
    public class DeleteMovieController : ApiController
    {
        private readonly IRequestHandler<DeleteMovieRequest, VoidResponse> _handler;

        public DeleteMovieController(IRequestHandler<DeleteMovieRequest, VoidResponse> handler)
        {
            _handler = handler;
        }

        [HttpDelete("movies/{movieId:guid}")]
        public HttpResponseMessage Process([FromUri] DeleteMovieRequest request)
        {
            _handler.Handle(request);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}

