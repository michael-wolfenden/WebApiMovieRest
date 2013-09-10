using System.Collections.Generic;

namespace WebApiMovieRest.Infrastructure.Extensions
{
    public static class ObjectExtensions
    {
        public static IEnumerable<T> Yield<T>(this T singleInstance)
        {
            yield return singleInstance;
        }
    }
}