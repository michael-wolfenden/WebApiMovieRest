using System.Web.Http;
using WebApiMovieRest.Infrastructure.Filters;
using WebApiMovieRest.Infrastructure.IoC;

namespace WebApiMovieRest.Core.StartupTasks
{
    public class RegisterFilters : IStartable
    {
        private readonly HttpConfiguration _httpConfiguration;

        public RegisterFilters(HttpConfiguration httpConfiguration)
        {
            _httpConfiguration = httpConfiguration;
        }

        public void Start()
        {
            _httpConfiguration.Filters.Add(new ResourceNotFoundExceptionFilter());
            _httpConfiguration.Filters.Add(new ValidationExceptionFilter());
        }
    }
}