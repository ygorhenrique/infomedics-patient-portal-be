namespace InfomedicsPortal.Core.Patients;

public class PatientsService
{
    private readonly IPatientsStorage _storage;

    public PatientsService(IPatientsStorage storage)
    {
        this._storage = storage;
    }
    
    public async Task<Patient?> GetPatientAsync(Guid patientId)
    {
        var getPatientResult = await _storage.GetPatientByIdAsync(patientId);
        return getPatientResult;
    }

    public async Task<ExecutionResult<Patient>> AddPatient(Patient patient)
    {
        if (string.IsNullOrEmpty(patient.FullName))
        {
            return ExecutionResult<Patient>.Failure("Full name is required");
        }
        
        if (string.IsNullOrEmpty(patient.Address))
        {
            return ExecutionResult<Patient>.Failure("Address is required");
        }
        
        if (string.IsNullOrEmpty(patient.Photo))
        {
            return ExecutionResult<Patient>.Failure("Photo is required");
        }

        var addResult = await _storage.AddPatientAsync(patient);
        return ExecutionResult<Patient>.Success(addResult);
    }
    
    public async Task<Patient[]> GetAllPatientsAsync()
    {
        var getPatientResult = await _storage.GetAllPatientsAsync();
        return getPatientResult;
    }
}