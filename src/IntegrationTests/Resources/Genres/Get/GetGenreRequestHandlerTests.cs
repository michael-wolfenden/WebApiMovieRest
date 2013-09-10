using System;
using FluentAssertions;
using WebApiMovieRest.Core.Domain;
using WebApiMovieRest.Core.Persistence;
using WebApiMovieRest.Core.Resources.Genres;
using WebApiMovieRest.Core.Resources.Genres.Get;
using WebApiMovieRest.Infrastructure.Exceptions;
using WebApiMovieRest.Infrastructure.Extensions;
using WebApiMovieRest.IntegrationTests.Helpers;
using Xbehave;
using Xunit;

namespace WebApiMovieRest.IntegrationTests.Resources.Genres.Get
{
    public class GetGenreRequestHandlerTests : DatabaseTest<WebApiMovieRestContext>
    {
        [Scenario]
        public void Should_return_genre_translated_to_a_genres_response_when_genre_is_found(
            IWebApiMovieRestContext context,
            GetGenreRequestHandler getGenreRequestHandler,
            Genre newGenre,
            GetGenreRequest getGenreRequest,
            GenresResponse genresResponse
            )
        {
            "Given a WebApiMovieRestContext"
                .Given(() => context = new WebApiMovieRestContext().AutoRollback());

            "And a GetGenreRequestHandler constructed with the context"
                .And(() => getGenreRequestHandler = new GetGenreRequestHandler(context));

            "And a new Genre that has been inserted into the database"
                .And(() => newGenre = Db.InsertGenre(new Genre("a new genre")));

            "And a GetGenreRequest containing the id of the newly inserted Genre".
                And(() => getGenreRequest = new GetGenreRequest() {GenreId = newGenre.Id});

            "After handling the GetGenreRequest"
                .When(() => genresResponse = getGenreRequestHandler.Handle(getGenreRequest));

            "The GenresResponse should be the newly inserted Genre translated"
                .Then(() => genresResponse.ShouldBeEquivalentTo(newGenre.Yield().ToResponse()));
        }

        [Scenario]
        public void Should_throw_exception_when_genre_is_not_found(
            IWebApiMovieRestContext context,
            GetGenreRequestHandler getGenreRequestHandler,
            GetGenreRequest getGenreRequest,
            Exception exception
            )
        {
            "Given a WebApiMovieRestContext"
                .Given(() => context = new WebApiMovieRestContext().AutoRollback());

            "And a GetGenreRequestHandler constructed with the context"
                .And(() => getGenreRequestHandler = new GetGenreRequestHandler(context));

            "And a GetGenreRequest containing the id of a Genre that does not exist".
                And(() => getGenreRequest = new GetGenreRequest() { GenreId = Guid.Empty });

            "When handling the GetGenreRequest"
                .When(() => exception = Record.Exception(() =>  getGenreRequestHandler.Handle(getGenreRequest)));

            "A ResourceNotFoundException should be thrown"
                .Then(() => exception.Should().BeOfType<ResourceNotFoundException>());

            "With a message indicating the id of the Genre that was not found"
                .Then(() => exception.Message.Should().Be("Genre[Id=00000000-0000-0000-0000-000000000000] could not be found"));
        }
    }
}