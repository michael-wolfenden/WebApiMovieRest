using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiMovieRest.Infrastructure;

namespace WebApiMovieRest.Core.Resources.Movies.Update
{
    [RoutePrefix("api/v1")]
    public class UpdateMovieController : ApiController
    {
        private readonly IRequestHandler<UpdateMovieRequest, MoviesResponse> _handler;

        public UpdateMovieController(IRequestHandler<UpdateMovieRequest, MoviesResponse> handler)
        {
            _handler = handler;
        }

        [HttpPut("movies/{movieId:guid}.{ext}")]
        [HttpPut("movies/{movieId:guid}")]
        public HttpResponseMessage Process([FromUri] Guid movieId, UpdateMovieRequest request)
        {
            request.MovieId = movieId;
            return Request.CreateResponse(HttpStatusCode.OK, _handler.Handle(request));
        }
    }
}

