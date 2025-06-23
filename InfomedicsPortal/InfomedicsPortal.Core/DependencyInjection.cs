using InfomedicsPortal.Core.Appointments;
using InfomedicsPortal.Core.Dentists;
using InfomedicsPortal.Core.Patients;
using InfomedicsPortal.Core.Stats;
using InfomedicsPortal.Core.Treatments;
using Microsoft.Extensions.DependencyInjection;

namespace InfomedicsPortal.Core;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAppointmentsService, AppointmentsService>();
        services.AddScoped<IDentistsService, DentistsService>();
        services.AddScoped<IPatientsService, PatientsService>();
        services.AddScoped<ITreatmentsService, TreatmentsService>();
        services.AddScoped<IStatsService, StatsService>();
    }
}