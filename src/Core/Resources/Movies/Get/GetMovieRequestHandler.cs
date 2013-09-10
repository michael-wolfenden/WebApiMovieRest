using System.Data.Entity;
using System.Linq;
using WebApiMovieRest.Core.Persistence;
using WebApiMovieRest.Infrastructure;
using WebApiMovieRest.Infrastructure.Exceptions;
using WebApiMovieRest.Infrastructure.Extensions;

namespace WebApiMovieRest.Core.Resources.Movies.Get
{
    public class GetMovieRequestHandler : IRequestHandler<GetMovieRequest, MoviesResponse>
    {
        private readonly IWebApiMovieRestContext _context;

        public GetMovieRequestHandler(IWebApiMovieRestContext context)
        {
            _context = context;
        }

        public MoviesResponse Handle(GetMovieRequest request)
        {
            var movie = _context.Movies
                .Include(m => m.Genres)
                .SingleOrDefault(m => m.Id == request.MovieId);

            if (movie == null) throw new ResourceNotFoundException("Movie[Id={0}] could not be found", request.MovieId);

            return movie.Yield().ToResponse();
        }
    }
}