using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using WebApiMovieRest.Core.Domain;

namespace WebApiMovieRest.Core.Persistence
{
    public class WebApiMovieRestContext : DbContext, IWebApiMovieRestContext
    {
        public IDbSet<Movie> Movies { get { return Set<Movie>(); } }
        public IDbSet<Genre> Genres { get { return Set<Genre>(); } }

        static WebApiMovieRestContext()
        {
            Database.SetInitializer(new WebApiMovieRestInitializer());
        }

        public WebApiMovieRestContext() : base("Name=WebApiMovieRestContext")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ValidateOnSaveEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            ConfigureIdConventions(modelBuilder);
            ConfigureStringConventions(modelBuilder);

            modelBuilder.Configurations.AddFromAssembly(GetType().Assembly);
        }

        private void ConfigureIdConventions(DbModelBuilder modelBuilder)
        {
            // Rename Id => TableNameId
            modelBuilder.Properties()
                        .Where(property => property.Name == "Id")
                        .Configure(property =>
                        {
                            property.IsKey();
                            property.HasColumnName(property.ClrPropertyInfo.ReflectedType.Name + "Id");
                            property.HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
                        });
        }

        private void ConfigureStringConventions(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<string>()
                        .Configure(property =>
                            {
                                property.HasMaxLength(64);
                                property.IsRequired();
                            });
        }

        public void Commit()
        {
            SaveChanges();
        }
    }
}