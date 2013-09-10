using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack.Common.Extensions;
using WebApiMovieRest.Core.Domain;
using WebApiMovieRest.Core.Persistence;
using WebApiMovieRest.Infrastructure;
using WebApiMovieRest.Infrastructure.Extensions;

namespace WebApiMovieRest.Core.Resources.Movies.Create
{
    public class CreateMovieRequestHandler : IRequestHandler<CreateMovieRequest, MoviesResponse>
    {
        private readonly IWebApiMovieRestContext _context;

        public CreateMovieRequestHandler(IWebApiMovieRestContext context)
        {
            _context = context;
        }

        public MoviesResponse Handle(CreateMovieRequest request)
        {
            var existingGenres = _context.Genres.ToList();

            var movie = new Movie(
                request.ImdbId,
                request.Title,
                request.Rating,
                request.Director,
                request.ReleaseDate.Value,
                request.Plot,
                GetExistingOrCreateNewGenreFromNames(request.Genres, existingGenres)
            );

            _context.Movies.Add(movie);

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