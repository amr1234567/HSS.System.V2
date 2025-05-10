
using HSS.System.V2.DataAccess.Contexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace HSS.System.V2.DataAccess;

public static class DependencyInjection
{
    public static IServiceCollection AddContextDI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(optionsBuilder =>
        {
            var connectionString = configuration.GetConnectionString("default-mido");
            optionsBuilder.UseSqlServer(connectionString);
        });
        return services;
    }
}
