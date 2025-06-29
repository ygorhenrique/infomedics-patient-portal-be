namespace InfomedicsPortal.Core.Appointments;

public class Appointment
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid DentistId { get; set; }
    public DateTime AppointmentDateTime { get; set; }
    public Guid TreatmentId { get; set; }
    public DateTime CreationDateUtc { get; set; }
    public DateTime LastModifiedDateUtc { get; set; }
    public string Status { get; set; } = "scheduled";

    // possible enhacements
    // public status Scheduled (scheduled, done, noshow)
}