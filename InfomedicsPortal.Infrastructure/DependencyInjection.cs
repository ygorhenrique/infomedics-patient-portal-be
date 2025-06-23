using InfomedicsPortal.Core.Appointments;
using InfomedicsPortal.Core.Dentists;
using InfomedicsPortal.Core.Patients;
using InfomedicsPortal.Core.Treatments;
using InfomedicsPortal.Infrastructure.InMemory;
using Microsoft.Extensions.DependencyInjection;

namespace InfomedicsPortal.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IAppointmentsStorage, AppointmentsRepository>();
        services.AddSingleton<IDentistsStorage, DentistsRepository>();
        services.AddSingleton<IPatientsStorage, PatientsRepository>();
        services.AddSingleton<ITreatmentsStorage, TreatmentsRepository>();
    }
}