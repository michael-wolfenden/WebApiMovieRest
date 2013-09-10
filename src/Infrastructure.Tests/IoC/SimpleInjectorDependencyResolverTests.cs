using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using FluentAssertions;
using SimpleInjector;
using WebApiMovieRest.Infrastructure.IoC;
using Xbehave;
using Xunit;

namespace WebApiMovieRest.Infrastructure.Tests.IoC
{
    public class SimpleInjectorDependencyResolverTests
    {
        [Scenario]
        public void Should_throw_when_Container_is_null(
            Exception exception)
        {
            "When constructing a SimpleInjectorDependencyResolver with a null Container"
                .When(() => exception = Record.Exception(() => new SimpleInjectorDependencyResolver(null)));

            "Then an ArgumentNullException should be thrown"
                .Then(() => exception.Should().BeOfType<ArgumentNullException>());

            "An the parameter name should be 'container'"
                .Then(() => exception.As<ArgumentNullException>().ParamName.Should().Be("container"));
        }

        [Scenario]
        public void GetService_should_return_null_when_service_is_not_registered(
            Container Container,
            SimpleInjectorDependencyResolver dependencyResolver,
            object retrievedService)
        {
            "Given a Container with no services registered"
                .Given(() => Container = new Container());

            "And a SimpleInjectorDependencyResolver constructed with the Container"
                .And(() => dependencyResolver = new SimpleInjectorDependencyResolver(Container));

            "When GetService is called"
                .When(() => retrievedService = dependencyResolver.GetService(typeof(object)));

            "Then retrieved service should be null"
                .Then(() => retrievedService.Should().BeNull());
        }

        [Scenario]
        public void GetService_should_return_registered_service(
            Container Container,
            SimpleInjectorDependencyResolver dependencyResolver,
            object registeredService,
            object retrievedService)
        {
            "Given a service of a particular type"
                .Given(() => registeredService = new object());

            "And a Container"
                .And(() => Container = new Container());
            
            "With the service registered"
                .And(() => Container.RegisterSingle(() => registeredService));

            "And a SimpleInjectorDependencyResolver constructed with the Container"
                .And(() => dependencyResolver = new SimpleInjectorDependencyResolver(Container));

            "When GetService is called for the particular service type"
                .When(() => retrievedService = dependencyResolver.GetService(typeof(object)));

            "Then the registered service should be returned"
                .Then(() => retrievedService.Should().BeSameAs(registeredService));
        }

        [Scenario]
        public void GetServices_should_return_empty_enumerable_when_service_is_not_registered(
            Container Container,
            SimpleInjectorDependencyResolver dependencyResolver,
            IEnumerable<object> retrievedServices)
        {
            "Given a Container with no services registered"
                .Given(() => Container = new Container());

            "And a SimpleInjectorDependencyResolver constructed with the Container"
                .And(() => dependencyResolver = new SimpleInjectorDependencyResolver(Container));

            "When GetServices is called"
                .When(() => retrievedServices = dependencyResolver.GetServices(typeof(object)));

            "Then retrieved services should be an empty enumerable"
                .Then(() => retrievedServices.Should().BeEmpty());
        }

        [Scenario]
        public void GetServices_should_return_registered_services(
            Container Container,
            SimpleInjectorDependencyResolver dependencyResolver,
            object firstRegisteredService,
            object secondRegisteredService,
            IEnumerable<object> retrievedServices)
        {
            "Given a service of a particular type"
                .Given(() => firstRegisteredService = new object());

            "And another service of the same type"
                .And(() => secondRegisteredService = new object());

            "And a Container"
                .And(() => Container = new Container());

            "With both services registered"
                .And(() => Container.RegisterAll(firstRegisteredService, secondRegisteredService));

            "And a SimpleInjectorDependencyResolver constructed with the Container"
                .And(() => dependencyResolver = new SimpleInjectorDependencyResolver(Container));

            "When GetServices is called for the particular service type"
                .When(() => retrievedServices = dependencyResolver.GetServices(typeof(object)));

            "Then the registered services should be returned"
                .Then(() => retrievedServices.Should().BeEquivalentTo(firstRegisteredService, secondRegisteredService));
        }

        [Scenario]
        public void BeginScope_should_return_the_dependency_resolver_itself(
            SimpleInjectorDependencyResolver dependencyResolver,
            IDependencyScope dependencyScope)
        {
            "Given a SimpleInjectorDependencyResolver"
                .Given(() => dependencyResolver = new SimpleInjectorDependencyResolver(new Container()));

            "When BeginScope is called"
                .When(() => dependencyScope = dependencyResolver.BeginScope());

            "Then the dependency scope returned should be the dependency resolver itself"
                .Then(() => dependencyScope.Should().BeSameAs(dependencyResolver));
        }
    }
}