using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiMovieRest.Infrastructure;

namespace WebApiMovieRest.Core.Resources.Genres.Get
{
    [RoutePrefix("api/v1")]
    public class GetGenreController : ApiController
    {
        private readonly IRequestHandler<GetGenreRequest, GenresResponse> _handler;

        public GetGenreController(IRequestHandler<GetGenreRequest, GenresResponse> handler)
        {
            _handler = handler;
        }

        [HttpGet("genres/{genreId:guid}.{ext}")]
        [HttpGet("genres/{genreId:guid}")]
        public HttpResponseMessage Process([FromUri] GetGenreRequest request)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _handler.Handle(request));
        }
    }
}

