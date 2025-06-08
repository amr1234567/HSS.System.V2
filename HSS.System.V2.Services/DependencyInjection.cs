using Hangfire;
using Hangfire.SqlServer;

using HSS.System.V2.Services.Contracts;
using HSS.System.V2.Services.Helpers;
using HSS.System.V2.Services.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serilog;
using Serilog.Extensions.Hosting;

using static HSS.System.V2.Services.Helpers.AccountServiceHelper;

namespace HSS.System.V2.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceLayerDI(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure Serilog
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders(); // Remove default logging providers
                loggingBuilder.AddSerilog(logger, dispose: true);
            });

            // Explicitly register IDiagnosticContext for request logging
            //services.AddSingleton<IDiagnosticContext, DiagnosticContext>();

            services.AddHangfire(cfg =>
            {
                var hangfireConnection = configuration.GetConnectionString("default");
                cfg.UseSqlServerStorage(hangfireConnection, new SqlServerStorageOptions
                {
                    // Optional tuning:
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.FromSeconds(15),
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                });

                // e.g. set global job filters, serializers, etc.
                cfg.UseSimpleAssemblyNameTypeSerializer();
                cfg.UseRecommendedSerializerSettings();
            });

            services.AddHangfireServer();
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
            services.AddScoped<IGeneralServices, GeneralServices>();
            services.AddScoped<IRadiologyCenterServices, RadiologyCenterServices>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IMedicalLabServices, MedicalLabServices>();

            return services;
        }

        public static WebApplication UseHangfireRoutes(this WebApplication app)
        {
            app.UseHangfireDashboard("/hangfire");
            return app;
        }

        public static WebApplication UseSerilog(this WebApplication app)
        {
            //app.UseSerilogRequestLogging(); // Enable request logging for ASP.NET Core
            return app;
        }

    }
}
