using Microsoft.Extensions.DependencyInjection;

namespace GtMotive.Fleet.Api.DependencyInjection
{
    public static class UserInterfaceExtensions
    {
        public static IServiceCollection AddPresenters(this IServiceCollection services)
        {
            return services;
        }
    }
}
