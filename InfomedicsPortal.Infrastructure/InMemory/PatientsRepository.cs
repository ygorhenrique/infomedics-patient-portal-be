using InfomedicsPortal.Core.Patients;

namespace InfomedicsPortal.Infrastructure.InMemory;

public class PatientsRepository : BaseRepository<Patient, Guid>, IPatientsStorage
{
    public PatientsRepository() : base(patient => patient.Id)
    {
    }

    public async Task<Patient> AddPatientAsync(Patient patient)
    {
        if (patient.Id == Guid.Empty)
        {
            patient.Id = Guid.NewGuid();
        }

        return await AddAsync(patient);
    }

    public async Task<Patient?> GetPatientByIdAsync(Guid id)
    {
        return await GetByIdAsync(id);
    }
    
    public async Task<Patient[]> GetAllPatientsAsync()
    {
        var getAllResult = await GetAllAsync();
        return getAllResult.ToArray();
    }
}