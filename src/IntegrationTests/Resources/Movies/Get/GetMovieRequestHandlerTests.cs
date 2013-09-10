using System;
using FluentAssertions;
using WebApiMovieRest.Core.Domain;
using WebApiMovieRest.Core.Persistence;
using WebApiMovieRest.Core.Resources.Movies;
using WebApiMovieRest.Core.Resources.Movies.Get;
using WebApiMovieRest.Infrastructure.Exceptions;
using WebApiMovieRest.Infrastructure.Extensions;
using WebApiMovieRest.IntegrationTests.Builders;
using WebApiMovieRest.IntegrationTests.Helpers;
using Xbehave;
using Xunit;

namespace WebApiMovieRest.IntegrationTests.Resources.Movies.Get
{
    public class GetMovieRequestHandlerTests : DatabaseTest<WebApiMovieRestContext>
    {
        [Scenario]
        public void Should_return_movie_translated_to_a_movies_response_when_movie_is_found(
            IWebApiMovieRestContext context,
            GetMovieRequestHandler getMovieRequestHandler,
            Movie newMovie,
            GetMovieRequest getMovieRequest,
            MoviesResponse moviesResponse
            )
        {
            "Given a WebApiMovieRestContext"
                .Given(() => context = new WebApiMovieRestContext().AutoRollback());

            "And a GetMovieRequestHandler constructed with the context"
                .And(() => getMovieRequestHandler = new GetMovieRequestHandler(context));

            "And a new Movie that has been inserted into the database"
                .And(() => newMovie = Db.InsertMovie(new MovieBuilder()));

            "And a GetMovieRequest containing the id of the newly inserted Movie".
                And(() => getMovieRequest = new GetMovieRequest() {MovieId = newMovie.Id});

            "After handling the GetMovieRequest"
                .When(() => moviesResponse = getMovieRequestHandler.Handle(getMovieRequest));

            "The MovieResponse should be the newly inserted Movie translated"
                .Then(() => moviesResponse.ShouldBeEquivalentTo(newMovie.Yield().ToResponse()));
        }

        [Scenario]
        public void Should_throw_exception_when_movie_is_not_found(
            IWebApiMovieRestContext context,
            GetMovieRequestHandler getMovieRequestHandler,
            GetMovieRequest getMovieRequest,
            Exception exception
            )
        {
            "Given a WebApiMovieRestContext"
                .Given(() => context = new WebApiMovieRestContext().AutoRollback());

            "And a GetMovieRequestHandler constructed with the context"
                .And(() => getMovieRequestHandler = new GetMovieRequestHandler(context));

            "And a GetMovieRequest containing the id of a Movie that does not exist".
                And(() => getMovieRequest = new GetMovieRequest() { MovieId = Guid.Empty });

            "When handling the GetMovieRequest"
                .When(() => exception = Record.Exception(() => getMovieRequestHandler.Handle(getMovieRequest)));

            "A ResourceNotFoundException should be thrown"
                .Then(() => exception.Should().BeOfType<ResourceNotFoundException>());

            "With a message indicating the id of the Movie that was not found"
                .Then(() => exception.Message.Should().Be("Movie[Id=00000000-0000-0000-0000-000000000000] could not be found"));
        }
    }
}