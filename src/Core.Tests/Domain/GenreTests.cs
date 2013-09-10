using System;
using FluentAssertions;
using WebApiMovieRest.Core.Domain;
using WebApiMovieRest.Core.Tests.xUnitDataSources;
using Xbehave;
using Xunit;

namespace WebApiMovieRest.Core.Tests.Domain
{
    public class GenreTests
    {
        [Scenario]
        [NullEmptyAndWhitespace]
        [StringOfLength(65)]
        public void Should_throw_when_name_is_null_empty_or_greater_than_64_characters(
            string name,
            Exception exception)
        {
            "When constructing a Movie with name '{0}'"
                .When(() => exception = Record.Exception(() => new Genre(name)));

            "Then an ArgumentException should be thrown"
                .Then(() => exception.Should().BeAssignableTo<ArgumentException>());

            "With a parameter name of 'name'"
                .And(() => exception.As<ArgumentException>().ParamName.Should().Be("name"));
        }
    }
}