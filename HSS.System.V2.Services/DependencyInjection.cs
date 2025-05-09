using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HSS.System.V2.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
           
            return services;
        }

    }
}
