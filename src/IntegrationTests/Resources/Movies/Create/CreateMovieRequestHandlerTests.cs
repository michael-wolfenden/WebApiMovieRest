using System.Data.Entity;
using System.Linq;
using FluentAssertions;
using WebApiMovieRest.Core.Domain;
using WebApiMovieRest.Core.Persistence;
using WebApiMovieRest.Core.Resources.Movies;
using WebApiMovieRest.Core.Resources.Movies.Create;
using WebApiMovieRest.Infrastructure.Extensions;
using WebApiMovieRest.IntegrationTests.Builders;
using WebApiMovieRest.IntegrationTests.Helpers;
using Xbehave;

namespace WebApiMovieRest.IntegrationTests.Resources.Movies.Create
{
    public class CreateMovieRequestHandlerTests : DatabaseTest<WebApiMovieRestContext>
    {
        [Scenario]
        public void Should_create_movie_and_create_genre_if_genre_does_not_exist(
            IWebApiMovieRestContext context,
            CreateMovieRequestHandler createMovieRequestHandler,
            Movie newMovie,
            CreateMovieRequest createMovieRequest,
            MoviesResponse moviesResponse,
            Movie createdMovie
            )
        {
            "Given a WebApiMovieRestContext"
                .Given(() => context = new WebApiMovieRestContext().AutoRollback());

            "And a CreateMovieRequestHandler constructed with the context"
                .And(() => createMovieRequestHandler = new CreateMovieRequestHandler(context));

            "And a CreateMovieRequest containing a genre that doesnt exist in the database".
                And(() => createMovieRequest = new CreateMovieRequestBuilder().WithGenres(new[] {"a new genre"}));

            "After handling the CreateMovieRequest"
                .When(() => moviesResponse = createMovieRequestHandler.Handle(createMovieRequest));

            "And commiting the WebApiMovieRestContext"
                .When(() => context.Commit());

            "Then a Movie should have be created in the database".Then(() =>
            {
                var createdMovieId = moviesResponse.Movies.First().Id;
                createdMovie = Db.GetMovieById(createdMovieId);

                createdMovie.Should().NotBeNull();
            });

            "And a Genre should have be created in the database"
                .Then(() => Db.GetGenreByName("a new genre").Should().NotBeNull());

            "And the new movie should have the same values as the request"
                .Then(() => createdMovie.ShouldBeEquivalentTo(createMovieRequest, o => o.ExcludingMissingProperties()));

            "And the new movie should have the same genre as the request"
                .Then(() => createdMovie.Genres.Single().Name.Should().Be(createMovieRequest.Genres[0]));

            "And the MovieResponse should be the newly created Movie translated"
                .Then(() => moviesResponse.ShouldBeEquivalentTo(createdMovie.Yield().ToResponse()));
        }

        [Scenario]
        public void Should_create_movie_and_use_existing_genre_if_genre_exists(
            IWebApiMovieRestContext context,
            CreateMovieRequestHandler createMovieRequestHandler,
            Genre alreadyExistingGenre,
            Movie newMovie,
            CreateMovieRequest createMovieRequest,
            MoviesResponse moviesResponse,
            Movie createdMovie
            )
        {
            "Given a WebApiMovieRestContext"
                .Given(() => context = new WebApiMovieRestContext().AutoRollback());

            "And a CreateMovieRequestHandler constructed with the context"
                .And(() => createMovieRequestHandler = new CreateMovieRequestHandler(context));

            "And a Genre that already exists in the database".And(() =>
            {
                var alreadyExistingGenreName = new WebApiMovieRestInitializer()
                                                    .SeedMovies().First()
                                                    .Genres.First()
                                                    .Name;

                alreadyExistingGenre = Db.GetGenreByName(alreadyExistingGenreName);
            });

            "And a CreateMovieRequest containing a genre that exists in the database".
                And(() => createMovieRequest = new CreateMovieRequestBuilder().WithGenres(new[] { alreadyExistingGenre.Name }));

            "After handling the CreateMovieRequest"
                .When(() => moviesResponse = createMovieRequestHandler.Handle(createMovieRequest));

            "And commiting the WebApiMovieRestContext"
                .When(() => context.Commit());

            "Then a Movie should have be created in the database".Then(() =>
            {
                var createdMovieId = moviesResponse.Movies.First().Id;
                createdMovie = Db.GetMovieById(createdMovieId);

                createdMovie.Should().NotBeNull();
            });

            "And the already existing Genre should have be used".Then(() => 
                createdMovie.Genres.Single().ShouldBeEquivalentTo(alreadyExistingGenre));

            "And the new movie should have the same values as the request"
                .Then(() => createdMovie.ShouldBeEquivalentTo(createMovieRequest, o => o.ExcludingMissingProperties()));

            "And the new movie should have the same genre as the request"
                .Then(() => createdMovie.Genres.Single().Name.Should().Be(createMovieRequest.Genres[0]));

            "And the MovieResponse should be the newly created Movie translated"
                .Then(() => moviesResponse.ShouldBeEquivalentTo(createdMovie.Yield().ToResponse()));
        }
    }
}