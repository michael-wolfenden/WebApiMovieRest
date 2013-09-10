using System;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using FluentAssertions;
using WebApiMovieRest.Core.StartupTasks;
using WebApiMovieRest.Infrastructure.Formatters;
using Xbehave;
using Xunit.Extensions;

namespace WebApiMovieRest.Core.Tests.StartupTasks
{
    public class RegisterFormattersTests
    {
        [Scenario]
        [InlineData(typeof(XmlMediaTypeFormatter), "xml", "application/xml")]
        [InlineData(typeof(ServiceStackJsonFormatter), "json", "application/json")]
        public void When_starting_the_register_formatters_task(Type formatterType, string matchValue, string mimeType)
        {
            var httpConfiguration = default(HttpConfiguration);
            var registerFormatters = default(RegisterFormatters);

            "Given a HttpConfiguration"
                .Given(() => httpConfiguration = new HttpConfiguration());

            "And a RegisterFormatters task"
                .And(() => registerFormatters = new RegisterFormatters(httpConfiguration));

            "After starting the task"
                .When(() => registerFormatters.Start());

            ("Then should register " + formatterType.Name)
                .Then(() => httpConfiguration.Formatters.Should().Contain(x => x.GetType() == formatterType));

            ("And " + formatterType.Name + " should map to extension '" + matchValue + "'")
                .Then(() => GetUriPathExtensionMappingFor(httpConfiguration, formatterType).UriPathExtension.Should().Be(matchValue));

            ("And " + formatterType.Name + " should map to mimetype '" + mimeType + "'")
                .Then(() => GetUriPathExtensionMappingFor(httpConfiguration, formatterType).MediaType.MediaType.Should().Be(mimeType));

            "And the default json formatter should be removed"
                .Then(() => httpConfiguration.Formatters.Should().NotContain(x => x.GetType() == typeof(JsonMediaTypeFormatter)));
        }

        private UriPathExtensionMapping GetUriPathExtensionMappingFor(HttpConfiguration configuration, Type formatterType)
        {
            return configuration
                .Formatters
                .Single(mediaTypeFormatter => mediaTypeFormatter.GetType() == formatterType)
                .MediaTypeMappings.OfType<UriPathExtensionMapping>()
                .Single();
        }
    }
}