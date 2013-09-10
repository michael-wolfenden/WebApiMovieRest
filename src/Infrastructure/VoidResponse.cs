namespace WebApiMovieRest.Infrastructure
{
    public sealed class VoidResponse
    {
        static readonly VoidResponse _instance = new VoidResponse();

        static VoidResponse()
        {
        }

        private VoidResponse()
        {
        }

        public static VoidResponse Instance
        {
            get { return _instance; }
        }
    }
}