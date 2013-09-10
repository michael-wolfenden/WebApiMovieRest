using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using FluentAssertions;
using ServiceStack.Text;
using WebApiMovieRest.Infrastructure.Formatters;
using Xbehave;
using Xunit;

namespace WebApiMovieRest.Infrastructure.Tests.Formatters
{
    public class ServiceStackJsonFormatterTests
    {
        [Scenario]
        public void Should_support_only_json_media_type(
            ServiceStackJsonFormatter formatter)
        {
            "Given a ServiceStackJsonFormatter"
                .Given(() => formatter = new ServiceStackJsonFormatter());

            "Should only support json media types"
                .Then(() => formatter.SupportedMediaTypes.Single().Should().Be(new MediaTypeHeaderValue("application/json")));
        }

        [Scenario]
        public void Should_support_only_utf8_encoding(
            ServiceStackJsonFormatter formatter)
        {
            "Given a ServiceStackJsonFormatter"
                .Given(() => formatter = new ServiceStackJsonFormatter());

            "Should only support utf8 encoding"
                .Then(() => formatter.SupportedEncodings.Single().Should().BeOfType<UTF8Encoding>());
        }

        [Scenario]
        public void Should_format_dates_as_iso8601(
            ServiceStackJsonFormatter formatter,
            DateTime dateTime,
            string serializedString)
        {
            "Given a ServiceStackJsonFormatter"
                .Given(() => formatter = new ServiceStackJsonFormatter());

            "And a DateTime"
                .And(() => dateTime = new DateTime(1994, 11, 24, 0, 0, 0, DateTimeKind.Utc));

            "When serializing the DateTime"
                .When(() => serializedString = formatter.Serialize(dateTime));

            "Then the DateTime value should be serialized as iso8601"
                .Then(() => serializedString.Should().Be("\"1994-11-24T00:00:00.0000000Z\""));

            "But setting should only apply when using the ServiceStackJsonFormatter"
                .But(() => dateTime.ToJson().Should().Be("\"\\/Date(785635200000)\\/\""));
        }

        [Scenario]
        public void Should_emit_camel_case_names(
            ServiceStackJsonFormatter formatter,
            string serializedString)
        {
            "Given a ServiceStackJsonFormatter"
                .Given(() => formatter = new ServiceStackJsonFormatter());

            "When serializing an object with a pascal cased property names"
                .When(() => serializedString = formatter.Serialize(new { PropertyName = "value"}));

            "Then property name should be serialized as camel case"
                .Then(() => serializedString.Should().Be("{\"propertyName\":\"value\"}"));

            "But setting should only apply when using the ServiceStackJsonFormatter"
                .But(() => new { PropertyName = "value" }.ToJson().Should().Be("{\"PropertyName\":\"value\"}"));
        }

        [Scenario]
        public void CanReadType_throws_if_type_is_null(
            ServiceStackJsonFormatter formatter,
            Exception exception)
        {
            "Given a ServiceStackJsonFormatter"
                .Given(() => formatter = new ServiceStackJsonFormatter());
            
            "When calling CanReadType with a null type"
                .When(() => exception = Record.Exception(() => formatter.CanReadType(null)));

            "Then an ArgumentNullException should be thrown"
                .Then(() => exception.Should().BeOfType<ArgumentNullException>());

            "With a parameter name of 'type'"
                .Then(() => exception.As<ArgumentNullException>().ParamName.Should().Be("type"));
        }

        [Scenario]
        public void CanWriteType_throws_if_type_is_null(
            ServiceStackJsonFormatter formatter,
            Exception exception)
        {
            "Given a ServiceStackJsonFormatter"
                .Given(() => formatter = new ServiceStackJsonFormatter());

            "When calling CanWriteType with a null type"
                .When(() => exception = Record.Exception(() => formatter.CanWriteType(null)));

            "Then an ArgumentNullException should be thrown"
                .Then(() => exception.Should().BeOfType<ArgumentNullException>());

            "With a parameter name of 'type'"
                .Then(() => exception.As<ArgumentNullException>().ParamName.Should().Be("type"));
        }

        [Scenario]
        public void Should_format_Guids_with_dashes(
            ServiceStackJsonFormatter formatter,
            string serializedGuid)
        {
            "Given a ServiceStackJsonFormatter"
                .Given(() => formatter = new ServiceStackJsonFormatter());

            "When serializing a Guid"
                .When(() => serializedGuid = formatter.Serialize(Guid.Empty));

            "Then Guid should be serialized with dashes"
                .Then(() => serializedGuid.Should().Be("\"00000000-0000-0000-0000-000000000000\""));

            "But setting should only apply when using the ServiceStackJsonFormatter"
                .But(() => Guid.Empty.ToJson().Should().Be("\"00000000000000000000000000000000\""));
        }

        [Scenario]
        public void Should_use_service_stack_to_serialize_object_to_stream(
            ServiceStackJsonFormatter formatter,
            ClassWithAllTypes objectToSerialize,
            string expectedSerializedObject,
            string actualSerializedObject
            )
        {
            "Given a ServiceStackJsonFormatter"
                .Given(() => formatter = new ServiceStackJsonFormatter());

            "And an object to serialize"
                .And(() => objectToSerialize = ClassWithAllTypes.Create(1));

            "And the object serialized using service stack directly"
                .And(() =>
                {
                    // use same configuration options as ServiceStackJsonFormatter for the ToJson call
                    using (new ServiceStackJsonFormatter.ConfigurationScope())
                    {
                        expectedSerializedObject = objectToSerialize.ToJson();
                    }
                });

            "When serializing using the formatter"
                .When(() => actualSerializedObject = formatter.Serialize(objectToSerialize));

            "Should result in the same serialized object"
                .Then(() => actualSerializedObject.Should().Be(expectedSerializedObject));
        }

