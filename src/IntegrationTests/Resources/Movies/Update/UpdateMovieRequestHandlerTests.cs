using System;
using System.Data.Entity;
using System.Linq;
using FluentAssertions;
using WebApiMovieRest.Core.Domain;
using WebApiMovieRest.Core.Persistence;
using WebApiMovieRest.Core.Resources.Movies;
using WebApiMovieRest.Core.Resources.Movies.Update;
using WebApiMovieRest.Infrastructure.Exceptions;
using WebApiMovieRest.Infrastructure.Extensions;
using WebApiMovieRest.IntegrationTests.Builders;
using WebApiMovieRest.IntegrationTests.Helpers;
using Xbehave;
using Xunit;

namespace WebApiMovieRest.IntegrationTests.Resources.Movies.Update
{
    public class UpdateMovieRequestHandlerTests : DatabaseTest<WebApiMovieRestContext>
    {
        [Scenario]
        public void Should_update_movie_and_create_genre_if_genre_does_not_exist(
            IWebApiMovieRestContext context,
            UpdateMovieRequestHandler updateMovieRequestHandler,
            Movie newMovie,
            UpdateMovieRequest updateMovieRequest,
            MoviesResponse moviesResponse,
            Movie updatedMovie
            )
        {
            "Given a WebApiMovieRestContext"
                .Given(() => context = new WebApiMovieRestContext().AutoRollback());

            "And a UpdateMovieRequestHandler constructed with the context"
                .And(() => updateMovieRequestHandler = new UpdateMovieRequestHandler(context));

            "And a new Movie that has been inserted into the database".And(() =>
            {
                newMovie = new MovieBuilder();

                using (var dbContext = new WebApiMovieRestContext())
                {
                    dbContext.Movies.Add(newMovie);
                    dbContext.SaveChanges();
                }
            });

            "And a UpdateMovieRequest containing the id of the newly inserted Movie and genre that doesnt exist in the database".
                And(() => updateMovieRequest = new UpdateMovieRequestBuilder()
                                                    .WithMovieId(newMovie.Id)
                                                    .WithDirector("new director")
                                                    .WithGenres(new[] { "a new genre" })
                                                    .WithImdbId("newimdbid")
                                                    .WithPlot("new plot")
                                                    .WithRating(0.1m)
                                                    .WithReleaseDate(new DateTime(2000,01,01))
                                                    .WithTitle("new title"));

            "After handling the UpdateMovieRequest"
                .When(() => moviesResponse = updateMovieRequestHandler.Handle(updateMovieRequest));

            "And commiting the WebApiMovieRestContext"
                .When(() => context.Commit());

            "And retrieving the updated Movie".When(() =>
            {
                using (var dbContext = new WebApiMovieRestContext())
                {
                    updatedMovie = dbContext.Movies
                        .Include(movie => movie.Genres)
                        .SingleOrDefault(movie => movie.Id == newMovie.Id);
                }
            });

            "Then a Genre should have be created in the database".Then(() =>
            {
                using (var dbContext = new WebApiMovieRestContext())
                {
                    dbContext.Genres
                        .SingleOrDefault(genre => genre.Name == "a new genre")
                        .Should()
                        .NotBeNull();
                }
            });

            "And the updated movie should have the same values as the request"
                .Then(() => updatedMovie.ShouldBeEquivalentTo(updateMovieRequest, o => o.ExcludingMissingProperties()));

            "And the updated movie should have the same genre as the request"
                .Then(() => updatedMovie.Genres.Single().Name.Should().Be(updateMovieRequest.Genres[0]));

            "And the MovieResponse should be the newly updated Movie translated"
                .Then(() => moviesResponse.ShouldBeEquivalentTo(updatedMovie.Yield().ToResponse()));
        }

