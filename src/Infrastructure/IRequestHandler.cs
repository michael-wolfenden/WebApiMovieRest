namespace WebApiMovieRest.Infrastructure
{
    public interface IRequestHandler<in TRequest, out TResponse> where TRequest : class, IRequest<TResponse>
    {
        TResponse Handle(TRequest request);
    }
}