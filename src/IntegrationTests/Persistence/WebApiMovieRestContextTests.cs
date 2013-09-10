using System;
using System.Collections.Generic;
using WebApiMovieRest.Core.Domain;
using WebApiMovieRest.Core.Persistence;
using WebApiMovieRest.IntegrationTests.Helpers;
using Xbehave;
using System.Linq;
using System.Data.Entity;
using FluentAssertions;

namespace WebApiMovieRest.IntegrationTests.Persistence
{
    public class WebApiMovieRestContextTests : DatabaseTest<WebApiMovieRestContext>
    {
        [Scenario]
        public void Should_seed_database_on_initialization(
            WebApiMovieRestContext context
            )
        {
            "Given a WebApiMovieRestContext"
                .Given(() => context = new WebApiMovieRestContext().Using());

            "When initializing"
                .When(() => context.Database.Initialize(false));

            "Should seed the database"
                .Then(() =>
                {
                    var expectedMovies = new WebApiMovieRestInitializer().SeedMovies();
                    
                    var retrievedMovies = context.Movies
                        .Include(movie => movie.Genres)
                        .ToList();

                    ShouldBeEquivalent(retrievedMovies, expectedMovies);
                });
        }

        public static void ShouldBeEquivalent(IEnumerable<Movie> actualMovies, IEnumerable<Movie> expectedMovies)
        {
            // we cannot just compare movies as the genres may be in diffent orders, so we have to create a new type
            // with the genres ordered
            Func<Movie, object> toAnonymousType = movie => new
            {
                movie.Director,
                Genres = movie.Genres.Select(genre => genre.Name).OrderBy(genre => genre),
                movie.ImdbId,
                movie.Rating,
                movie.ReleaseDate,
                movie.Plot,
                movie.Title
            };

            var actual = actualMovies.OrderBy(movie => movie.Title)
                                     .Select(toAnonymousType);

            var expected = expectedMovies.OrderBy(movie => movie.Title)
                                         .Select(toAnonymousType);

            actual.ShouldBeEquivalentTo(expected);
        }
    }
}