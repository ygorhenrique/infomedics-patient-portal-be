namespace InfomedicsPortal.Core.Patients;

public interface IPatientsStorage
{
    public Task<Patient> AddPatientAsync(Patient patient);
    public Task<Patient?> GetPatientByIdAsync(Guid patientId);

    public Task<Patient[]> GetAllPatientsAsync();
}