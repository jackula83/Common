using Framework2.Infra.Data.Entity;
using MediatR;

namespace Framework2.Infra.Data.UnitTests.Tests.Context.Stubs
{
    public class DomainEventStub : FxDomainEvent, INotification
    {
        public DomainEventStub(string hello) => Hello = hello;

        public string Hello { get; private set; } 
    }
}
