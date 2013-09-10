using System;
using WebApiMovieRest.Infrastructure;

namespace WebApiMovieRest.Core.Resources.Movies.Create
{
    public class CreateMovieRequest : IRequest<MoviesResponse>, ICreateOrUpdateMovieRequest
    {
        public string ImdbId { get; set; }
        public string Title { get; set; }
        public decimal Rating { get; set; }
        public string Director { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Plot { get; set; }
        public string[] Genres { get; set; }
    }
}