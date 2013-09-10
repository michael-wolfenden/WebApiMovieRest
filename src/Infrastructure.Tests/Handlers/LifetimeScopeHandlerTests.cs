using System;
using FluentAssertions;
using NSubstitute;
using SimpleInjector;
using SimpleInjector.Extensions.LifetimeScoping;
using WebApiMovieRest.Infrastructure.Handlers;
using Xbehave;

namespace WebApiMovieRest.Infrastructure.Tests.Handlers
{
    public class LifetimeScopeHandlerTests
    {
        [Scenario]
        public void Should_wrap_decorated_handler_in_a_lifetime_scope(
            IRequestHandler<Request, Response> lifetimeScopeHandler,
            Container container,
            Response response)
        {
            "Given a Container"
                .Given(() => container = new Container());

            "With a lifetime scoped service registered"
                .And(() => container.Register(() => Substitute.For<IDisposable>(), new LifetimeScopeLifestyle()));

            "And a LifetimeScopeDecorator constructed with the Container and the IRequestHandler"
                .And(() => lifetimeScopeHandler = new LifetimeScopeHandler<Request, Response>(container, () => new RequestHandler(container)));

            "After handling a Request"
                .When(() => response = lifetimeScopeHandler.Handle(new Request()));

            "All the lifetime scoped services resolved by the IRequestHandler should be the same instance"
                .Then(() => response.ALifetimeScopedObject.Should().BeSameAs(response.AnotherLifetimeScopedObject));

            "And all the lifetime scoped services resolved by the IRequestHandler should have be disposed"
                .Then(() => response.ALifetimeScopedObject.Received().Dispose());
        }

        public class Request : IRequest<Response>
        {
        }

        public class Response
        {
            public IDisposable ALifetimeScopedObject { get; set; }
            public IDisposable AnotherLifetimeScopedObject { get; set; }
        }

        public class RequestHandler : IRequestHandler<Request, Response>
        {
            private readonly Container _container;

            public RequestHandler(Container container)
            {
                _container = container;
            }

            public Response Handle(Request request)
            {
                return new Response()
                {
                    ALifetimeScopedObject = _container.GetInstance<IDisposable>(),
                    AnotherLifetimeScopedObject = _container.GetInstance<IDisposable>(),
                };
            }
        }
    }
}