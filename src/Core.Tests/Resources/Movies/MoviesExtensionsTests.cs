using System;
using System.Collections.Generic;
using FluentAssertions;
using WebApiMovieRest.Core.Domain;
using WebApiMovieRest.Core.Resources.Genres;
using WebApiMovieRest.Core.Resources.Movies;
using WebApiMovieRest.Core.Tests.Builders;
using Xbehave;

namespace WebApiMovieRest.Core.Tests.Resources.Movies
{
    public class MoviesExtensionsTests
    {
        [Scenario]
        public void Should_be_able_to_map_from_Movie_to_MovieDto(
            Movie movie,
            MovieDto movieDto
            )
        {
            var drama = new Genre("Drama");
            var crime = new Genre("Crime");

            "Given a Movie with movies reverse name order"
                .Given(() => movie = new MovieBuilder().WithGenres(new[] { drama, crime }));

            "When mapping to a MovieDto"
                .When(() => movieDto = movie.ToDto());

            "Should map id"
                .Then(() => movieDto.Id.Should()
                    .Be(movie.Id));

            "Should map imdbId"
                .Then(() => movieDto.ImdbId.Should()
                    .Be(movie.ImdbId));

            "Should map plot"
                .Then(() => movieDto.Plot.Should()
                    .Be(movie.Plot));

            "Should map rating"
                .Then(() => movieDto.Rating.Should()
                    .Be(movie.Rating));

            "Should map release date"
                .Then(() => movieDto.ReleaseDate.Should()
                    .Be(movie.ReleaseDate));

            "Should map title"
                .Then(() => movieDto.Title.Should()
                    .Be(movie.Title));

            "Should map the ids of the movies in name order as a link dto"
                .Then(() => movieDto.Links.Genres.Should().ContainInOrder(crime.Id, drama.Id));
        }

        [Scenario]
        public void Should_be_able_to_map_from_an_enumerable_of_Movies_to_MoviesResponse(
            IEnumerable<Movie> movies,
            MoviesResponse response
            )
        {
            var crime = new Genre("Crime");
            var drama = new Genre("Drama");
            var animation = new Genre("Animation");
            var adventure = new Genre("Adventure");

            var shawshank = new MovieBuilder()
                .WithDirector("Frank Darabont")
                .WithGenres(new[] {crime, drama})
                .WithImdbId("tt0111161")
                .WithPlot("Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.")
                .WithRating(9.3m)
                .WithReleaseDate(new DateTime(1994, 10, 14))
                .WithTitle("The Shawshank Redemption")
                .Build();

            var wallE = new MovieBuilder()
                .WithDirector("Andrew Stanton")
                .WithGenres(new[] {animation, adventure, drama})
                .WithImdbId("tt0910970")
                .WithPlot("In the distant future, a small waste collecting robot inadvertently embarks on a space journey that will ultimately decide the fate of mankind.")
                .WithRating(8.5m)
                .WithReleaseDate(new DateTime(2008, 06, 27))
                .WithTitle("WALL·E")
                .Build();

            "Given a enumerable of Movies in reverse title order"
                .Given(() => movies = new[] { wallE, shawshank });

            "When mapping to a MoviesResponse"
                .When(() => response = movies.ToResponse());

            "Should map the movies as dtos ordered by title"
                .Then(() => response.Movies.ShouldAllBeEquivalentTo(new[]
                {
                    shawshank.ToDto(), 
                    wallE.ToDto()
                }));

            "Should map the movies distinct genres as dtos ordered by name"
                .Then(() => response.Genres.ShouldAllBeEquivalentTo(new[]
                {
                    adventure.ToDto(), 
                    animation.ToDto(),
                    crime.ToDto(),
                    drama.ToDto(),
                }));
        }
    }
}