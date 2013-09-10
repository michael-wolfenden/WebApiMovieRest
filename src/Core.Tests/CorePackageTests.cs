using System;
using System.Web.Http;
using FluentAssertions;
using FluentValidation;
using SimpleInjector;
using WebApiMovieRest.Core.Persistence;
using WebApiMovieRest.Core.Resources.Movies;
using WebApiMovieRest.Core.Resources.Movies.Create;
using WebApiMovieRest.Core.Resources.Movies.Get;
using WebApiMovieRest.Infrastructure;
using WebApiMovieRest.Infrastructure.IoC;
using WebApiMovieRest.Infrastructure.Persistence;
using Xbehave;

namespace WebApiMovieRest.Core.Tests
{
    public class CorePackageTests
    {
        [Scenario]
        public void Should_register_all_core_dependencies(
            Container container)
        {
            "Given a Container"
                .Given(() => container = new Container());

            "With a HttpConfiguration registered"
                .And(() => container.Register(() => new HttpConfiguration()));

            "When registering the CorePackage with the Container"
                .When(() => container.RegisterPackages(new [] { typeof(CorePackage).Assembly}));

            "Then it should have registered an IServiceProvider"
                .Then(() => container.ShouldHaveRegistrationFor<IServiceProvider>());

            "And it should have registered the IStartable tasks"
                .Then(() => container.ShouldHaveRegistrationFors<IStartable>());

            "And it should have registered the ApiControllers"
                .Then(() => container.ShouldHaveRegistrationFor<GetMovieController>());

            "And it should have registered the IValidators"
                .Then(() => container.ShouldHaveRegistrationFor<IValidator<CreateMovieRequest>>());

            "And it should have registered the IWebApiMovieRestContext"
                .Then(() => container.ShouldHaveRegistrationFor<IWebApiMovieRestContext>());

            "And it should have registered the IUnitOfWork as the same instance as IWebApiMovieRestContext"
                .Then(() =>
                {
                    using (container.BeginLifetimeScope())
                    {
                        var context = container.GetInstance<IWebApiMovieRestContext>();
                        var unitOfWork = container.GetInstance<IUnitOfWork>();

                        context.Should().BeSameAs(unitOfWork);
                    }
                });

            "And it should register the IRequestHandlers"
                .Then(() => container.ShouldHaveRegistrationFor<IRequestHandler<GetMovieRequest, MoviesResponse>>());
        }
    }

    internal static class ContainerExtensions
    {
        public static void ShouldHaveRegistrationFor<T>(this Container container)
        {
            using (container.BeginLifetimeScope())
            {
                container.GetInstance(typeof(T))
                        .Should()
                        .NotBeNull();
            }
        }

        public static void ShouldHaveRegistrationFors<T>(this Container container)
        {
            using (container.BeginLifetimeScope())
            {
                container.GetAllInstances(typeof(T))
                        .Should()
                        .NotBeEmpty();
            }
        }
    }
}