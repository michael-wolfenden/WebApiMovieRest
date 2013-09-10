using System.Linq;
using WebApiMovieRest.Core.Persistence;
using WebApiMovieRest.Infrastructure;
using WebApiMovieRest.Infrastructure.Exceptions;

namespace WebApiMovieRest.Core.Resources.Movies.Delete
{
    public class DeleteMovieRequestHandler : IRequestHandler<DeleteMovieRequest, VoidResponse>
    {
        private readonly IWebApiMovieRestContext _context;

        public DeleteMovieRequestHandler(IWebApiMovieRestContext context)
        {
            _context = context;
        }

        public VoidResponse Handle(DeleteMovieRequest request)
        {
            var movie = _context.Movies
                                .SingleOrDefault(m => m.Id == request.MovieId);

            if (movie == null) throw new ResourceNotFoundException("Movie[Id={0}] could not be found", request.MovieId);

            _context.Movies.Remove(movie);

            return VoidResponse.Instance;
        }
    }
}