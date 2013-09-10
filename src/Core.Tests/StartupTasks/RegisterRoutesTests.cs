using System;
using System.Web.Http;
using WebApiMovieRest.Core.Resources.Genres.Get;
using WebApiMovieRest.Core.Resources.Genres.GetAll;
using WebApiMovieRest.Core.Resources.Movies.Create;
using WebApiMovieRest.Core.Resources.Movies.Delete;
using WebApiMovieRest.Core.Resources.Movies.Get;
using WebApiMovieRest.Core.Resources.Movies.GetAll;
using WebApiMovieRest.Core.Resources.Movies.Update;
using WebApiMovieRest.Core.StartupTasks;
using WebApiMovieRest.Core.Tests.Helpers;
using Xbehave;

namespace WebApiMovieRest.Core.Tests.StartupTasks
{
    public class RegisterRoutesTests
    {
        [Scenario]
        public void When_starting_the_register_routes_task(
            RegisterRoutes registerRoutes,
            HttpConfiguration httpConfiguration
            )
        {
            const string BASE_URL = "http://localhost/";

            var aMovieId = Guid.NewGuid();
            var aGenreId = Guid.NewGuid();

            "Given a HttpConfiguration"
                .Given(() => httpConfiguration = new HttpConfiguration());

            "And a RegisterDependencyResolver task"
                .And(() => registerRoutes = new RegisterRoutes(httpConfiguration));

            "After starting the task"
                .When(() => registerRoutes.Start());

            #region Movie Routes

            "Should register a route to get a movie by id".Then(() =>
                new RouteTester<GetMovieController>(BASE_URL, httpConfiguration)
                    .GET("/api/v1/movies/{0}", aMovieId)
                    .ShouldRouteToAction(x => x.Process(new GetMovieRequest()))
                    .WithRouteValue("movieId", aMovieId.ToString())
                    .WithRouteName(Constants.Routes.GetMovieById)
                    .Verify());

            "Should not route to get a movie if movie id is not a guid".Then(() =>
                new RouteTester<GetMovieController>(BASE_URL, httpConfiguration)
                    .GET("/api/v1/movies/{0}", 1)
                    .ShouldNotFindRoute()
                    .Verify());

            "Should register a route to get a movie with extension".Then(() =>
                new RouteTester<GetMovieController>(BASE_URL, httpConfiguration)
                    .GET("/api/v1/movies/{0}.json", aMovieId)
                    .ShouldRouteToAction(x => x.Process(new GetMovieRequest()))
                    .WithRouteValue("movieId", aMovieId.ToString())
                    .WithRouteValue("ext", "json")
                    .WithRouteName(Constants.Routes.GetMovieById)
                    .Verify());

            "Should register route to get all movies".Then(() =>
                new RouteTester<GetMoviesController>(BASE_URL, httpConfiguration)
                    .GET("/api/v1/movies")
                    .ShouldRouteToAction(x => x.Process(new GetMoviesRequest()))
                    .Verify());

            "Should register route to get all movies with extension".Then(() =>
                new RouteTester<GetMoviesController>(BASE_URL, httpConfiguration)
                    .GET("/api/v1/movies.json")
                    .ShouldRouteToAction(x => x.Process(new GetMoviesRequest()))
                    .WithRouteValue("ext", "json")
                    .Verify());

            "Should register route to delete a movie".Then(() =>
                new RouteTester<DeleteMovieController>(BASE_URL, httpConfiguration)
                    .DELETE("/api/v1/movies/{0}", aMovieId)
                    .ShouldRouteToAction(x => x.Process(new DeleteMovieRequest()))
                    .WithRouteValue("movieId", aMovieId.ToString())
                    .Verify());

            "Should not route to delete a movie if movie id is not a guid".Then(() =>
                new RouteTester<DeleteMovieController>(BASE_URL, httpConfiguration)
                    .DELETE("/api/v1/movies/{0}", 1)
                    .ShouldNotFindRoute()
                    .Verify());

            "Should register route to update a movie".Then(() =>
                new RouteTester<UpdateMovieController>(BASE_URL, httpConfiguration)
                    .PUT("/api/v1/movies/{0}", aMovieId)
                    .ShouldRouteToAction(x => x.Process(Guid.Empty, null))
                    .WithRouteValue("movieId", aMovieId.ToString())
                    .Verify());

            "Should register route to create a movie".Then(() =>
                new RouteTester<CreateMovieController>(BASE_URL, httpConfiguration)
                    .POST("/api/v1/movies", aMovieId)
                    .ShouldRouteToAction(x => x.Process(null))
                    .Verify());

            #endregion

            #region Genre Routes

            "Should register route to get all genres".Then(() =>
                new RouteTester<GetGenresController>(BASE_URL, httpConfiguration)
                    .GET("/api/v1/genres")
                    .ShouldRouteToAction(x => x.Process(new GetGenresRequest()))
                    .Verify());

            "Should register route to get all genres with extension".Then(() =>
                new RouteTester<GetGenresController>(BASE_URL, httpConfiguration)
                    .GET("/api/v1/genres.json")
                    .ShouldRouteToAction(x => x.Process(new GetGenresRequest()))
                    .WithRouteValue("ext", "json")
                    .Verify());

            "Should register a route to get a genre by id".Then(() =>
                new RouteTester<GetGenreController>(BASE_URL, httpConfiguration)
                    .GET("/api/v1/genres/{0}", aGenreId)
                    .ShouldRouteToAction(x => x.Process(new GetGenreRequest()))
                    .WithRouteValue("genreId", aGenreId.ToString())
                    .Verify());

            "Should not route to get a genre if genre id is not a guid".Then(() =>
                new RouteTester<GetGenreController>(BASE_URL, httpConfiguration)
                    .GET("/api/v1/genres/{0}", 1)
                    .ShouldNotFindRoute()
                    .Verify());

            "Should register a route to get a genre with extension".Then(() =>
                new RouteTester<GetGenreController>(BASE_URL, httpConfiguration)
                    .GET("/api/v1/genres/{0}.json", aGenreId)
                    .ShouldRouteToAction(x => x.Process(new GetGenreRequest()))
                    .WithRouteValue("genreId", aGenreId.ToString())
                    .WithRouteValue("ext", "json")
                    .Verify());

            #endregion

        }
    }
}
