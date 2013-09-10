using System.Data.Entity;
using System.Linq;
using WebApiMovieRest.Core.Persistence;
using WebApiMovieRest.Infrastructure;
using WebApiMovieRest.Infrastructure.Extensions;

namespace WebApiMovieRest.Core.Resources.Movies.GetAll
{
    public class GetMoviesRequestHandler : IRequestHandler<GetMoviesRequest, MoviesResponse>
    {
        private readonly IWebApiMovieRestContext _context;

        public GetMoviesRequestHandler(IWebApiMovieRestContext context)
        {
            _context = context;
        }

        public MoviesResponse Handle(GetMoviesRequest request)
        {
            var movies = _context.Movies
                                 .Include(m => m.Genres);

            if (!request.Genre.IsNullOrWhiteSpace())
            {
                movies = movies.Where(movie => movie.Genres.Any(genre => genre.Name == request.Genre));
            }

            return movies.ToList()
                         .ToResponse();
        }
    }
}