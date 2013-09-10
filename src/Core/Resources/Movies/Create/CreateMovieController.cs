using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiMovieRest.Core.Extensions;
using WebApiMovieRest.Infrastructure;

namespace WebApiMovieRest.Core.Resources.Movies.Create
{
    [RoutePrefix("api/v1")]
    public class CreateMovieController : ApiController
    {
        private readonly IRequestHandler<CreateMovieRequest, MoviesResponse> _handler;

        public CreateMovieController(IRequestHandler<CreateMovieRequest, MoviesResponse> handler)
        {
            _handler = handler;
        }

        [HttpPost("movies.{ext}")]
        [HttpPost("movies")]
        public HttpResponseMessage Process(CreateMovieRequest request)
        {
            MoviesResponse moviesResponse = _handler.Handle(request);
            var createdMovieId = moviesResponse.Movies.First().Id;

            var response = Request.CreateResponse(HttpStatusCode.Created, moviesResponse);
            response.Headers.Location = Url.GetMovieById(createdMovieId);

            return response;
        }
    }
}