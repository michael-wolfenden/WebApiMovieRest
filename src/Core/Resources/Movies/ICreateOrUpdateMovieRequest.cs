using System;

namespace WebApiMovieRest.Core.Resources.Movies
{
    public interface ICreateOrUpdateMovieRequest
    {
        string ImdbId { get; set; }
        string Title { get; set; }
        decimal Rating { get; set; }
        string Director { get; set; }
        DateTime? ReleaseDate { get; set; }
        string Plot { get; set; }
        string[] Genres { get; set; } 
    }
}