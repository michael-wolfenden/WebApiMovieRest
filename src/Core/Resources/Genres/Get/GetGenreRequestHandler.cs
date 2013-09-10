using System.Linq;
using WebApiMovieRest.Core.Persistence;
using WebApiMovieRest.Infrastructure;
using WebApiMovieRest.Infrastructure.Exceptions;
using WebApiMovieRest.Infrastructure.Extensions;

namespace WebApiMovieRest.Core.Resources.Genres.Get
{
    public class GetGenreRequestHandler : IRequestHandler<GetGenreRequest, GenresResponse>
    {
        private readonly IWebApiMovieRestContext _context;

        public GetGenreRequestHandler(IWebApiMovieRestContext context)
        {
            _context = context;
        }

        public GenresResponse Handle(GetGenreRequest request)
        {
            var genre = _context.Genres.SingleOrDefault(g => g.Id == request.GenreId);
            if (genre == null) throw new ResourceNotFoundException("Genre[Id={0}] could not be found", request.GenreId);

            return genre.Yield().ToResponse();
        }
    }
}