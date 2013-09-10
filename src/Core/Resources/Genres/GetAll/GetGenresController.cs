using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiMovieRest.Infrastructure;

namespace WebApiMovieRest.Core.Resources.Genres.GetAll
{
    [RoutePrefix("api/v1")]
    public class GetGenresController : ApiController
    {
        private readonly IRequestHandler<GetGenresRequest, GenresResponse> _handler;

        public GetGenresController(IRequestHandler<GetGenresRequest, GenresResponse> handler)
        {
            _handler = handler;
        }

        [HttpGet("genres.{ext}")]
        [HttpGet("genres")]
        public HttpResponseMessage Process([FromUri] GetGenresRequest request)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _handler.Handle(request));
        }
    }
}