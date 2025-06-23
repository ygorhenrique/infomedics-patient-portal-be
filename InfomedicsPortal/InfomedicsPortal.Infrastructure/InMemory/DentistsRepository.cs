using InfomedicsPortal.Core.Dentists;

namespace InfomedicsPortal.Infrastructure.InMemory;

public class DentistsRepository : IDentistsStorage
{
    public Task<Dentist[]> GetAllDentistsAsync()
    {
        return Task.FromResult(
            new[] {
                new Dentist() { Id = Guid.NewGuid(), Name = "Bruce Wayne" },
                new Dentist() { Id = Guid.NewGuid(), Name = "Selina Kyle" },
                new Dentist() { Id = Guid.NewGuid(), Name = "Jack Oswald White" },
            }
        );
    }
}