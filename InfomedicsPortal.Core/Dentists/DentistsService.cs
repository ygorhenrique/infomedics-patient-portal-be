namespace InfomedicsPortal.Core.Dentists;

public class DentistsService : IDentistsService
{
    private readonly IDentistsStorage _storage;

    public DentistsService(IDentistsStorage storage)
    {
        this._storage = storage;
    }

    public async Task<Dentist[]> GetAllDentistsAsync()
    {
        var getAllResult = await _storage.GetAllDentistsAsync();
        return getAllResult;
    }
}