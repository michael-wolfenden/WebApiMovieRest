using System;
using System.Runtime.Serialization;

namespace WebApiMovieRest.Core.Resources.Movies
{
    [DataContract(Name = "Movie", Namespace = Constants.Namespace)]
    public class MovieDto : IExtensibleDataObject 
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }

        [DataMember(Order = 2)]
        public string ImdbId { get; set; }

        [DataMember(Order = 3)]
        public string Title { get; set; }

        [DataMember(Order = 4)]
        public decimal Rating { get; set; }

        [DataMember(Order = 5)]
        public string Director { get; set; }

        [DataMember(Order = 6)]
        public DateTime ReleaseDate { get; set; }

        [DataMember(Order = 7)]
        public string Plot { get; set; }

        [DataMember(Order = 8)]
        public LinkDto Links { get; set; }

        [DataContract(Name = "Link", Namespace = Constants.Namespace)]
        public class LinkDto 
        {
            [DataMember(Order = 1)]
            public Guid[] Genres { get; set; }
        }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}