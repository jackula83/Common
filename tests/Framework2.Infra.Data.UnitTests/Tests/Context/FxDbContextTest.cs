using Framework2.Core.Extensions;
using Framework2.Infra.Data.Entity;
using Framework2.Infra.Data.UnitTests.Tests.Context.Stubs;
using Framework2.Infra.Data.UnitTests.Utilities;
using MediatR;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Framework2.Infra.Data.UnitTests.Tests.Context
{
    public class FxDbContextTest
    {
        private readonly DbContextStub _sut;
        private readonly Mock<IMediator> _mediatorMock;

        public FxDbContextTest()
        {
            _mediatorMock = new();
            _sut = Utils.CreateInMemoryDatabase<DbContextStub>(nameof(FxDbContextTest), _mediatorMock.Object)!;
        }

        [Fact]
        public async Task SaveChangesAsync_WhenThereIsAnAggregateRootWithOneEvent_PublishesEvent()
        {
            var aggregateRoot = new AggregateRootStub();
            aggregateRoot.Work();
            _sut.Add(aggregateRoot);
            await _sut.SaveChangesAsync();

            _mediatorMock.Verify(m => m.Publish(It.IsAny<FxDomainEvent>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task SaveChangesAsync_WhenThereIsAnAggregateRootWithoutEvents_DoesNotPublishEvent()
        {
            var aggregateRoot = new AggregateRootStub();
            _sut.Add(aggregateRoot);
            await _sut.SaveChangesAsync();

            _mediatorMock.Verify(m => m.Publish(It.IsAny<FxDomainEvent>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Fact]
        public async Task SaveChangesAsync_WhenThereIsAnAggregateRootWithThreeEvents_PublishesThreeTimes()
        {
            var aggregateRoot = new AggregateRootStub();
            aggregateRoot.Work(3);
            _sut.Add(aggregateRoot);
            await _sut.SaveChangesAsync();

            _mediatorMock.Verify(m => m.Publish(It.IsAny<FxDomainEvent>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
        }

        [Fact]
        public async Task SaveChangesAsync_WhenThereAreTwoAggregateRootsWithThreeEvents_PublishesSixTimes()
        {
            var aggregateRoot1 = new AggregateRootStub().Tap(s => s.Work(3));
            var aggregateRoot2 = new AggregateRootStub().Tap(s => s.Work(3));
            _sut.Add(aggregateRoot1);
            _sut.Add(aggregateRoot2);
            await _sut.SaveChangesAsync();

            _mediatorMock.Verify(m => m.Publish(It.IsAny<FxDomainEvent>(), It.IsAny<CancellationToken>()), Times.Exactly(6));
        }

        [Fact]
        public async Task SaveChangesAsync_WhenThereIsAnAggregateRootWithOneEventSavedTwice_PublishesOneEventOnly()
        {
            var aggregateRoot = new AggregateRootStub();
            aggregateRoot.Work();
            _sut.Add(aggregateRoot);
            await _sut.SaveChangesAsync();
            await _sut.SaveChangesAsync();

            _mediatorMock.Verify(m => m.Publish(It.IsAny<FxDomainEvent>(), It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
