using FluentAssertions;
using WebApiMovieRest.Core.Tests.xUnitDataSources;
using WebApiMovieRest.Infrastructure.Extensions;
using Xbehave;

namespace WebApiMovieRest.Infrastructure.Tests.Extensions
{
    public class StringExtensionsTest
    {
        [Scenario]
        [NullEmptyAndWhitespace]
        public void IsNullOrWhiteSpace_should_return_true_when_string_is_null_or_empty(
            string input,
            bool isNullOrWhiteSpace)
        {
            "Given a null or empty string '{0}'"
                .Given(() => {});

            "When calling IsNullOrWhiteSpace"
                .When(() => isNullOrWhiteSpace = input.IsNullOrWhiteSpace());

            "Then should return true"
                .Then(() => isNullOrWhiteSpace.Should().BeTrue());
        }

        [Scenario]
        public void IsNullOrWhiteSpace_should_return_false_when_string_is_not_null_or_empty(
            string notNullOrEmptyString,
            bool isNullOrWhiteSpace)
        {
            "Given a not null or empty string"
                .Given(() => notNullOrEmptyString = "not null or empty string");

            "When calling IsNullOrWhiteSpace"
                .When(() => isNullOrWhiteSpace = notNullOrEmptyString.IsNullOrWhiteSpace());

            "Then should return false"
                .Then(() => isNullOrWhiteSpace.Should().BeFalse());
        }
    }
}