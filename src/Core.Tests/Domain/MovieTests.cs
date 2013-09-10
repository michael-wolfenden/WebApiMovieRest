using System;
using FluentAssertions;
using WebApiMovieRest.Core.Domain;
using WebApiMovieRest.Core.Tests.Builders;
using WebApiMovieRest.Core.Tests.xUnitDataSources;
using Xbehave;
using Xunit;

namespace WebApiMovieRest.Core.Tests.Domain
{
    public class MovieTests
    {
        [Scenario]
        [NullEmptyAndWhitespace]
        [StringOfLength(10)]
        [StringOfLength(8)]
        public void Should_throw_when_imdb_id_is_null_or_not_exactly_9_characters(
            string imdbId, 
            Exception exception)
        {
            "When constructing a Movie with imdbId '{0}'"
                .When(() => exception = Record.Exception(() => new MovieBuilder().WithImdbId(imdbId).Build()));

            "Then an ArgumentException should be thrown"
                .Then(() => exception.Should().BeAssignableTo<ArgumentException>());

            "With a parameter name of 'imdbId'"
                .And(() => exception.As<ArgumentException>().ParamName.Should().Be("imdbId"));
        }

        [Scenario]
        [NullEmptyAndWhitespace]
        [StringOfLength(129)]
        public void Should_throw_when_title_is_null_empty_or_greater_than_128_characters(
            string title,
            Exception exception)
        {
            "When constructing a Movie with title '{0}'"
                .When(() => exception = Record.Exception(() => new MovieBuilder().WithTitle(title).Build()));

            "Then an ArgumentException should be thrown"
                .Then(() => exception.Should().BeAssignableTo<ArgumentException>());

            "With a parameter name of 'title'"
                .And(() => exception.As<ArgumentException>().ParamName.Should().Be("title"));
        }

        [Scenario]
        [NullEmptyAndWhitespace]
        [StringOfLength(65)]
        public void Should_throw_when_director_is_null_empty_or_greater_than_64_characters(
            string director,
            Exception exception)
        {
            "When constructing a Movie with director '{0}'"
                .When(() => exception = Record.Exception(() => new MovieBuilder().WithDirector(director).Build()));

            "Then an ArgumentException should be thrown"
                .Then(() => exception.Should().BeAssignableTo<ArgumentException>());

            "With a parameter name of 'director'"
                .And(() => exception.As<ArgumentException>().ParamName.Should().Be("director"));
        }

        [Scenario]
        [NullEmptyAndWhitespace]
        [StringOfLength(257)]
        public void Should_throw_when_plot_is_null_empty_or_greater_than_256_characters(
            string plot,
            Exception exception)
        {
            "When constructing a Movie with plot '{0}'"
                .When(() => exception = Record.Exception(() => new MovieBuilder().WithPlot(plot).Build()));

            "Then an ArgumentException should be thrown"
                .Then(() => exception.Should().BeAssignableTo<ArgumentException>());

            "With a parameter name of 'plot'"
                .And(() => exception.As<ArgumentException>().ParamName.Should().Be("plot"));
        }

        [Scenario]
        [Decimal("-0.1")]
        [Decimal("10.1")]
        public void Should_throw_when_rating_is_negative_or_greather_than_10(
            decimal rating,
            Exception exception)
        {
            "When constructing a Movie with rating '{0}'"
                .When(() => exception = Record.Exception(() => new MovieBuilder().WithRating(rating).Build()));

            "Then an ArgumentException should be thrown"
                .Then(() => exception.Should().BeAssignableTo<ArgumentException>());

            "With a parameter name of 'rating'"
                .And(() => exception.As<ArgumentException>().ParamName.Should().Be("rating"));
        }

        [Scenario]
        public void Should_throw_when_genres_null(
            Exception exception)
        {
            "When constructing a Movie with a null genres collection"
                .When(() => exception = Record.Exception(() => new MovieBuilder().WithGenres(null).Build()));

            "Then an ArgumentException should be thrown"
                .Then(() => exception.Should().BeAssignableTo<ArgumentException>());

            "With a parameter name of 'genres'"
                .And(() => exception.As<ArgumentException>().ParamName.Should().Be("genres"));
        }

        [Scenario]
        public void Should_throw_when_genres_empty(
            Exception exception)
        {
            "When constructing a Movie with an empty genres collection"
                .When(() => exception = Record.Exception(() => new MovieBuilder().WithGenres(new Genre[0]).Build()));

            "Then an ArgumentException should be thrown"
                .Then(() => exception.Should().BeAssignableTo<ArgumentException>());

            "With a parameter name of 'genres'"
                .And(() => exception.As<ArgumentException>().ParamName.Should().Be("genres"));
        }


        [Scenario]
        public void Should_not_throw_when_Movie_is_valid(
            Exception exception)
        {
            "When constructing a Movie with valid parameters"
                .When(() => exception = Record.Exception(() => new MovieBuilder().Build()));

            "Then no exceptions should be thrown"
                .Then(() => exception.Should().BeNull());
        }
    }
}