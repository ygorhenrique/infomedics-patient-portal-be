namespace InfomedicsPortal.Core.Dentists;

public interface IDentistsStorage
{
    public Task<Dentist[]> GetAllDentistsAsync();
}