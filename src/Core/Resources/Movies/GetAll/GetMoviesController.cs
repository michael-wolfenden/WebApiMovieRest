using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiMovieRest.Infrastructure;

namespace WebApiMovieRest.Core.Resources.Movies.GetAll
{
    [RoutePrefix("api/v1")]
    public class GetMoviesController : ApiController
    {
        private readonly IRequestHandler<GetMoviesRequest, MoviesResponse> _handler;

        public GetMoviesController(IRequestHandler<GetMoviesRequest, MoviesResponse> handler)
        {
            _handler = handler;
        }

        [HttpGet("movies.{ext}")]
        [HttpGet("movies")]
        public HttpResponseMessage Process([FromUri] GetMoviesRequest request)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _handler.Handle(request));
        }
    }
}