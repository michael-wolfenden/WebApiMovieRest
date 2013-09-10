using System;
using WebApiMovieRest.Core.Resources.Movies.Update;

namespace WebApiMovieRest.IntegrationTests.Builders
{
    public class UpdateMovieRequestBuilder
    {
        private Guid _movieId = Guid.NewGuid();
        private string _imdbid = "tt0111161";
        private string _title = "The Shawshank Redemption";
        private decimal _rating = 9.3m;
        private string _director = "Frank Darabont";
        private DateTime? _releasedate = new DateTime(1994, 10, 14);
        private string _plot = "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.";
        private string[] _genreNames = { "Crime", "Drama" };

        public UpdateMovieRequestBuilder WithMovieId(Guid movieId)
        {
            _movieId = movieId;
            return this;
        }

        public UpdateMovieRequestBuilder WithImdbId(string imdbid)
        {
            _imdbid = imdbid;
            return this;
        }

        public UpdateMovieRequestBuilder WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public UpdateMovieRequestBuilder WithRating(decimal rating)
        {
            _rating = rating;
            return this;
        }

        public UpdateMovieRequestBuilder WithDirector(string director)
        {
            _director = director;
            return this;
        }

        public UpdateMovieRequestBuilder WithReleaseDate(DateTime? releasedate)
        {
            _releasedate = releasedate;
            return this;
        }

        public UpdateMovieRequestBuilder WithPlot(string plot)
        {
            _plot = plot;
            return this;
        }

        public UpdateMovieRequestBuilder WithGenres(string[] genreNames)
        {
            _genreNames = genreNames;
            return this;
        }

        public static implicit operator UpdateMovieRequest(UpdateMovieRequestBuilder builder)
        {
            return builder.Build();
        }

        public UpdateMovieRequest Build()
        {
            return new UpdateMovieRequest
            {
                MovieId = _movieId,
                Director = _director,
                ImdbId = _imdbid,
                Rating = _rating,
                ReleaseDate = _releasedate,
                Plot = _plot,
                Title = _title,
                Genres = _genreNames
            };
        }
    }
}