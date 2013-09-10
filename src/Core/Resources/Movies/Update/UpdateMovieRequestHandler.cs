using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ServiceStack.Common.Extensions;
using WebApiMovieRest.Core.Domain;
using WebApiMovieRest.Core.Persistence;
using WebApiMovieRest.Infrastructure;
using WebApiMovieRest.Infrastructure.Exceptions;
using WebApiMovieRest.Infrastructure.Extensions;

namespace WebApiMovieRest.Core.Resources.Movies.Update
{
    public class UpdateMovieRequestHandler : IRequestHandler<UpdateMovieRequest, MoviesResponse>
    {
        private readonly IWebApiMovieRestContext _context;

        public UpdateMovieRequestHandler(IWebApiMovieRestContext context)
        {
            _context = context;
        }

        public MoviesResponse Handle(UpdateMovieRequest request)
        {
            var movie = _context.Movies
                .Include(m => m.Genres)
                .SingleOrDefault(m => m.Id == request.MovieId);

            if (movie == null) throw new ResourceNotFoundException("Movie[Id={0}] could not be found", request.MovieId);

            movie.ImdbId = request.ImdbId;
            movie.Title = request.Title;
            movie.Rating = request.Rating;
            movie.Director = request.Director;
            movie.ReleaseDate = request.ReleaseDate.Value;
            movie.Plot = request.Plot;

            var existingGenres = _context.Genres.ToList();
            movie.SetGenres(GetExistingOrCreateNewGenreFromNames(request.Genres, existingGenres));

            return movie.Yield().ToResponse();
        }

        private static IEnumerable<Genre> GetExistingOrCreateNewGenreFromNames(string[] genreNames, IEnumerable<Genre> existingGenres)
        {
            return genreNames
                .Select(name =>
                    existingGenres.SingleOrDefault(genre => genre.Name.EqualsIgnoreCase(name)) ??
                    new Genre(name)
                );
        }
    }
}