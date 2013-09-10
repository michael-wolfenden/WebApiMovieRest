using System;
using WebApiMovieRest.Infrastructure;

namespace WebApiMovieRest.Core.Resources.Movies.Delete
{
    public class DeleteMovieRequest : IRequest<VoidResponse>
    {
        public Guid MovieId { get; set; }
    }
}