using InfomedicsPortal.Core.Patients;

namespace InfomedicsPortal.Core.Appointments;

public class AppointmentsService
{
    private readonly IAppointmentsStorage _appointmentsStorage;
    private readonly IPatientsStorage _patientsStorage;

    public AppointmentsService(
        IAppointmentsStorage appointmentsStorage,
        IPatientsStorage patientsStorage)
    {
        _appointmentsStorage = appointmentsStorage;
        _patientsStorage = patientsStorage;
    }

    public async Task<ExecutionResult<Appointment>> AddAppointmentAsync(NewAppointmentRequest newAppointmentRequest)
    {
        if (newAppointmentRequest.AppointmentDateTime < DateTime.UtcNow)
        {
            return ExecutionResult<Appointment>.Failure("Appointment date cannot be in the past");
        }
        
        if (newAppointmentRequest.PatientId == Guid.Empty)
        {
            return ExecutionResult<Appointment>.Failure("Patient ID cannot be empty");
        }

        if (newAppointmentRequest.DentistId == Guid.Empty)
        {
            return ExecutionResult<Appointment>.Failure("Dentist ID cannot be empty");
        }
        
        if (newAppointmentRequest.TreatmentId == Guid.Empty)
        {
            return ExecutionResult<Appointment>.Failure("Treatment ID cannot be empty");
        }
        
        var addResult = await _appointmentsStorage.AddAppointmentAsync(new()
        {
            Id = Guid.NewGuid(),
            PatientId = newAppointmentRequest.PatientId,
            DentistId = newAppointmentRequest.DentistId,
            AppointmentDateTime = newAppointmentRequest.AppointmentDateTime,
            TreatmentId = newAppointmentRequest.TreatmentId,
            CreationDateUtc = DateTime.UtcNow,
            LastModifiedDateUtc = DateTime.UtcNow
        });
        
        return ExecutionResult<Appointment>.Success(addResult);
    }
    
    public async IAsyncEnumerable<PatientAppointment> GetAllAppointmentAsync()
    {
        var addResult = await _appointmentsStorage.GetAllAppointmentsAsync();
        foreach (var appointment in addResult)
        {
            var patient = await _patientsStorage.GetPatientByIdAsync(appointment.PatientId);
            if (patient != null)
            {
                yield return new PatientAppointment(appointment, patient);
            }
        }
    }
    
    public async Task<Appointment[]?> GetAppointmentsByPatientIdAsync(Guid patientId)
    {
        var patient = await _patientsStorage.GetPatientByIdAsync(patientId);
        if (patient == null)
        {
            return null;
        }
        
        var appointmentsResult = await _appointmentsStorage.GetAppointmentsByPatientId(patient.Id);
        return appointmentsResult;
    }

    public readonly struct PatientAppointment
    {
        public Appointment Appointment { get; }
        public Patient Patient { get; }
        
        public PatientAppointment(Appointment appointment, Patient patient)
        {
            Appointment = appointment;
            Patient = patient;
        }
    }
}