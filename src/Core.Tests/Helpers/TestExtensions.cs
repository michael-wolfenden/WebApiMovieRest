using System.Net.Http;

namespace WebApiMovieRest.Core.Tests.Helpers
{
    public static class Extensions
    {
        public static T ContentAs<T>(this HttpResponseMessage self)
        {
            T content;
            self.TryGetContentValue(out content);

            return content;
        }
    }
}