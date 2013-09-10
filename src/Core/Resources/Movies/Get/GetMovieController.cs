using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiMovieRest.Infrastructure;

namespace WebApiMovieRest.Core.Resources.Movies.Get
{
    [RoutePrefix("api/v1")]
    public class GetMovieController : ApiController
    {
        private readonly IRequestHandler<GetMovieRequest, MoviesResponse> _handler;

        public GetMovieController(IRequestHandler<GetMovieRequest, MoviesResponse> handler)
        {
            _handler = handler;
        }

        [HttpGet("movies/{movieId:guid}.{ext}")]
        [HttpGet("movies/{movieId:guid}", RouteName = Constants.Routes.GetMovieById)]
        public HttpResponseMessage Process([FromUri] GetMovieRequest request)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _handler.Handle(request));
        }
    }
}