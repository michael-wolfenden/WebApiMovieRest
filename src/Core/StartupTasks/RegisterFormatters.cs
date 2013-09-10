using System.Net.Http.Formatting;
using System.Web.Http;
using WebApiMovieRest.Infrastructure.Formatters;
using WebApiMovieRest.Infrastructure.IoC;

namespace WebApiMovieRest.Core.StartupTasks
{
    public class RegisterFormatters : IStartable
    {
        private readonly HttpConfiguration _httpConfiguration;

        public RegisterFormatters(HttpConfiguration httpConfiguration)
        {
            _httpConfiguration = httpConfiguration;
        }

        public void Start()
        {
            var xmlFormatter = _httpConfiguration.Formatters.XmlFormatter;
            var jsonFormatter = new ServiceStackJsonFormatter();

           _httpConfiguration.Formatters.Remove(_httpConfiguration.Formatters.JsonFormatter);
           _httpConfiguration.Formatters.Add(jsonFormatter);

           AllowSpecifyingFormatAsExtension(xmlFormatter, "xml", "application/xml");
           AllowSpecifyingFormatAsExtension(jsonFormatter, "json", "application/json");
        }

        private void AllowSpecifyingFormatAsExtension(MediaTypeFormatter formatter, string matchValue, string mimeType)
        {
            formatter.AddUriPathExtensionMapping(matchValue, mimeType);
        }
    }
}