using System.Linq;
using WebApiMovieRest.Core.Persistence;
using WebApiMovieRest.Infrastructure;

namespace WebApiMovieRest.Core.Resources.Genres.GetAll
{
    public class GetGenresRequestHandler : IRequestHandler<GetGenresRequest, GenresResponse>
    {
        private readonly IWebApiMovieRestContext _context;

        public GetGenresRequestHandler(IWebApiMovieRestContext context)
        {
            _context = context;
        }

        public GenresResponse Handle(GetGenresRequest request)
        {
            return _context.Genres.ToList().ToResponse();
        }
    }
}