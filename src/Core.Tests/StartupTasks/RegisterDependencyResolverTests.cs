using System.Web.Http;
using FluentAssertions;
using SimpleInjector;
using WebApiMovieRest.Core.StartupTasks;
using WebApiMovieRest.Infrastructure.IoC;
using Xbehave;

namespace WebApiMovieRest.Core.Tests.StartupTasks
{
    public class RegisterDependencyResolverTests
    {
        [Scenario]
        public void When_starting_the_register_dependency_resolver_task(
            RegisterDependencyResolver registerDependencyResolver,
            HttpConfiguration httpConfiguration
            )
        {
            "Given a HttpConfiguration"
                .Given(() => httpConfiguration = new HttpConfiguration());

            "And a RegisterDependencyResolver task"
                .And(() => registerDependencyResolver = new RegisterDependencyResolver(httpConfiguration, new Container()));

            "After starting the task"
                .When(() => registerDependencyResolver.Start());

            "The controller suffix not required ttpControllerTypeResolver should be registered with the HttpConfiguration"
                .Then(() => httpConfiguration.DependencyResolver.Should().BeOfType<SimpleInjectorDependencyResolver>());
        } 
    }
}