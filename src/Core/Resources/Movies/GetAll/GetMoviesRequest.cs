using WebApiMovieRest.Infrastructure;

namespace WebApiMovieRest.Core.Resources.Movies.GetAll
{
    public class GetMoviesRequest : IRequest<MoviesResponse>
    {
        public string Genre { get; set; }
    }
}