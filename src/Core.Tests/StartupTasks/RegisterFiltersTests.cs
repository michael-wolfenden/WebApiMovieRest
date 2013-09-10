using System;
using System.Web.Http;
using FluentAssertions;
using WebApiMovieRest.Core.StartupTasks;
using WebApiMovieRest.Infrastructure.Filters;
using Xbehave;
using Xunit.Extensions;

namespace WebApiMovieRest.Core.Tests.StartupTasks
{
    public class RegisterFilterTests
    {
        [Scenario]
        [InlineData(typeof (ResourceNotFoundExceptionFilter))]
        [InlineData(typeof (ValidationExceptionFilter))]
        public void When_starting_the_register_filters_task(Type filterType)
        {
            var httpConfiguration = default(HttpConfiguration);
            var registerFilters = default(RegisterFilters);

            "Given a HttpConfiguration"
                .Given(() => httpConfiguration = new HttpConfiguration());

            "And a RegisterFilters task"
                .And(() => registerFilters = new RegisterFilters(httpConfiguration));

            "After starting the task"
                .When(() => registerFilters.Start());

            ("Then should register " + filterType.Name)
                .Then(() => httpConfiguration.Filters.Should().Contain(x => x.Instance.GetType() == filterType));
        }
    }
}