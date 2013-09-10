using System.Collections.Generic;
using System.Linq;
using ServiceStack.Common;
using WebApiMovieRest.Core.Domain;
using WebApiMovieRest.Core.Resources.Genres;

namespace WebApiMovieRest.Core.Resources.Movies
{
    public static class MoviesExtensions
    {
        public static MoviesResponse ToResponse(this IEnumerable<Movie> movies)
        {
            var movieDtos = movies.OrderBy(movie => movie.Title)
                                          .Select(movie => movie.ToDto())
                                          .ToArray();

            var genreDtos = movies.SelectMany(movie => movie.Genres)
                                          .Distinct()
                                          .OrderBy(genre => genre.Name)
                                          .Select(genre => genre.ToDto())
                                          .ToArray();
            return new MoviesResponse()
            {
                Genres = genreDtos,
                Movies = movieDtos
            };
        }

        public static MovieDto ToDto(this Movie movie)
        {
            var movieDto = movie.TranslateTo<MovieDto>();
            movieDto.Links = new MovieDto.LinkDto()
            {
                Genres = movie.Genres
                              .OrderBy(genre => genre.Name)
                              .Select(genre => genre.Id)
                              .ToArray()
            };
            return movieDto;
        }
    }
}