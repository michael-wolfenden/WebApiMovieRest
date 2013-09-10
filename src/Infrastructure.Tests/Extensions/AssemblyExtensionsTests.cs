using System;
using System.Collections.Generic;
using FluentAssertions;
using WebApiMovieRest.Infrastructure.Extensions;
using Xbehave;

namespace WebApiMovieRest.Infrastructure.Tests.Extensions
{
    public class AssemblyExtensionsTests
    {
        [Scenario]
        public void TypesOf_should_return_all_public_non_abstract_types_assignable_to_the_specified_type(
            IEnumerable<Type> actualTypes)
        {
            "Given a assembly with a number of classes or different visibilites inherting from the same abstract class"
                .Given(() => { });

            "When calling TypesOf on that assembly specifying the abstract class"
                .When(() => actualTypes = typeof(AssemblyExtensionsTests).Assembly.TypesOf<AnAbstractClass>());

            "Then all the public, non abstract types assignable to that abstract class should be returned"
                .Then(() => actualTypes.Should().BeEquivalentTo(typeof(APublicConcreteClass), typeof(AnotherPublicConcreteClass)));
        }


        public abstract class AnAbstractClass { }
        public abstract class AnotherAbstractClass : AnAbstractClass { }
        public class APublicConcreteClass : AnAbstractClass { }
        public class AnotherPublicConcreteClass : AnAbstractClass { }
        private class APrivateConcreteClass : AnAbstractClass { }
        internal class AnIternalConcreteClass : AnAbstractClass { }
    }
}