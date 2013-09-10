using System;
using System.Runtime.Serialization;

namespace WebApiMovieRest.Core.Resources.Genres
{
    [DataContract(Name = "Genre", Namespace = Constants.Namespace)]
    public class GenreDto : IExtensibleDataObject
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}