using System;
using System.Web.Http.Routing;

namespace WebApiMovieRest.Core.Extensions
{
    public static class UrlHelperExtensions
    {
        public static Uri GetMovieById(this UrlHelper url, Guid movieId)
        {
            return new Uri(url.Link(Constants.Routes.GetMovieById, new { movieId }));
        }
    }
}