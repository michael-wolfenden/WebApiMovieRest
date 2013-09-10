using System.Data.Entity.ModelConfiguration;

namespace WebApiMovieRest.Core.Domain.Mapping
{
    public class MovieMap : EntityTypeConfiguration<Movie>
    {
        public MovieMap()
        {
            Property(movie => movie.ImdbId)
                .HasMaxLength(9);

            Property(movie => movie.Title)
                .HasMaxLength(128);

            Property(movie => movie.Plot)
                .HasMaxLength(256);

            HasMany(movie => movie.Genres)
                .WithMany()
                .Map(m =>
                    {
                        m.MapLeftKey("MovieId");
                        m.MapRightKey("GenreId");
                    });
        }
    }
}