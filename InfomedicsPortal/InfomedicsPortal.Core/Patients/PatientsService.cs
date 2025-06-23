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
        
        // if (string.IsNullOrEmpty(patient.Photo))
        // {
        //     return ExecutionResult<Patient>.Failure("Photo is required");
        // }

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

    public class NewPatientRequest
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Photo { get; set; }
    }
}