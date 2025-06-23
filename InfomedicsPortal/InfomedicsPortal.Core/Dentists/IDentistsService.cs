namespace InfomedicsPortal.Core.Dentists;

public interface IDentistsService
{
    Task<Dentist[]> GetAllDentistsAsync();
}