using System.Runtime.Serialization;

namespace WebApiMovieRest.Core.Resources.Genres
{
    [DataContract(Name = "Response", Namespace = Constants.Namespace)]
    public class GenresResponse : IExtensibleDataObject 
    {
        [DataMember(Order = 1)]
        public GenreDto[] Genres { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}