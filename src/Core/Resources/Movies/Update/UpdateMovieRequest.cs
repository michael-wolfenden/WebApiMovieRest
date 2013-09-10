using System;
using WebApiMovieRest.Infrastructure;

namespace WebApiMovieRest.Core.Resources.Movies.Update
{
    public class UpdateMovieRequest : IRequest<MoviesResponse>, ICreateOrUpdateMovieRequest
    {
        public Guid MovieId { get; set; }
        public string ImdbId { get; set; }
        public string Title { get; set; }
        public decimal Rating { get; set; }
        public string Director { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Plot { get; set; }
        public string[] Genres { get; set; }
    }
}