namespace InfomedicsPortal.Core.Appointments;

public class NewAppointmentRequest
{
    public Guid PatientId { get; set; }
    public Guid DentistId { get; set; }
    public DateTime AppointmentDateTime { get; set; }
    public Guid TreatmentId { get; set; }
}