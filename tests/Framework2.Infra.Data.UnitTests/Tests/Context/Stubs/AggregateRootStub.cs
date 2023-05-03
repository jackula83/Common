using Framework2.Infra.Data.Entity;

namespace Framework2.Infra.Data.UnitTests.Tests.Context.Stubs
{
    public class AggregateRootStub : FxAggregateRoot
    {
        public void Work(int eventsToAdd = 1)
        {
            for (int i = 0; i < eventsToAdd; ++i)
                AddDomainEvent(new DomainEventStub((i + 1).ToString()));
        }
    }
}
