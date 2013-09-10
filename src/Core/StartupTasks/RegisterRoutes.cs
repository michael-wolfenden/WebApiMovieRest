using System.Web.Http;
using WebApiMovieRest.Infrastructure.IoC;

namespace WebApiMovieRest.Core.StartupTasks
{
    public class RegisterRoutes : IStartable
    {
        private readonly HttpConfiguration _httpConfiguration;

        public RegisterRoutes(HttpConfiguration httpConfiguration)
        {
            _httpConfiguration = httpConfiguration;
        }

        public void Start()
        {
            _httpConfiguration.MapHttpAttributeRoutes();
        }
    }
}