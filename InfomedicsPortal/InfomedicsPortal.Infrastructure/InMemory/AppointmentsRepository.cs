using InfomedicsPortal.Core.Appointments;

namespace InfomedicsPortal.Infrastructure.InMemory;

public class AppointmentsRepository : BaseRepository<Appointment, Guid>, IAppointmentsStorage
{
    public AppointmentsRepository() : base(appointment => appointment.Id)
    {
    }

    public async Task<Appointment> AddAppointmentAsync(Appointment appointment)
    {
        if (appointment.Id == Guid.Empty)
        {
            appointment.Id = Guid.NewGuid();
        }

        return await AddAsync(appointment);
    }
    
    public async Task<Appointment[]> GetAppointmentsByPatientId(Guid patientId)
    {
        var allAppointmentsResult = await GetAllAppointmentsAsync();
        var allAppointmentsForPatient = allAppointmentsResult.Where(a => a.PatientId == patientId);
        return allAppointmentsForPatient.ToArray();
    }

    public async Task<Appointment[]> GetAllAppointmentsAsync()
    {
        var getAllResult = await GetAllAsync();
        return getAllResult.ToArray();
    }
}