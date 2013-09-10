using System;
using System.Data.Entity;
using System.Linq;
using WebApiMovieRest.Core.Domain;
using WebApiMovieRest.Core.Persistence;

namespace WebApiMovieRest.IntegrationTests.Helpers
{
    public static class Db
    {
        public static Genre InsertGenre(Genre genre)
        {
            WithinContext(context => context.Genres.Add(genre));
            return genre;
        }

        public static Movie InsertMovie(Movie movie)
        {
            WithinContext(context => context.Movies.Add(movie));
            return movie;
        }

        public static Movie GetMovieById(Guid movieId)
        {
            Movie movie = null;
            WithinContext(context => 
                movie = context.Movies
                               .Include(x => x.Genres)
                               .SingleOrDefault(x => x.Id == movieId)
            );
            return movie;
        }

        public static Genre GetGenreByName(string genreName)
        {
            Genre genre = null;
            WithinContext(context => genre = context.Genres.SingleOrDefault(x => x.Name == genreName));
            return genre;
        }

        private static void WithinContext(Action<WebApiMovieRestContext> action)
        {
            using (var dbContext = new WebApiMovieRestContext())
            {
                action(dbContext);
                dbContext.SaveChanges();
            }
        }
    }
}