        [Scenario]
        public void Should_use_service_stack_to_deserialise_object_from_stream(
            ServiceStackJsonFormatter formatter,
            ClassWithAllTypes objectToSerialize,
            byte[] serializedObjectAsBytes,
            ClassWithAllTypes objectAfterDeserializing
            )
        {
            "Given a ServiceStackJsonFormatter"
                .Given(() => formatter = new ServiceStackJsonFormatter());

            "And an object to serialize"
                .And(() => objectToSerialize = ClassWithAllTypes.Create(1));

            "And the object serialized to bytes using service stack directly"
                .And(() =>
                {
                    // use same configuration options as ServiceStackJsonFormatter for the ToJson call
                    using (new ServiceStackJsonFormatter.ConfigurationScope())
                    {
                        // as well as the same encoding
                        var encoding = formatter.SupportedEncodings.Single();
                        serializedObjectAsBytes = encoding.GetBytes(objectToSerialize.ToJson());
                    }
                });

            "When deserializing the serialized bytes using the formatter"
                .When(() => objectAfterDeserializing = formatter.Deserialize<ClassWithAllTypes>(serializedObjectAsBytes));

            "Should result in the same object that was serialized"
                .Then(() => objectToSerialize.Should().Be(objectAfterDeserializing));
        }
    }

    public static class ServiceStackJsonFormatterExtensions
    {
        public static string Serialize<T>(this ServiceStackJsonFormatter formatter, T objectToSerialize)
        {
            string serializedObject = null;
            using (var stream = new MemoryStream())
            {
                var content = new StringContent(string.Empty);

                formatter.WriteToStream(typeof(T), objectToSerialize, stream, content);
                serializedObject = Encoding.UTF8.GetString(stream.ToArray());
            }

            return serializedObject;
        }

        public static T Deserialize<T>(this ServiceStackJsonFormatter formatter, byte[] bytes)
        {
            T deserializedObject;
            using (var stream = new MemoryStream(bytes))
            {
                var content = new StringContent(string.Empty);
                deserializedObject = (T)formatter.ReadFromStream(typeof(ClassWithAllTypes), stream, content, null);
            }

            return deserializedObject;
        }
    }

    public class ClassWithAllTypes
    {
        public char CharValue { get; set; }
        public byte ByteValue { get; set; }
        public sbyte SByteValue { get; set; }
        public short ShortValue { get; set; }
        public ushort UShortValue { get; set; }
        public int IntValue { get; set; }
        public uint UIntValue { get; set; }
        public long LongValue { get; set; }
        public ulong ULongValue { get; set; }
        public float FloatValue { get; set; }
        public double DoubleValue { get; set; }
        public decimal DecimalValue { get; set; }
        public DateTime DateTimeValue { get; set; }
        public Guid GuidValue { get; set; }
        public string StringValue { get; set; }

        public static ClassWithAllTypes Create(byte i)
        {
            return new ClassWithAllTypes
            {
                ByteValue = i,
                CharValue = (char)i,
                DateTimeValue = new DateTime(i),
                DecimalValue = i,
                DoubleValue = i,
                FloatValue = i,
                IntValue = i,
                LongValue = i,
                SByteValue = (sbyte)i,
                ShortValue = i,
                UIntValue = i,
                ULongValue = i,
                UShortValue = i,
                GuidValue = Guid.NewGuid(),
                StringValue = new String((char)i, 1)
            };
        }

        protected bool Equals(ClassWithAllTypes other)
        {
            return CharValue == other.CharValue &&
                   ByteValue == other.ByteValue &&
                   SByteValue == other.SByteValue &&
                   ShortValue == other.ShortValue &&
                   UShortValue == other.UShortValue &&
                   IntValue == other.IntValue &&
                   UIntValue == other.UIntValue &&
                   LongValue == other.LongValue &&
                   ULongValue == other.ULongValue &&
                   FloatValue.Equals(other.FloatValue) &&
                   DoubleValue.Equals(other.DoubleValue) &&
                   DecimalValue == other.DecimalValue &&
                   DateTimeValue.Equals(other.DateTimeValue) &&
                   GuidValue.Equals(other.GuidValue) &&
                   StringValue.Equals(other.StringValue);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ClassWithAllTypes)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = CharValue.GetHashCode();
                hashCode = (hashCode * 397) ^ ByteValue.GetHashCode();
                hashCode = (hashCode * 397) ^ SByteValue.GetHashCode();
                hashCode = (hashCode * 397) ^ ShortValue.GetHashCode();
                hashCode = (hashCode * 397) ^ UShortValue.GetHashCode();
                hashCode = (hashCode * 397) ^ IntValue;
                hashCode = (hashCode * 397) ^ (int)UIntValue;
                hashCode = (hashCode * 397) ^ LongValue.GetHashCode();
                hashCode = (hashCode * 397) ^ ULongValue.GetHashCode();
                hashCode = (hashCode * 397) ^ FloatValue.GetHashCode();
                hashCode = (hashCode * 397) ^ DoubleValue.GetHashCode();
                hashCode = (hashCode * 397) ^ DecimalValue.GetHashCode();
                hashCode = (hashCode * 397) ^ DateTimeValue.GetHashCode();
                hashCode = (hashCode * 397) ^ GuidValue.GetHashCode();
                hashCode = (hashCode * 397) ^ StringValue.GetHashCode();
                return hashCode;
            }
        }
    }
}