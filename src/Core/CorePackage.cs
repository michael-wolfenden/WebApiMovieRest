using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using FluentValidation;
using SimpleInjector;
using SimpleInjector.Extensions;
using SimpleInjector.Packaging;
using WebApiMovieRest.Core.Persistence;
using WebApiMovieRest.Infrastructure;
using WebApiMovieRest.Infrastructure.Extensions;
using WebApiMovieRest.Infrastructure.Handlers;
using WebApiMovieRest.Infrastructure.IoC;
using WebApiMovieRest.Infrastructure.Persistence;

namespace WebApiMovieRest.Core
{
    public class CorePackage : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.EnableLifetimeScoping();
            
            var thisAssembly = typeof (CorePackage).Assembly;

            RegisterServiceProvider(container);
            RegisterStartables(container, thisAssembly);
            RegisterControllers(container, thisAssembly);
            RegisterValidators(container, thisAssembly);
            RegisterContextAndUnitOfWork(container);
            RegisterHandlersAndDecorators(container, thisAssembly);
        }

        private void RegisterServiceProvider(Container container)
        {
            container.RegisterSingle<IServiceProvider>(() => container);
        }

        private void RegisterStartables(Container container, Assembly thisAssembly)
        {
            var startables = thisAssembly.TypesOf<IStartable>();
            container.RegisterAll<IStartable>(startables);
        }

        private void RegisterControllers(Container container, Assembly thisAssembly)
        {
            var controllers = thisAssembly.TypesOf<ApiController>().ToList();
            controllers.ForEach(controller => container.Register(controller));
        }

        private void RegisterValidators(Container container, Assembly thisAssembly)
        {
            container.RegisterManyForOpenGeneric(typeof(IValidator<>), thisAssembly);
        }

        private void RegisterContextAndUnitOfWork(Container container)
        {
            container.RegisterLifetimeScope<IWebApiMovieRestContext, WebApiMovieRestContext>();
            container.RegisterLifetimeScope<IUnitOfWork>(() => (IUnitOfWork)container.GetInstance<IWebApiMovieRestContext>());
        }

        private void RegisterHandlersAndDecorators(Container container, Assembly thisAssembly)
        {
            container.RegisterManyForOpenGeneric(typeof(IRequestHandler<,>), thisAssembly);

            container.RegisterDecorator(
                typeof(IRequestHandler<,>),
                typeof(ValidationHandler<,>));

            container.RegisterDecorator(
                typeof(IRequestHandler<,>),
                typeof(UnitOfWorkHandler<,>));

            container.RegisterDecorator(
                typeof(IRequestHandler<,>),
                typeof(LifetimeScopeHandler<,>));
        }
    }
}