        [Scenario]
        public void Should_update_movie_and_use_existing_genre_if_genre_exists(
            IWebApiMovieRestContext context,
            UpdateMovieRequestHandler updateMovieRequestHandler,
            Genre alreadyExistingGenre,
            Movie newMovie,
            UpdateMovieRequest updateMovieRequest,
            MoviesResponse moviesResponse,
            Movie updatedMovie
            )
        {
            "Given a WebApiMovieRestContext"
                .Given(() => context = new WebApiMovieRestContext().AutoRollback());

            "And a UpdateMovieRequestHandler constructed with the context"
                .And(() => updateMovieRequestHandler = new UpdateMovieRequestHandler(context));

            "And a new Movie that has been inserted into the database".And(() =>
            {
                newMovie = new MovieBuilder().WithGenres(new[] {new Genre("new genre")});

                using (var dbContext = new WebApiMovieRestContext())
                {
                    dbContext.Movies.Add(newMovie);
                    dbContext.SaveChanges();
                }
            });

            "And a Genre that already exists in the database".And(() =>
            {
                var alreadyExistingGenreName = new WebApiMovieRestInitializer()
                                                    .SeedMovies().First()
                                                    .Genres.First()
                                                    .Name;

                using (var dbContext = new WebApiMovieRestContext())
                {
                    alreadyExistingGenre = dbContext.Genres.Single(genre => genre.Name == alreadyExistingGenreName);
                }
            });

            "And a UpdateMovieRequest containing the id of the newly inserted Movie and genre that exists in the database".
                And(() => updateMovieRequest =  new UpdateMovieRequestBuilder()
                                                    .WithMovieId(newMovie.Id)
                                                    .WithDirector("new director")
                                                    .WithGenres(new[] { alreadyExistingGenre.Name })
                                                    .WithImdbId("newimdbid")
                                                    .WithPlot("new plot")
                                                    .WithRating(0.1m)
                                                    .WithReleaseDate(new DateTime(2000,01,01))
                                                    .WithTitle("new title"));

            "After handling the UpdateMovieRequest"
                .When(() => moviesResponse = updateMovieRequestHandler.Handle(updateMovieRequest));

            "And commiting the WebApiMovieRestContext"
                .When(() => context.Commit());

            "And retrieving the updated Movie".When(() =>
            {
                using (var dbContext = new WebApiMovieRestContext())
                {
                    updatedMovie = dbContext.Movies
                        .Include(movie => movie.Genres)
                        .SingleOrDefault(movie => movie.Id == newMovie.Id);
                }
            });

            "Then the already existing Genre should have be used".Then(() =>
                updatedMovie.Genres.Single().ShouldBeEquivalentTo(alreadyExistingGenre));

            "And the new movie should have the same values as the request"
                .Then(() => updatedMovie.ShouldBeEquivalentTo(updateMovieRequest, o => o.ExcludingMissingProperties()));

            "And the new movie should have the same genre as the request"
                .Then(() => updatedMovie.Genres.Single().Name.Should().Be(updateMovieRequest.Genres[0]));

            "And the MovieResponse should be the newly updated Movie translated"
                .Then(() => moviesResponse.ShouldBeEquivalentTo(updatedMovie.Yield().ToResponse()));
        }

        [Scenario]
        public void Should_throw_exception_when_movie_is_not_found(
            IWebApiMovieRestContext context,
            UpdateMovieRequestHandler updateMovieRequestHandler,
            UpdateMovieRequest updateMovieRequest,
            Exception exception
            )
        {
            "Given a WebApiMovieRestContext"
                .Given(() => context = new WebApiMovieRestContext().AutoRollback());

            "And a UpdateMovieRequestHandler constructed with the context"
                .And(() => updateMovieRequestHandler = new UpdateMovieRequestHandler(context));

            "And a UpdateMovieRequest containing the id of a Movie that does not exist".
                And(() => updateMovieRequest = new UpdateMovieRequest() { MovieId = Guid.Empty });

            "When handling the UpdateMovieRequest"
                .When(() => exception = Record.Exception(() => updateMovieRequestHandler.Handle(updateMovieRequest)));

            "A ResourceNotFoundException should be thrown"
                .Then(() => exception.Should().BeOfType<ResourceNotFoundException>());

            "With a message indicating the id of the Movie that was not found"
                .Then(() => exception.Message.Should().Be("Movie[Id=00000000-0000-0000-0000-000000000000] could not be found"));
        }
    }
}