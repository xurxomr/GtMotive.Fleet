using Xunit;

namespace GtMotive.Fleet.InfrastructureTests.Infrastructure
{
    [Collection(TestCollections.TestServer)]
    public abstract class InfrastructureTestBase(GenericInfrastructureTestServerFixture fixture)
    {
        protected GenericInfrastructureTestServerFixture Fixture { get; } = fixture;
    }
}
