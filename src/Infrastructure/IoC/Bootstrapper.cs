using System.Linq;
using System.Reflection;
using SimpleInjector;

namespace WebApiMovieRest.Infrastructure.IoC
{
    public static class Bootstrapper
    {
        public static Container Bootstrap(params Assembly[] assembliesToScan)
        {
            var container = new Container();
            container.RegisterPackages(assembliesToScan);
            container.Verify();

            StartEveryStartupTask(container);

            return container;
        }

        private static void StartEveryStartupTask(Container container)
        {
            container.GetAllInstances<IStartable>()
                .ToList()
                .ForEach(startable => startable.Start());
        }
    }
}