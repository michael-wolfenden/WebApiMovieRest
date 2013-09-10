using System;
using WebApiMovieRest.Core.Resources.Movies.Create;

namespace WebApiMovieRest.Core.Tests.Builders
{
    public class CreateMovieRequestBuilder
    {
        private string _imdbid = "tt0111161";
        private string _title = "The Shawshank Redemption";
        private decimal _rating = 9.3m;
        private string _director = "Frank Darabont";
        private DateTime? _releasedate = new DateTime(1994, 10, 14);
        private string _plot = "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.";
        private string[] _genreNames = { "Crime", "Drama" };

        public CreateMovieRequestBuilder WithImdbId(string imdbid)
        {
            _imdbid = imdbid;
            return this;
        }

        public CreateMovieRequestBuilder WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public CreateMovieRequestBuilder WithRating(decimal rating)
        {
            _rating = rating;
            return this;
        }

        public CreateMovieRequestBuilder WithDirector(string director)
        {
            _director = director;
            return this;
        }

        public CreateMovieRequestBuilder WithReleaseDate(DateTime? releasedate)
        {
            _releasedate = releasedate;
            return this;
        }

        public CreateMovieRequestBuilder WithPlot(string plot)
        {
            _plot = plot;
            return this;
        }

        public CreateMovieRequestBuilder WithGenres(string[] genreNames)
        {
            _genreNames = genreNames;
            return this;
        }

        public static implicit operator CreateMovieRequest(CreateMovieRequestBuilder builder)
        {
            return builder.Build();
        }

        public CreateMovieRequest Build()
        {
            return new CreateMovieRequest
            {
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