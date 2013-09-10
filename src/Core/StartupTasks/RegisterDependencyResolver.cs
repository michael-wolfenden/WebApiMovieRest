using System.Web.Http;
using SimpleInjector;
using WebApiMovieRest.Infrastructure.IoC;

namespace WebApiMovieRest.Core.StartupTasks
{
    public class RegisterDependencyResolver : IStartable
    {
        private readonly HttpConfiguration _httpConfiguration;
        private readonly Container _container;

        public RegisterDependencyResolver(HttpConfiguration httpConfiguration, Container container)
        {
            _httpConfiguration = httpConfiguration;
            _container = container;
        }

        public void Start()
        {
            _httpConfiguration.DependencyResolver = new SimpleInjectorDependencyResolver(_container);
        }
    }
}