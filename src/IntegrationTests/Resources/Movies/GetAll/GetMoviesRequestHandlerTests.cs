using System.Linq;
using FluentAssertions;
using WebApiMovieRest.Core.Persistence;
using WebApiMovieRest.Core.Resources.Movies;
using WebApiMovieRest.Core.Resources.Movies.GetAll;
using WebApiMovieRest.IntegrationTests.Helpers;
using Xbehave;

namespace WebApiMovieRest.IntegrationTests.Resources.Movies.GetAll
{
    public class GetMoviesRequestHandlerTests : DatabaseTest<WebApiMovieRestContext>
    {
        [Scenario]
        public void Should_return_all_movie_translated_to_a_movies_response_when_no_genre_specified_in_request(
            IWebApiMovieRestContext context,
            GetMoviesRequestHandler getMoviesRequestHandler,
            GetMoviesRequest getMovieRequest,
            MoviesResponse moviesResponse
            )
        {
            "Given a WebApiMovieRestContext"
                .Given(() => context = new WebApiMovieRestContext().AutoRollback());

            "And a GetMoviesRequestHandler constructed with the context"
                .And(() => getMoviesRequestHandler = new GetMoviesRequestHandler(context));

            "And a GetMoviesRequest with no genre specified".
                And(() => getMovieRequest = new GetMoviesRequest());

            "After handling the GetMoviesRequest"
                .When(() => moviesResponse = getMoviesRequestHandler.Handle(getMovieRequest));

            "The MovieResponse should be all existing Movies in the database translated".Then(() =>
            {
                var existingMoviesTranslated = new WebApiMovieRestInitializer()
                    .SeedMovies()
                    .ToResponse();

                moviesResponse.ShouldBeEquivalentTo(
                    existingMoviesTranslated,
                    // do not compare ids or links (as ids will be different)
                    o => o.Excluding(x => x.PropertyInfo.Name == "Id" || x.PropertyInfo.Name == "Links")
                );
            });
        }

        [Scenario]
        public void Should_return_all_movie_translated_to_a_movies_response_filtered_by_genre_when_genre_specified_in_request(
            IWebApiMovieRestContext context,
            GetMoviesRequestHandler getMoviesRequestHandler,
            GetMoviesRequest getMovieRequest,
            MoviesResponse moviesResponse
            )
        {
            "Given a WebApiMovieRestContext"
                .Given(() => context = new WebApiMovieRestContext().AutoRollback());

            "And a GetMoviesRequestHandler constructed with the context"
                .And(() => getMoviesRequestHandler = new GetMoviesRequestHandler(context));

            "And a GetMoviesRequest with a genre specified".
                And(() => getMovieRequest = new GetMoviesRequest() { Genre = "Crime" });

            "After handling the GetMoviesRequest"
                .When(() => moviesResponse = getMoviesRequestHandler.Handle(getMovieRequest));

            "The MovieResponse should be all existing Movies in the database filtered by the specified genre and translated".Then(() =>
            {
                var existingMoviesFilterAndTranslated = new WebApiMovieRestInitializer()
                    .SeedMovies()
                    .Where(movie => movie.Genres.Any(genre => genre.Name == "Crime"))
                    .ToResponse();

                moviesResponse.ShouldBeEquivalentTo(
                    existingMoviesFilterAndTranslated,
                    // do not compare ids or links (as ids will be different)
                    o => o.Excluding(x => x.PropertyInfo.Name == "Id" || x.PropertyInfo.Name == "Links")
                );
            });
        } 
    }
}