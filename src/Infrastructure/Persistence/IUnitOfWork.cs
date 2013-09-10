namespace WebApiMovieRest.Infrastructure.Persistence
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}