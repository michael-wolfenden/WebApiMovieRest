using System;
using NSubstitute;
using WebApiMovieRest.Infrastructure.Handlers;
using WebApiMovieRest.Infrastructure.Persistence;
using Xbehave;
using Xunit;

namespace WebApiMovieRest.Infrastructure.Tests.Handlers
{
    public class UnitOfWorkHandlerTests
    {
        [Scenario]
        public void Should_call_commit_if_decorated_handler_handles_request_successfully(
            IUnitOfWork unitOfWork,
            IRequestHandler<Request, Response> handler,
            IRequestHandler<Request, Response> unitOfWorkHandler)
        {
            "Given an IUnitOfWork"
                .Given(() => unitOfWork = Substitute.For<IUnitOfWork>());

            "And an IRequestHandler that will process the request successfully"
                .And(() => handler = Substitute.For<IRequestHandler<Request, Response>>());

            "And an UnitOfWorkHandler constructed with the IUnitOfWork and the IRequestHandler"
                .And(() => unitOfWorkHandler = new UnitOfWorkHandler<Request, Response>(unitOfWork, handler));

            "After handling a Request"
                .When(() => unitOfWorkHandler.Handle(new Request()));

            "Then the IUnitOfWork should be commited"
                .Then(() => unitOfWork.Received().Commit());
        }

        [Scenario]
        public void Should_not_call_commit_if_an_exception_occurs_when_decorated_handler_handles_request(
            IUnitOfWork unitOfWork,
            IRequestHandler<Request, Response> handler,
            IRequestHandler<Request, Response> unitOfWorkHandler)
        {
            "Given an IUnitOfWork"
                .Given(() => unitOfWork = Substitute.For<IUnitOfWork>());

            "And an IRequestHandler that will thow an exception"
                .And(() =>
                {
                    handler = Substitute.For<IRequestHandler<Request, Response>>();

                    handler
                        .Handle(Arg.Any<Request>())
                        .Returns(_ => { throw new Exception(); });
                });

            "And an UnitOfWorkHandler constructed with the IUnitOfWork and the IRequestHandler"
                .And(() => unitOfWorkHandler = new UnitOfWorkHandler<Request, Response>(unitOfWork, handler));

            "After handling a Request"
                .When(() => Record.Exception(() => unitOfWorkHandler.Handle(new Request())));

            "Then the IUnitOfWork should be commited"
                .Then(() => unitOfWork.DidNotReceive().Commit());
        }
        
        public class Request : IRequest<Response> { }
        public class Response { }
    } 
}