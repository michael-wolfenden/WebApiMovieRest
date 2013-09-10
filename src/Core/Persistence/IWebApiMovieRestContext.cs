using System;
using System.Data.Entity;
using WebApiMovieRest.Core.Domain;
using WebApiMovieRest.Infrastructure.Persistence;

namespace WebApiMovieRest.Core.Persistence
{
    public interface IWebApiMovieRestContext : IUnitOfWork, IDisposable
    {
        IDbSet<Movie> Movies { get; }
        IDbSet<Genre> Genres { get; }
    }
}