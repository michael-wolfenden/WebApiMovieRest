using System.Web.Http;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace WebApiMovieRest.Web
{
    public class WebPackage : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.Register(() => GlobalConfiguration.Configuration);
        }
    }
}