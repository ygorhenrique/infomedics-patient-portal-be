namespace InfomedicsPortal.Core.Appointments;

public interface IAppointmentsStorage
{
    public Task<Appointment> AddAppointmentAsync(Appointment appointment);
    public Task<Appointment[]> GetAppointmentsByPatientId(Guid patientId);
    public Task<Appointment[]> GetAllAppointmentsAsync();
}