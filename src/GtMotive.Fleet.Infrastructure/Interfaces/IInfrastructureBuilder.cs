using Microsoft.Extensions.DependencyInjection;

namespace GtMotive.Fleet.Infrastructure.Interfaces
{
    public interface IInfrastructureBuilder
    {
        IServiceCollection Services { get; }
    }
}
