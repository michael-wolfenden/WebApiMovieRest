using System;
using System.Linq;
using FluentAssertions;
using FluentValidation;
using SimpleInjector;
using WebApiMovieRest.Infrastructure.Handlers;
using Xbehave;
using Xunit;

namespace WebApiMovieRest.Infrastructure.Tests.Handlers
{
    public class ValidationHandlerTests
    {
        [Scenario]
        public void Should_throw_validation_exception_if_Request_is_null(
            IRequestHandler<Request, Response> validationHandler,
            Exception exception
            )
        {
            "Given a ValidationHandler"
                .Given(() => validationHandler = new ValidationHandler<Request, Response>(new Container(), new RequestHandler()));

            "When handling a null Request"
                .When(() => exception = Record.Exception(() => validationHandler.Handle(null)));

            "Then a ValidationException should be thrown"
                .Then(() => exception.Should().BeOfType<ValidationException>());

            "And the error property should be 'Request'"
                .Then(() => exception.As<ValidationException>().Errors.Single().PropertyName.Should().Be("Request"));

            "And the error message should be 'Request cannot be null'"
                .Then(() => exception.As<ValidationException>().Errors.Single().ErrorMessage.Should().Be("Request cannot be null"));
        }

        [Scenario]
        public void Should_not_throw_a_validation_exception_if_validator_is_not_found_but_Request_in_invalid(
            IRequestHandler<Request, Response> validationHandler,
            Request invalidRequest,
            Container Container,
            Exception exception
        )
        {
            "Given an invalid Request"
                .Given(() => invalidRequest = new Request() { Name = null });

            "And a Container"
                .And(() => Container = new Container());

            "With no validators registered"
                .And(() => { });

            "And a ValidationHandler constructed with the Container"
                .And(() => validationHandler = new ValidationHandler<Request, Response>(Container, new RequestHandler()));

            "When handling the invalid Request"
                .When(() => exception = Record.Exception(() => validationHandler.Handle(invalidRequest)));

            "A ValidationException should not be thrown"
                .Then(() => exception.Should().BeNull());
        }

        [Scenario]
        public void Should_not_throw_a_validation_exception_if_validator_is_found_but_Request_is_valid(
            IRequestHandler<Request, Response> validationHandler,
            Request validRequest,
            Container Container,
            Exception exception
        )
        {
            "Given a valid Request"
                .Given(() => validRequest = new Request() { Name = "A valid name" });

            "And a Container"
                .And(() => Container = new Container());

            "With an IValidator for the Request registered"
                .And(() => Container.Register<IValidator<Request>, RequestValidator>());

            "And a ValidationHandler constructed with the Container"
                .And(() => validationHandler = new ValidationHandler<Request, Response>(Container, new RequestHandler()));

            "When handling the valid Request"
                .When(() => exception = Record.Exception(() => validationHandler.Handle(validRequest)));

            "A ValidationException should not be thrown"
                .When(() => exception.Should().BeNull());
        }

        [Scenario]
        public void Should_throw_a_validation_exception_if_validator_is_found_and_Request_is_invalid(
            IRequestHandler<Request, Response> validationHandler,
            Request invalidRequest,
            Container Container,
            Exception exception
        )
        {
            "Given an invalid Request"
                .Given(() => invalidRequest = new Request() { Name = null });

            "And a Container"
                .And(() => Container = new Container());

            "And an IValidator for the Request registered"
                .And(() => Container.Register<IValidator<Request>, RequestValidator>());

            "And a ValidationHandler constructed with the Container"
                .And(() => validationHandler = new ValidationHandler<Request, Response>(Container, new RequestHandler()));

            "When handling the invalid Request"
                .When(() => exception = Record.Exception(() => validationHandler.Handle(invalidRequest)));

            "A ValidationException should be thrown"
                .Then(() => exception.Should().BeOfType<ValidationException>());

            "And the error property should be 'Name'"
                .Then(() => exception.As<ValidationException>().Errors.Single().PropertyName.Should().Be("Name"));

            "And the error message should be 'Name cannot be null'"
                .Then(() => exception.As<ValidationException>().Errors.Single().ErrorMessage.Should().Be("Name cannot be null"));
        }

        public class Request : IRequest<Response>
        {
            public string Name { get; set; }
        }

        public class Response { }

        public class RequestHandler : IRequestHandler<Request, Response>
        {
            public Response Handle(Request request)
            {
                return new Response();
            }
        }

        public class RequestValidator : AbstractValidator<Request>
        {
            public RequestValidator()
            {
                RuleFor(r => r.Name)
                    .NotNull()
                    .WithMessage("Name cannot be null");
            }
        }
    }
}
