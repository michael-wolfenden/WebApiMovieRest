using System;
using WebApiMovieRest.Infrastructure;

namespace WebApiMovieRest.Core.Resources.Movies.Get
{
    public class GetMovieRequest : IRequest<MoviesResponse>
    {
        public Guid MovieId { get; set; }
    }
}