using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using WebApiMovieRest.Infrastructure.Extensions;
using Xbehave;

namespace WebApiMovieRest.Infrastructure.Tests.Extensions
{
    public class ObjectExtensionsTests
    {
        [Scenario]
        public void Yield_should_return_an_enumerable_from_a_single_object(
            object objectToYield,
            IEnumerable<object> enumerable)
        {
            "Given an object"
                .Given(() => objectToYield = new object());

            "When yielding that object"
                .When(() => enumerable = objectToYield.Yield());

            "Then should return an enumerable containing only that object"
                .Then(() => enumerable.Should().ContainSingle(o => o == objectToYield));
        } 
    }
}