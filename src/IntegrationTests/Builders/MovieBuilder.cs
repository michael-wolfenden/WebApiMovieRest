using System;
using WebApiMovieRest.Core.Domain;

namespace WebApiMovieRest.IntegrationTests.Builders
{
    public class MovieBuilder
    {
        private string _imdbid = "tt0111161";
        private string _title = "The Shawshank Redemption";
        private decimal _rating = 9.3m;
        private string _director = "Frank Darabont";
        private DateTime _releasedate = new DateTime(1994,10,14);
        private string _plot = "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.";
        private Genre[] _genres = { new Genre("Crime"), new Genre( "Drama") };

        public MovieBuilder WithImdbId(string imdbid)
        {
            _imdbid = imdbid;
            return this;
        }

        public MovieBuilder WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public MovieBuilder WithRating(decimal rating)
        {
            _rating = rating;
            return this;
        }

        public MovieBuilder WithDirector(string director)
        {
            _director = director;
            return this;
        }

        public MovieBuilder WithReleaseDate(DateTime releasedate)
        {
            _releasedate = releasedate;
            return this;
        }

        public MovieBuilder WithPlot(string plot)
        {
            _plot = plot;
            return this;
        }

        public MovieBuilder WithGenres(Genre[] genres)
        {
            _genres = genres;
            return this;
        }

        public static implicit operator Movie(MovieBuilder builder)
        {
            return builder.Build();
        }

        public Movie Build()
        {
            return new Movie(_imdbid, _title, _rating, _director, _releasedate, _plot, _genres);
        }
    }
}