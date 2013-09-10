using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using FluentAssertions;
using ServiceStack.Text;

namespace WebApiMovieRest.Core.Tests.Helpers
{
    public class RouteTester<TController> : IRouteAssertions<TController>, IRouteValueAssertions where TController : ApiController
    {
        private readonly HttpConfiguration _configuration;
        private Uri _uri;
        private HttpMethod _httpMethod;
        private Expression<Func<TController, HttpResponseMessage>> _action;
        private readonly IDictionary<string, string> _routeValues;
        private bool _shouldFindRoute = true;
        private string _routeName;

        public RouteTester(string baseUrl, HttpConfiguration configuration)
        {
            _uri = new Uri(baseUrl);
            _configuration = configuration;
            _routeValues = new Dictionary<string, string>();
        }

        public IRouteAssertions<TController> GET(string formatUrl, params object[] args)
        {
            return Url(HttpMethod.Get, formatUrl, args);
        }

        public IRouteAssertions<TController> DELETE(string formatUrl, params object[] args)
        {
            return Url(HttpMethod.Delete, formatUrl, args);
        }

        public IRouteAssertions<TController> PUT(string formatUrl, params object[] args)
        {
            return Url(HttpMethod.Put, formatUrl, args);
        }

        public IRouteAssertions<TController> POST(string formatUrl, params object[] args)
        {
            return Url(HttpMethod.Post, formatUrl, args);
        }

        private IRouteAssertions<TController> Url(HttpMethod httpMethod, string formatUrl, params object[] args)
        {
            _uri = new Uri(_uri, string.Format(formatUrl, args));
            _httpMethod = httpMethod;

            return this;
        }

        public IVerify ShouldNotFindRoute()
        {
            _shouldFindRoute = false;
            return this;
        }

        public IRouteValueAssertions ShouldRouteToAction(Expression<Func<TController, HttpResponseMessage>> action)
        {
            _action = action;

            return this;
        }

        public IRouteValueAssertions WithRouteValue(string key, string value)
        {
            _routeValues.Add(key, value);
            return this;
        }

        public IVerify WithRouteName(string routeName)
        {
            _routeName = routeName;
            return this;
        }

        public void Verify()
        {
            var request = new HttpRequestMessage(_httpMethod, _uri);

            var routeData = _configuration.Routes.GetRouteData(request);

            if (_shouldFindRoute)
            {
                if (routeData == null) throw new Exception("Could not route url:" + _uri);

                request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, _configuration);
                request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;

                VerifyControllerType(_configuration, request);
                VerifyActionName(_configuration, request, routeData, _action);
                VerifyRouteValues(routeData, _routeValues);
                VerifyRouteName(request, _routeName);
            }
            else
            {
                routeData.Should()
                         .BeNull();
            }
        }

        private void VerifyRouteName(HttpRequestMessage request, string routeName)
        {
            if (routeName == null) return;

            request.GetUrlHelper()
                .Route(routeName, new Dictionary<string, object>())
                .Should()
                .NotBeNull();
        }

        private void VerifyRouteValues(IHttpRouteData routeData, IDictionary<string, string> routeValues)
        {
            foreach (var routeValue in routeValues)
            {
                if (!routeData.Values.ContainsKey(routeValue.Key)) throw new Exception("Could not find route value for :{0}".FormatWith(routeValue.Key));

                routeData.Values[routeValue.Key]
                    .Should()
                    .Be(routeValue.Value);
            }
        }

        private void VerifyActionName(HttpConfiguration configuration, HttpRequestMessage request, IHttpRouteData routeData, Expression<Func<TController, HttpResponseMessage>> action)
        {
            var controllerDescriptor = CreateControllerDescriptor(configuration, request);
            var httpControllerContext = new HttpControllerContext(_configuration, routeData, request)
                {
                    ControllerDescriptor = controllerDescriptor
                };

            var expectedActionName = (action.Body as MethodCallExpression).Method.Name;

            new ApiControllerActionSelector()
                .SelectAction(httpControllerContext)
                .ActionName
                .Should()
                .Be(expectedActionName);
        }

        private void VerifyControllerType(HttpConfiguration configuration, HttpRequestMessage request)
        {
            CreateControllerDescriptor(configuration, request)
                .ControllerType
                .Should()
                .Be<TController>();
        }

        private HttpControllerDescriptor CreateControllerDescriptor(HttpConfiguration configuration, HttpRequestMessage request)
        {
            return new DefaultHttpControllerSelector(configuration).SelectController(request);
        }
    }

    public interface IRouteAssertions<TController> where TController : ApiController
    {
        IRouteValueAssertions ShouldRouteToAction(Expression<Func<TController, HttpResponseMessage>> action);
        IVerify ShouldNotFindRoute();
    }

    public interface IRouteValueAssertions: IVerify
    {
        IRouteValueAssertions WithRouteValue(string key, string value);
        IVerify WithRouteName(string routeName);
    }

    public interface IVerify
    {
        void Verify();
    }
}