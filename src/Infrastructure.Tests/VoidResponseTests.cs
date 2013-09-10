using FluentAssertions;
using Xbehave;

namespace WebApiMovieRest.Infrastructure.Tests
{
    public class VoidResponseTests
    {
        [Scenario]
        public void VoidResponse_should_be_a_singleton(
            VoidResponse aVoidResponse, 
            VoidResponse anotherVoidResponse)
        {
            "Given a VoidResponse instance"
                .Given(() => aVoidResponse = VoidResponse.Instance);

            "And another VoidResponse instance"
                .And(() => anotherVoidResponse = VoidResponse.Instance);

            "Then they should be referentially equal to each other"
                .Then(() => aVoidResponse.Should().BeSameAs(anotherVoidResponse));
        }
    }
}