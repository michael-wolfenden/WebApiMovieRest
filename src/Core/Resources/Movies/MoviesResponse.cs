using System.Runtime.Serialization;
using WebApiMovieRest.Core.Resources.Genres;

namespace WebApiMovieRest.Core.Resources.Movies
{
    [DataContract(Name = "Response", Namespace = Constants.Namespace)]
    public class MoviesResponse : IExtensibleDataObject 
    {
        [DataMember(Order = 1)]
        public MovieDto[] Movies { get; set; }

        [DataMember(Order = 2)]
        public GenreDto[] Genres { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}