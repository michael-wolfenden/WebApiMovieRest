using System;
using FluentValidation;
using FluentValidation.Results;

namespace WebApiMovieRest.Infrastructure.Handlers
{
    public class ValidationHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : class, IRequest<TResponse>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IRequestHandler<TRequest, TResponse> _decoratedHandler;

        public ValidationHandler(IServiceProvider serviceProvider, IRequestHandler<TRequest, TResponse> decoratedHandler)
        {
            _serviceProvider = serviceProvider;
            _decoratedHandler = decoratedHandler;
        }
        public TResponse Handle(TRequest request)
        {
            if (request == null) throw new ValidationException(new[] { new ValidationFailure("Request", "Request cannot be null") });

            var validator = ResolveValidator(request.GetType());
            if (validator != null)
            {
                var validationResult = validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }
            }

            return _decoratedHandler.Handle(request);
        }

        private IValidator ResolveValidator(Type requestType)
        {
            var genericValidator = typeof(IValidator<>).MakeGenericType(requestType);
            var validator = _serviceProvider.GetService(genericValidator);

            if (validator == null) return null;

            return (IValidator)validator;
        }
    }
}