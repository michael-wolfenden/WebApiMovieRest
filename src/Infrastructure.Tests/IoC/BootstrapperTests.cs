using System.Linq;
using FluentAssertions;
using NSubstitute;
using SimpleInjector;
using SimpleInjector.Packaging;
using WebApiMovieRest.Infrastructure.IoC;
using Xbehave;

namespace WebApiMovieRest.Infrastructure.Tests.IoC
{
    public class BootstrapperTests
    {
        [Scenario]
        public void Bootstrapper_should_register_all_packages_from_the_provided_assemblies(Container container)
        {
            "Given a a package in this assembly that registers an object and a startable task"
                .Given(() => { });

            "When Bootstrapper.Bootstrap is called passing in this assembly"
                .When(() => container = Bootstrapper.Bootstrap(typeof(BootstrapperTests).Assembly));

            "Then all the dependencies in the package should be registered"
                .Then(() => container.GetInstance(typeof(object)).Should().NotBeNull());

            "And all the startable tasks in the package should have been started"
                .Then(() => container.GetAllInstances<IStartable>().Single().Received().Start());
        }

        public class PackgeThatRegistersAnObjectAndAStartupTask : IPackage
        {
            public void RegisterServices(Container container)
            {
                container.Register(() => new object());
                container.RegisterAll(Substitute.For<IStartable>());
            }
        }
    }
}