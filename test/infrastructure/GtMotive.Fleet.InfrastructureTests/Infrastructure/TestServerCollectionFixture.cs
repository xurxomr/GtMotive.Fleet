using Xunit;

namespace GtMotive.Fleet.InfrastructureTests.Infrastructure
{
    [CollectionDefinition(TestCollections.TestServer)]
    public class TestServerCollectionFixture : ICollectionFixture<GenericInfrastructureTestServerFixture>
    {
    }
}
