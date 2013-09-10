using System.Collections.Generic;
using FluentAssertions;
using WebApiMovieRest.Core.Domain;
using WebApiMovieRest.Core.Resources.Genres;
using Xbehave;

namespace WebApiMovieRest.Core.Tests.Resources.Genres
{
    public class GenresExtensionsTests
    {
        [Scenario]
        public void Should_be_able_to_map_from_Genre_to_GenreDto(
            Genre genre,
            GenreDto genreDto
            )
        {
            "Given a Genre"
               .Given(() => genre = new Genre("Crime"));

            "When mapping to a GenreDto"
                .When(() => genreDto = genre.ToDto());

            "Should map id"
                .Then(() => genreDto.Id.Should().Be(genre.Id));

            "Should map name"
                .Then(() => genreDto.Name.Should().Be(genre.Name));
        }

        [Scenario]
        public void Should_be_able_to_map_from_an_enumerable_of_Genres_to_GenresResponse(
            IEnumerable<Genre> genres,
            GenresResponse response
            )
        {
            var drama = new Genre("Drama");
            var crime = new Genre("Crime");

            "Given a enumerable of Genres in reverse name order"
               .Given(() => genres = new[] { drama, crime });

            "When mapping to a GenresResponse"
                .When(() => response = genres.ToResponse());

            "Should map the genres as dtos ordered by name"
                .Then(() => response.Genres.ShouldAllBeEquivalentTo(new[]
                {
                    crime.ToDto(), 
                    drama.ToDto()
                }));
        }
    }
}