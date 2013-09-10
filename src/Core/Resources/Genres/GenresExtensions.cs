using System.Collections.Generic;
using System.Linq;
using ServiceStack.Common;
using WebApiMovieRest.Core.Domain;

namespace WebApiMovieRest.Core.Resources.Genres
{
    public static class GenresExtensions
    {
        public static GenresResponse ToResponse(this IEnumerable<Genre> genres)
        {
            var genreDtos = genres.OrderBy(genre => genre.Name)
                                   .Select(genre => genre.ToDto())
                                   .ToArray();

            return new GenresResponse()
            {
                Genres = genreDtos
            };
        }

        public static GenreDto ToDto(this Genre genre)
        {
            return genre.TranslateTo<GenreDto>();
        }
    }
}