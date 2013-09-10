using System;
using FluentAssertions;
using WebApiMovieRest.Core.Domain;
using WebApiMovieRest.Core.Persistence;
using WebApiMovieRest.Core.Resources.Movies.Delete;
using WebApiMovieRest.Infrastructure.Exceptions;
using WebApiMovieRest.IntegrationTests.Builders;
using WebApiMovieRest.IntegrationTests.Helpers;
using Xbehave;
using Xunit;

namespace WebApiMovieRest.IntegrationTests.Resources.Movies.Delete
{
    public class DeleteMovieRequestHandlerTests : DatabaseTest<WebApiMovieRestContext>
    {
        [Scenario]
        public void Should_delete_movie_when_movie_is_found(
            IWebApiMovieRestContext context,
            DeleteMovieRequestHandler deleteMovieRequestHandler,
            Movie newMovie,
            DeleteMovieRequest deleteMovieRequest
            )
        {
            "Given a WebApiMovieRestContext"
                .Given(() => context = new WebApiMovieRestContext().AutoRollback());

            "And a DeleteMovieRequestHandler constructed with the context"
                .And(() => deleteMovieRequestHandler = new DeleteMovieRequestHandler(context));

            "And a new Movie that has been inserted into the database"
                .And(() => newMovie = Db.InsertMovie(new MovieBuilder()));

            "And a DeleteMovieRequest containing the id of the newly inserted Movie".
                And(() => deleteMovieRequest = new DeleteMovieRequest() { MovieId = newMovie.Id });

            "After handling the DeleteMovieRequest"
                .When(() => deleteMovieRequestHandler.Handle(deleteMovieRequest));

            "And commiting the WebApiMovieRestContext"
                .When(() => context.Commit());

            "Should have deleted the movie from the database"
                .Then(() => Db.GetMovieById(newMovie.Id).Should().BeNull());
        }

        [Scenario]
        public void Should_throw_exception_when_movie_is_not_found(
            IWebApiMovieRestContext context,
            DeleteMovieRequestHandler deleteMovieRequestHandler,
            DeleteMovieRequest deleteMovieRequest,
            Exception exception
            )
        {
            "Given a WebApiMovieRestContext"
                .Given(() => context = new WebApiMovieRestContext().AutoRollback());

            "And a DeleteMovieRequestHandler constructed with the context"
                .And(() => deleteMovieRequestHandler = new DeleteMovieRequestHandler(context));

            "And a DeleteMovieRequest containing the id of a Movie that does not exist".
                And(() => deleteMovieRequest = new DeleteMovieRequest() { MovieId = Guid.Empty });

            "When handling the DeleteMovieRequest"
                .When(() => exception = Record.Exception(() => deleteMovieRequestHandler.Handle(deleteMovieRequest)));

            "A ResourceNotFoundException should be thrown"
                .Then(() => exception.Should().BeOfType<ResourceNotFoundException>());

            "With a message indicating the id of the Movie that was not found"
                .Then(() => exception.Message.Should().Be("Movie[Id=00000000-0000-0000-0000-000000000000] could not be found"));
        }
    }
}