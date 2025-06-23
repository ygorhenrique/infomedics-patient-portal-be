namespace InfomedicsPortal.Core.Patients;

public interface IPatientsService
{
    Task<PatientsService.PatientWithAppointments?> GetPatientAsync(Guid patientId);
    Task<ExecutionResult<Patient>> AddPatientAsync(NewPatientRequest patient);
    Task<Patient[]> GetAllPatientsAsync();
}