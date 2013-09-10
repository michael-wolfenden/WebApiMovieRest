using System.Web;
using WebApiMovieRest.Core;
using WebApiMovieRest.Infrastructure.IoC;

namespace WebApiMovieRest.Web
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            Bootstrapper.Bootstrap(
                typeof(WebPackage).Assembly,
                typeof(CorePackage).Assembly
            );
        }
    }
}