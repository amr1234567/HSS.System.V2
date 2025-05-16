using HSS.System.V2.DataAccess.Contexts;
using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.DataAccess.Repositories;

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
            var connectionString = configuration.GetConnectionString("default");
            optionsBuilder.UseSqlServer(connectionString);
        });
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IHospitalRepository, HospitalRepository>();
        services.AddScoped<IMedicalLabTestResultServices, MedicalTestResultServices>();
        services.AddScoped<IMedicineRepository, MedicineRepository>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
        services.AddScoped<IQueueRepository, QueueRepository>();
        services.AddScoped<ITestRequiredRepository, TestRequiredRepository>();
        services.AddScoped<ITestsRepository, TestsRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<ISpecializationReporitory, SpecializationReporitory>();
        services.AddScoped<IMedicalHistoryServices, MedicalHistoryServices>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IMedicalHistoryRepository, MedicalHistoryRepository>();

        return services;
    }
}
