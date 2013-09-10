using WebApiMovieRest.Infrastructure.Persistence;

namespace WebApiMovieRest.Infrastructure.Handlers
{
    public class UnitOfWorkHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : class, IRequest<TResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestHandler<TRequest, TResponse> _decoratedHandler;

        public UnitOfWorkHandler(IUnitOfWork unitOfWork, IRequestHandler<TRequest, TResponse>  decoratedHandler)
        {
            _unitOfWork = unitOfWork;
            _decoratedHandler = decoratedHandler;
        }

        public TResponse Handle(TRequest request)
        {
            TResponse response = _decoratedHandler.Handle(request);
            _unitOfWork.Commit();
            return response;
        }
    }
}