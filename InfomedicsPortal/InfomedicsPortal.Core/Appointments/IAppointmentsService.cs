namespace InfomedicsPortal.Core.Appointments;

public interface IAppointmentsService
{
    Task<ExecutionResult<Appointment>> AddAppointmentAsync(NewAppointmentRequest newAppointmentRequest);
    IAsyncEnumerable<AppointmentsService.PatientAppointment> GetAllAppointmentAsync();
    Task<Appointment[]?> GetAppointmentsByPatientIdAsync(Guid patientId);
}