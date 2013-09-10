using System;
using System.Collections.Generic;
using System.Diagnostics;
using CuttingEdge.Conditions;

namespace WebApiMovieRest.Core.Domain
{
    [DebuggerDisplay("{Title}")]
    public class Movie : Entity
    {
        private string _imdbId;
        private string _title;
        private decimal _rating;
        private string _director;
        private string _plot;

        public DateTime ReleaseDate { get; set; }
        public ICollection<Genre> Genres { get; set; }

        internal Movie()
        {
        }

        public Movie(string imdbId, string title, decimal rating, string director, DateTime releaseDate, string plot, IEnumerable<Genre> genres)
        {
            ImdbId = imdbId;
            Title = title;
            Rating = rating;
            Director = director;
            ReleaseDate = releaseDate;
            Plot = plot;

            SetGenres(genres);
        }

        public string ImdbId
        {
            get { return _imdbId; }
            set
            {
                Condition.Requires(value, "imdbId").IsNotNullOrWhiteSpace().HasLength(9);
                _imdbId = value;
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                Condition.Requires(value, "title").IsNotNullOrWhiteSpace().IsShorterOrEqual(128);
                _title = value;
            }
        }

        public decimal Rating
        {
            get { return _rating; }
            set
            {
                Condition.Requires(value, "rating").IsInRange(0m, 10m);
                _rating = value;
            }
        }

        public string Director
        {
            get { return _director; }
            set
            {
                Condition.Requires(value, "director").IsNotNullOrWhiteSpace().IsShorterOrEqual(64);
                _director = value;
            }
        }

        public string Plot
        {
            get { return _plot; }
            set
            {
                Condition.Requires(value, "plot").IsNotNullOrWhiteSpace().IsShorterOrEqual(256);
                _plot = value;
            }
        }

        public void SetGenres(IEnumerable<Genre> genres)
        {
            Condition.Requires(genres, "genres").IsNotNull().IsLongerThan(0);
            Genres = new HashSet<Genre>(genres);
        }
    }
}