using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;
using FluentAssertions;
using ServiceStack.Text;
using WebApiMovieRest.Core.Domain;
using WebApiMovieRest.Core.Resources.Genres;
using WebApiMovieRest.Core.Resources.Movies;
using WebApiMovieRest.Core.Tests.Builders;
using WebApiMovieRest.Infrastructure.Extensions;
using WebApiMovieRest.Infrastructure.Formatters;
using Xbehave;
using Xunit.Extensions;

namespace WebApiMovieRest.Core.Tests
{
    public class SerializationTests
    {
        [Scenario]
        [PropertyData("Responses")]
        public void Should_be_able_to_serialize_responses_to_json(
            object response,
            string serializedContent)
        {
            "Given a response of type {0}"
                .Given(() => { });

            "When serializing to json"
                .When(() => serializedContent = Serialize(response, (o, stream) =>
                    new ServiceStackJsonFormatter().WriteToStream(o.GetType(), o, stream, new StringContent(""))));
            
            "Then should have serialized successfully".Then(() =>
            {
                Debug.Write(JsvFormatter.Format(serializedContent).Replace("\t", "  "));
                serializedContent.Should().NotBeNullOrEmpty();
            });
        }

        [Scenario]
        [PropertyData("Responses")]
        public void Should_be_able_to_serialize_responses_to_xml(
            object response,
            string serializedContent)
        {
            "Given a response of type {0}"
                .Given(() => { });

            "When serializing to xml"
                .When(() => serializedContent = Serialize(response, (o, stream) =>
                    new DataContractSerializer(o.GetType()).WriteObject(stream, o)));

            "Then should have serialized successfully".Then(() =>
            {
                Debug.WriteLine(XDocument.Parse(serializedContent).ToString());
                serializedContent.Should().NotBeNullOrEmpty();
            });
        }

        private string Serialize(object ojectToSerialize, Action<object, Stream> serializer)
        {
            string serializedContent = null;

            using (var stream = new MemoryStream())
            {
                serializer(ojectToSerialize, stream);
                stream.Seek(0, SeekOrigin.Begin);
                serializedContent = Encoding.UTF8.GetString(stream.ToArray());
            }

            return serializedContent;
        }

        public static IEnumerable<object[]> Responses
        {
            get
            {
                yield return new object[]
                {
                    new Genre("Action")
                        .Yield()
                        .ToResponse()
                };

                yield return new object[]
                {
                    new MovieBuilder()
                        .Build()
                        .Yield()
                        .ToResponse()
                };
            }
        }
    }
}