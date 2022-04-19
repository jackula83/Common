using Common.Domain.Core.Interfaces;
using Common.Domain.Tests.Unit.Models.Stubs;

namespace Common.Domain.UnitTests.Interfaces
{
    internal interface IEntityRepositoryStub : IEntityRepository<EntityStub>
    {
    }
}
