using System;
using WebApiMovieRest.Infrastructure;

namespace WebApiMovieRest.Core.Resources.Genres.Get
{
    public class GetGenreRequest : IRequest<GenresResponse>
    {
        public Guid GenreId { get; set; }
    }
}