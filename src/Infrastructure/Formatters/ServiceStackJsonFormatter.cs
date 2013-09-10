using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using ServiceStack.Text;

namespace WebApiMovieRest.Infrastructure.Formatters
{
    public class ServiceStackJsonFormatter : BufferedMediaTypeFormatter
    {
        public ServiceStackJsonFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            SupportedEncodings.Add(new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true));
        }

        public override object ReadFromStream(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            using(new ConfigurationScope())
            {
                return JsonSerializer.DeserializeFromStream(type, readStream);
            }
        }

        public override void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
        {
            using (new ConfigurationScope())
            {
                JsonSerializer.SerializeToStream(value, type, writeStream);
            }
        }

        public override bool CanReadType(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            return true;
        }

        public override bool CanWriteType(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            return true;
        }

        public void SetJsConfigSettings(JsConfigScope scope)
        {
            scope.DateHandler = JsonDateHandler.ISO8601;
            scope.EmitCamelCaseNames = true;
        }

        public class ConfigurationScope : IDisposable
        {
            private readonly JsConfigScope _jsConfigScope;

            public ConfigurationScope()
            {
                _jsConfigScope = JsConfig.BeginScope();
                _jsConfigScope.DateHandler = JsonDateHandler.ISO8601;
                _jsConfigScope.EmitCamelCaseNames = true;

                // serialize guids with dashes
                JsConfig<Guid>.SerializeFn = guid => guid.ToString("D");
            }

            public void Dispose()
            {
                if (_jsConfigScope != null)
                {
                    _jsConfigScope.Dispose();
                }

                JsConfig<Guid>.SerializeFn = null;
            }
        }
    }
}