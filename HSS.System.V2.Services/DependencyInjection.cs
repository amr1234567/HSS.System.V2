using HSS.System.V2.Services.Contracts;
using HSS.System.V2.Services.Helpers;
using HSS.System.V2.Services.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using static HSS.System.V2.Services.Helpers.AccountServiceHelper;

namespace HSS.System.V2.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceLayerDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            services.AddHostedService<QueuedHostedService>();
            services.AddHttpContextAccessor();
            services.AddScoped<IUserContext, UserContext>();
            services.AddScoped<AccountServiceHelper>();
            services.AddScoped<TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPatientService, PatientServices>();
            services.AddScoped<IClinicServices, ClinicServices>();
            services.AddScoped<IReceptionServices, ReceptionServices>();

            return services;
        }

    }
}
