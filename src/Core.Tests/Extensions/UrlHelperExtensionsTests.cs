using System;
using System.Net.Http;
using FluentAssertions;
using WebApiMovieRest.Core.Extensions;
using WebApiMovieRest.Core.StartupTasks;
using WebApiMovieRest.Core.Tests.Builders;
using Xbehave;

namespace WebApiMovieRest.Core.Tests.Extensions
{
    public class UrlHelperExtensionsTests
    {
        [Scenario]
        public void Should_be_able_to_generate_link_to_get_movie_by_id(
            HttpRequestMessage request,
            Uri getMovieByIdUri
            )
        {
            "Given a request with all routes registered"
                .Given(() => request = new HttpRequestBuilder()
                            .ConfigureRoutes(config => new RegisterRoutes(config).Start()));

            "Calling GetMovieById on the UrlHelper created from the request"
                .When(() => getMovieByIdUri = request.GetUrlHelper().GetMovieById(Guid.Empty));

            "Should generate the uri to get a movie by id"
                .Then(() => getMovieByIdUri.ToString().Should().Be("http://localhost/api/v1/movies/00000000-0000-0000-0000-000000000000"));
        }
    }
}