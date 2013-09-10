using System.Linq;
using FluentAssertions;
using WebApiMovieRest.Core.Persistence;
using WebApiMovieRest.Core.Resources.Genres;
using WebApiMovieRest.Core.Resources.Genres.GetAll;
using WebApiMovieRest.IntegrationTests.Helpers;
using Xbehave;

namespace WebApiMovieRest.IntegrationTests.Resources.Genres.GetAll
{
    public class GetGenresRequestHandlerTests : DatabaseTest<WebApiMovieRestContext>
    {
        [Scenario]
        public void Should_return_all_genres_translated_to_a_genres_response(
            IWebApiMovieRestContext context,
            GetGenresRequestHandler getGenresRequestHandler,
            GetGenresRequest getGenreRequest,
            GenresResponse genresResponse
            )
        {
            "Given a WebApiMovieRestContext"
                .Given(() => context = new WebApiMovieRestContext().AutoRollback());

            "And a GetGenresRequestHandler constructed with the context"
                .And(() => getGenresRequestHandler = new GetGenresRequestHandler(context));

            "And a GetGenresRequest".
                And(() => getGenreRequest = new GetGenresRequest());

            "After handling the GetGenresRequest"
                .When(() => genresResponse = getGenresRequestHandler.Handle(getGenreRequest));

            "The GenresResponse should be all existing Genres in the database translated".Then(() =>
            {
                var existingGenresTranslated = new WebApiMovieRestInitializer()
                    .SeedMovies()
                    .SelectMany(movie => movie.Genres)
                    .Distinct()
                    .ToResponse();

                genresResponse.ShouldBeEquivalentTo(
                    existingGenresTranslated,
                    // do not compare ids
                    o => o.Excluding(x => x.PropertyInfo.Name == "Id") 
                );
            });
        } 
    }
}