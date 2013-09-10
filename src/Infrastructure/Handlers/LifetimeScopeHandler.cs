using System;
using SimpleInjector;

namespace WebApiMovieRest.Infrastructure.Handlers
{
    public class LifetimeScopeHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : class, IRequest<TResponse>
    {
        private readonly Container _container;
        private readonly Func<IRequestHandler<TRequest, TResponse>> _decoratedHandlerFactory;

        public LifetimeScopeHandler(Container container, Func<IRequestHandler<TRequest, TResponse>> decoratedHandlerFactory)
        {
            _container = container;
            _decoratedHandlerFactory = decoratedHandlerFactory;
        }

        public TResponse Handle(TRequest request)
        {
            TResponse response;

            using (_container.BeginLifetimeScope())
            {
                var decoratedHandler = _decoratedHandlerFactory();
                response = decoratedHandler.Handle(request);
            }

            return response;
        }
    }
}