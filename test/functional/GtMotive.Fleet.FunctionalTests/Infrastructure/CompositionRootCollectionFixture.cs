using Xunit;

namespace GtMotive.Fleet.FunctionalTests.Infrastructure
{
    [CollectionDefinition(TestCollections.Functional)]
    public class CompositionRootCollectionFixture : ICollectionFixture<CompositionRootTestFixture>
    {
    }
}
