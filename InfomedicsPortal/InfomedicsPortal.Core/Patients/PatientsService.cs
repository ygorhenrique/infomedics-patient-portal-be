using InfomedicsPortal.Core.Appointments;

namespace InfomedicsPortal.Core.Patients;

public partial class PatientsService : IPatientsService
{
    private readonly IPatientsStorage _storage;
    private readonly IAppointmentsService _appointmentsService;

    public PatientsService(
        IAppointmentsService appointmentsService, 
        IPatientsStorage storage)
    {
        this._storage = storage;
        this._appointmentsService = appointmentsService;
    }
    
    public async Task<PatientWithAppointments?> GetPatientAsync(Guid patientId)
    {
        var getPatientResult = await _storage.GetPatientByIdAsync(patientId);
        if (getPatientResult == null)
            return null;
        
        var appointments = await _appointmentsService.GetAppointmentsByPatientIdAsync(patientId) ?? [];
        return new PatientWithAppointments(getPatientResult, appointments);
    }

    public async Task<ExecutionResult<Patient>> AddPatient(NewPatientRequest patient)
    {
        if (string.IsNullOrEmpty(patient.FullName))
        {
            return ExecutionResult<Patient>.Failure("Full name is required");
        }
        
        if (string.IsNullOrEmpty(patient.Address))
        {
            return ExecutionResult<Patient>.Failure("Address is required");
        }

        var addResult = await _storage.AddPatientAsync(new Patient()
        {
            Id = Guid.NewGuid(),
            FullName = patient.FullName,
            Address = patient.Address,  
            Photo = patient.Photo,
            CreatedAtUtc = DateTime.UtcNow,
        });
        return ExecutionResult<Patient>.Success(addResult);
    }
    
    public async Task<Patient[]> GetAllPatientsAsync()
    {
        var getPatientResult = await _storage.GetAllPatientsAsync();
        return getPatientResult;
    }
}